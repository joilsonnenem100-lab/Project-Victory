namespace ProjectVictory.Application.Reward;

// PV-XP-010 (ADR-007): parte da Experience que cabe a um participante.
public sealed record ExperienceShare(
    Guid CharacterId,
    long Amount);

// PV-XP-010 (ADR-007): Experience Distribution — responsabilidade ÚNICA de repartir a Experience
// FINAL da recompensa (já calculada pelo Reward Calculation e já com o XP rate/bônus aplicados) entre
// os participantes. Não calcula recompensa nem aplica bônus, e NÃO altera Experience: quem aplica as
// partes é o Reward Pipeline (ADR-024), a única via que muda Experience.
public interface IExperienceDistributor
{
    IReadOnlyList<ExperienceShare> Distribute(
        long finalExperience,
        IReadOnlyList<Guid> participants);
}

// *** PROVISÓRIO *** (regra de grupo é decisão do PO): divisão igual; o resto da divisão vai, 1 a 1,
// aos primeiros participantes na ordem recebida (determinístico; a soma das partes = o total).
// Com 1 participante (o caso de hoje: 1 jogador por Hunt), ele recebe tudo — mesmo XP de antes.
public sealed class EvenSplitExperienceDistributor : IExperienceDistributor
{
    public static EvenSplitExperienceDistributor Instance { get; } = new();

    public IReadOnlyList<ExperienceShare> Distribute(
        long finalExperience,
        IReadOnlyList<Guid> participants)
    {
        ArgumentNullException.ThrowIfNull(participants);

        if (finalExperience < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(finalExperience), "Experience cannot be negative.");
        }

        if (participants.Count == 0)
        {
            throw new ArgumentException("At least one participant is required.", nameof(participants));
        }

        if (participants.Distinct().Count() != participants.Count)
        {
            throw new ArgumentException("Participants must be distinct.", nameof(participants));
        }

        var each = finalExperience / participants.Count;
        var remainder = finalExperience % participants.Count;

        return participants
            .Select((characterId, index) => new ExperienceShare(characterId, each + (index < remainder ? 1 : 0)))
            .ToList();
    }
}
