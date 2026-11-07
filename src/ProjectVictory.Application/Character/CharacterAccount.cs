using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

// PV-CHAR-013 (PO-36 a PO-39; R-066): regras de personagem da conta, decididas no SERVIDOR com a conta
// do token (ICurrentPlayerContext). Exceções: 400 (formato), 409 (conflito com o estado da conta).
public sealed class InvalidCharacterRequestException(string message) : Exception(message);

public sealed class CharacterCreationConflictException(string message) : Exception(message);

public sealed class CharacterDeletionConflictException(string message) : Exception(message);

// Dados da conta sobre o personagem que não fazem parte do agregado Character (aparência e exclusão).
public sealed record CharacterProfile(
    Guid CharacterId,
    CharacterGender Gender,
    string? Outfit,
    DateTimeOffset? DeletionRequestedAt,
    DateTimeOffset? DeletionScheduledFor);

public interface ICharacterProfileStore
{
    Task<IReadOnlyDictionary<Guid, CharacterProfile>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);

    Task SetAppearanceAsync(Guid characterId, CharacterGender gender, string outfit, CancellationToken cancellationToken = default);

    Task SetDeletionAsync(Guid characterId, DateTimeOffset? requestedAt, DateTimeOffset? scheduledFor, CancellationToken cancellationToken = default);

    // Finaliza as exclusões vencidas da conta (o personagem some das consultas da conta); devolve quantas.
    Task<int> FinalizeDueDeletionsAsync(Guid accountId, DateTimeOffset now, CancellationToken cancellationToken = default);

    // Nome em uso por personagem não excluído, sem diferenciar maiúsculas (PO-38).
    Task<bool> NameInUseAsync(string name, CancellationToken cancellationToken = default);
}

// Criação de personagem serializada no processo (limite de slots, classe e nome únicos sem corrida).
public sealed class CharacterCreationGate
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<IDisposable> AcquireAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        return new Releaser(_semaphore);
    }

    private sealed class Releaser(SemaphoreSlim semaphore) : IDisposable
    {
        private int _disposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 0)
            {
                semaphore.Release();
            }
        }
    }
}

// Valida a criação contra as regras da conta (chamado pelo CreateCharacterHandler antes de criar).
public sealed class CharacterCreationPolicy(
    ICharacterRepository characters,
    ICharacterProfileStore profiles,
    TimeProvider clock)
{
    public async Task<(CharacterGender Gender, string Outfit)> EnsureCanCreateAsync(
        Guid accountId,
        CreateCharacterCommand command,
        CancellationToken cancellationToken = default)
    {
        if (CharacterNameRules.Validate(command.Name) is { } nameError)
        {
            throw new InvalidCharacterRequestException(nameError);
        }

        if (!Enum.IsDefined(command.CharacterClass))
        {
            throw new InvalidCharacterRequestException("Unknown character class.");
        }

        var gender = command.Gender ?? CharacterGender.Male;

        if (!CharacterAppearanceOptions.Genders.Contains(gender))
        {
            throw new InvalidCharacterRequestException("Unknown gender.");
        }

        var outfit = command.Outfit is null
            ? CharacterAppearanceOptions.DefaultOutfit
            : CharacterAppearanceOptions.FindOutfit(command.Outfit)
              ?? throw new InvalidCharacterRequestException("Unknown outfit.");

        await profiles.FinalizeDueDeletionsAsync(accountId, clock.GetUtcNow(), cancellationToken);

        var existing = await characters.GetAllByAccountIdAsync(accountId, cancellationToken);

        if (existing.Count >= CharacterSlotRules.UnlockedSlots)
        {
            throw new CharacterCreationConflictException(
                existing.Count >= CharacterSlotRules.MaximumSlots
                    ? "The account already has the maximum number of characters."
                    : "No free character slot: the remaining slots are locked.");
        }

        if (existing.Any(c => c.CharacterClass == command.CharacterClass))
        {
            throw new CharacterCreationConflictException("The account already has a character of this class.");
        }

        if (await profiles.NameInUseAsync(command.Name, cancellationToken))
        {
            throw new CharacterCreationConflictException("Character name is already in use.");
        }

        return (gender, outfit);
    }
}

public sealed record CharacterSlotView(int Index, CharacterSlotState State, Guid? CharacterId, string? CharacterName, CharacterClass? CharacterClass);

public sealed record CharacterSlotsResult(
    int MaximumSlots,
    int UnlockedSlots,
    IReadOnlyList<CharacterSlotView> Slots,
    IReadOnlyList<string> Genders,
    IReadOnlyList<string> Outfits);

// GET api/character/slots: 5 slots (ocupados, livres e bloqueados) e as opções de aparência.
public sealed class GetCharacterSlotsHandler(
    ICharacterRepository characters,
    ICharacterProfileStore profiles,
    ICurrentPlayerContext player,
    TimeProvider clock)
{
    public async Task<CharacterSlotsResult> Handle(CancellationToken cancellationToken = default)
    {
        var accountId = player.AccountId;
        await profiles.FinalizeDueDeletionsAsync(accountId, clock.GetUtcNow(), cancellationToken);
        var existing = await characters.GetAllByAccountIdAsync(accountId, cancellationToken);

        var slots = Enumerable.Range(0, CharacterSlotRules.MaximumSlots)
            .Select(index => index < existing.Count
                ? new CharacterSlotView(index, CharacterSlotState.Occupied, existing[index].Id, existing[index].Name, existing[index].CharacterClass)
                : new CharacterSlotView(
                    index,
                    index < CharacterSlotRules.UnlockedSlots ? CharacterSlotState.Free : CharacterSlotState.Locked,
                    null,
                    null,
                    null))
            .ToList();

        return new CharacterSlotsResult(
            CharacterSlotRules.MaximumSlots,
            CharacterSlotRules.UnlockedSlots,
            slots,
            CharacterAppearanceOptions.Genders.Select(g => g.ToString()).ToList(),
            CharacterAppearanceOptions.Outfits);
    }
}

public sealed record CharacterDeletionResult(Guid CharacterId, DateTimeOffset? DeletionScheduledFor);

// POST api/character/{id}/delete e /cancel-deletion — posse pelo token (PlayerOwnershipGuard, 403).
public sealed class CharacterDeletionHandler(
    PlayerOwnershipGuard ownership,
    ICharacterRepository characters,
    ICharacterProfileStore profiles,
    TimeProvider clock)
{
    public async Task<CharacterDeletionResult> RequestAsync(Guid characterId, CancellationToken cancellationToken = default)
    {
        var profile = await OwnedProfileAsync(characterId, cancellationToken);

        if (profile?.DeletionScheduledFor is not null)
        {
            throw new CharacterDeletionConflictException("Character deletion is already pending.");
        }

        var now = clock.GetUtcNow();
        var scheduledFor = CharacterDeletionRules.ScheduledFor(now);
        await profiles.SetDeletionAsync(characterId, now, scheduledFor, cancellationToken);
        return new CharacterDeletionResult(characterId, scheduledFor);
    }

    public async Task<CharacterDeletionResult> CancelAsync(Guid characterId, CancellationToken cancellationToken = default)
    {
        var profile = await OwnedProfileAsync(characterId, cancellationToken);

        if (profile?.DeletionScheduledFor is null)
        {
            throw new CharacterDeletionConflictException("Character deletion is not pending.");
        }

        await profiles.SetDeletionAsync(characterId, null, null, cancellationToken);
        return new CharacterDeletionResult(characterId, null);
    }

    private async Task<CharacterProfile?> OwnedProfileAsync(Guid characterId, CancellationToken cancellationToken)
    {
        await ownership.EnsureCharacterAsync(characterId, cancellationToken);
        var character = await characters.GetByIdAsync(characterId, cancellationToken);
        var accountId = character!.AccountId.Value;

        // Exclusão vencida já finalizada: o personagem não pertence mais à conta (403, como inexistente).
        await profiles.FinalizeDueDeletionsAsync(accountId, clock.GetUtcNow(), cancellationToken);
        var account = await profiles.GetByAccountAsync(accountId, cancellationToken);

        if (!account.TryGetValue(characterId, out var profile))
        {
            throw new OwnershipViolationException("character");
        }

        return profile;
    }
}
