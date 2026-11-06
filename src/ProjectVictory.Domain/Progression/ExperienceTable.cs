using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectVictory.Domain.Progression;

// PV-PROG-011: Experience Table (ADR-025; developer-guide/domain/experience-table.md).
// Interpreta a Experience absoluta e resolve o Level. Nunca armazena nem altera Experience.
// Entradas explícitas para todos os Levels, validadas na construção:
// começa no Level 1 com Experience 0, níveis contínuos, Experience crescente.
public sealed class ExperienceTable
{
    // ADR-026: cada Ascension possui limite de Level 500.
    public const int MaximumLevelPerAscension = 500;

    private static readonly Lazy<ExperienceTable> StandardTable =
        new(() => FromCurve(MaximumLevelPerAscension));

    // Tabela padrão: gerada pela curva do DEC-20261106-10, Levels 1 a 500.
    public static ExperienceTable Standard => StandardTable.Value;

    private readonly long[] _minimumExperienceByLevel;

    public IReadOnlyList<ExperienceTableEntry> Entries { get; }

    public int MaximumLevel => _minimumExperienceByLevel.Length;

    public ExperienceTable(
        IReadOnlyList<ExperienceTableEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        _minimumExperienceByLevel = Validate(entries);

        Entries = entries.ToArray();
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

    public static ExperienceTable FromCurve(int maximumLevel)
    {
        if (maximumLevel < 1)
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

    // Maior Level cuja Experience mínima já foi alcançada; nunca passa do último Level da tabela.
    public int GetLevel(long experience)
    {
        if (experience < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(experience),
                "Experience cannot be negative.");
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

        return _minimumExperienceByLevel[level - 1];
    }

    // Experience que falta para o próximo Level; null quando já está no último Level da tabela.
    public long? GetExperienceToNextLevel(long experience)
    {
        var level = GetLevel(experience);

        if (level >= MaximumLevel)
        {
            return null;
        }

        return _minimumExperienceByLevel[level] - experience;
    }

    // API anterior ao PV-PROG-011, mantida pela Regra Aditiva: Level fora da tabela → 0.
    public int GetRequiredExperience(int level)
    {
        if (level < 1 || level > MaximumLevel)
        {
            return 0;
        }

        return checked((int)_minimumExperienceByLevel[level - 1]);
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
