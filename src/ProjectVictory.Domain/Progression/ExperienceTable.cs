using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectVictory.Domain.Progression;

// PV-PROG-011/012: Experience Table (ADR-025; developer-guide/domain/experience-table.md).
// Interpreta a Experience absoluta e resolve o Level. Nunca armazena nem altera Experience.
//
// Dois tipos:
// - tabela de ENTRADAS explícitas (validada: começa no Level 1 com 0, contínua, crescente);
// - tabela da CURVA (Standard), SEM TETO de Level (DEC-20261106-19): o Level vem da fórmula do Tibia
//   (DEC-20261106-10) por busca na curva, até o maior Level cuja Experience cabe em long.
public sealed class ExperienceTable
{
    // *** Histórico *** (PV-PROG-011): teto de 500 Levels por Ascension — SUPERADO pelo
    // DEC-20261106-19 (Level sem teto; Ascension derivada do Level, passo em AscensionRules).
    // Mantido como constante pública pela Regra Aditiva; não limita mais nenhuma tabela.
    public const int MaximumLevelPerAscension = 500;

    // A primeira Ascension (a Ascension agora é derivada do Level: AscensionRules).
    public const int FirstAscension = 1;

    private static readonly ExperienceTable StandardTable = new();

    // Tabela padrão: a curva do DEC-20261106-10, sem teto de Level.
    public static ExperienceTable Standard => StandardTable;

    private readonly long[]? _minimumExperienceByLevel;

    // Entradas explícitas; vazio na tabela da curva (que não tem lista finita).
    public IReadOnlyList<ExperienceTableEntry> Entries { get; }

    public bool IsUnbounded => _minimumExperienceByLevel is null;

    public int MaximumLevel =>
        _minimumExperienceByLevel?.Length ?? ExperienceCurve.MaximumSupportedLevel;

    // Tabela da curva (sem teto).
    private ExperienceTable()
    {
        _minimumExperienceByLevel = null;
        Entries = Array.Empty<ExperienceTableEntry>();
    }

    public ExperienceTable(
        IReadOnlyList<ExperienceTableEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        _minimumExperienceByLevel = Validate(entries);

        Entries = Array.AsReadOnly(entries.ToArray());
    }

    // Construtor anterior ao PV-PROG-011 (Level → Experience mínima), mantido pela Regra Aditiva.
    public ExperienceTable(
        IReadOnlyDictionary<int, int> levels)
        : this(ToEntries(levels))
    {
    }

    // DEC-20261106-11: a mesma curva vale para todas as Ascensions.
    public static ExperienceTable ForAscension(int ascension)
    {
        if (ascension < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(ascension),
                "Ascension must be at least 1.");
        }

        return Standard;
    }

    // Tabela de entradas explícitas gerada pela curva até `maximumLevel` (uso em testes/ferramentas).
    public static ExperienceTable FromCurve(int maximumLevel)
    {
        if (maximumLevel < 1 || maximumLevel > ExperienceCurve.MaximumSupportedLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumLevel));
        }

        var entries =
            Enumerable.Range(1, maximumLevel)
                .Select(level => new ExperienceTableEntry(
                    level,
                    ExperienceCurve.TotalExperienceForLevel(level)))
                .ToArray();

        return new ExperienceTable(entries);
    }

    // Maior Level cuja Experience mínima já foi alcançada.
    public int GetLevel(long experience)
    {
        if (experience < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(experience),
                "Experience cannot be negative.");
        }

        if (_minimumExperienceByLevel is null)
        {
            return ExperienceCurve.LevelFor(experience);
        }

        var index =
            Array.BinarySearch(
                _minimumExperienceByLevel,
                experience);

        if (index >= 0)
        {
            // Experience exatamente igual ao mínimo de um Level.
            return index + 1;
        }

        // ~index = primeiro Level cujo mínimo é maior que a Experience.
        return ~index;
    }

    public long GetMinimumExperience(int level)
    {
        if (level < 1 || level > MaximumLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        return _minimumExperienceByLevel is null
            ? ExperienceCurve.TotalExperienceForLevel(level)
            : _minimumExperienceByLevel[level - 1];
    }

    // Experience máxima do Level (experience-table.md, "Estrutura"): mínima do Level seguinte − 1;
    // null no último Level, cujo intervalo é aberto (ADR-023).
    public long? GetMaximumExperience(int level)
    {
        if (level < 1 || level > MaximumLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        return level == MaximumLevel
            ? null
            : GetMinimumExperience(level + 1) - 1;
    }

    // Experience que falta para o próximo Level; null só no último Level representável.
    public long? GetExperienceToNextLevel(long experience)
    {
        var level = GetLevel(experience);

        if (level >= MaximumLevel)
        {
            return null;
        }

        return GetMinimumExperience(level + 1) - experience;
    }

    // API anterior ao PV-PROG-011, mantida pela Regra Aditiva: Level fora da tabela → 0.
    // Retorna int: lança OverflowException se o mínimo do Level passar de int.MaxValue
    // (a partir do Level ~ 505). Use GetMinimumExperience.
    public int GetRequiredExperience(int level)
    {
        if (level < 1 || level > MaximumLevel)
        {
            return 0;
        }

        return checked((int)GetMinimumExperience(level));
    }

    private static long[] Validate(
        IReadOnlyList<ExperienceTableEntry> entries)
    {
        if (entries.Count == 0)
        {
            throw new ArgumentException(
                "Experience Table must have at least one level.",
                nameof(entries));
        }

        // PV-PROG-012: o limite de 500 entradas por tabela (ADR-026, texto anterior) foi removido
        // pelo DEC-20261106-19 (Level sem teto). "Último nível válido" = último da lista.

        var minimums = new long[entries.Count];

        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i] ??
                throw new ArgumentException("Experience Table entry cannot be null.", nameof(entries));

            if (entry.Level != i + 1)
            {
                throw new ArgumentException(
                    "Experience Table levels must start at 1 and be continuous.",
                    nameof(entries));
            }

            if (i == 0 && entry.MinimumExperience != 0)
            {
                throw new ArgumentException(
                    "Level 1 must start at 0 Experience.",
                    nameof(entries));
            }

            if (i > 0 && entry.MinimumExperience <= minimums[i - 1])
            {
                throw new ArgumentException(
                    "Experience Table must be strictly increasing.",
                    nameof(entries));
            }

            minimums[i] = entry.MinimumExperience;
        }

        return minimums;
    }

    private static IReadOnlyList<ExperienceTableEntry> ToEntries(
        IReadOnlyDictionary<int, int> levels)
    {
        ArgumentNullException.ThrowIfNull(levels);

        return levels
            .OrderBy(x => x.Key)
            .Select(x => new ExperienceTableEntry(x.Key, x.Value))
            .ToArray();
    }
}

public sealed record ExperienceTableEntry(
    int Level,
    long MinimumExperience);
