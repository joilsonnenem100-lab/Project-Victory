using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectVictory.Domain.Progression;

// PV-PROG-013 (DEC-20261106-30): título e tier DERIVADOS do Level (substitui a Ascension da Fila 4).
// O Level vem da Experience (sem teto, nunca reseta); o título é só uma leitura do Level.
// Tier numérico: 0 = Novice; 1 a 4 = Ascensions; 5 = Legend (permanente).
public sealed record LevelTitle(
    int Tier,
    string Label,
    int MinimumLevel);

// Tabela de títulos. Os rótulos e as faixas vêm de CONFIGURAÇÃO (seção "LevelTitles"), trocáveis
// sem recompilar; esta classe só valida e consulta. `Default` é o valor decidido pelo PO.
public sealed class LevelTitleTable
{
    public const string SectionKey = "LevelTitles";

    public static LevelTitleTable Default { get; } = new(
    [
        new LevelTitle(0, "Novice", 1),
        new LevelTitle(1, "Ascension 1", 500),
        new LevelTitle(2, "Ascension 2", 1000),
        new LevelTitle(3, "Ascension 3", 1500),
        new LevelTitle(4, "Ascension 4", 2000),
        new LevelTitle(5, "Legend", 2500)
    ]);

    public IReadOnlyList<LevelTitle> Titles { get; }

    public LevelTitleTable(IEnumerable<LevelTitle> titles)
    {
        ArgumentNullException.ThrowIfNull(titles);

        var ordered = titles.OrderBy(t => t.MinimumLevel).ToList();

        if (ordered.Count == 0)
        {
            throw new ArgumentException("At least one level title is required.", nameof(titles));
        }

        if (ordered[0].MinimumLevel != 1)
        {
            throw new ArgumentException("The first level title must start at Level 1.", nameof(titles));
        }

        for (var i = 0; i < ordered.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(ordered[i].Label))
            {
                throw new ArgumentException("Level title labels cannot be empty.", nameof(titles));
            }

            // Tiers consecutivos a partir de 0, na ordem das faixas.
            if (ordered[i].Tier != i)
            {
                throw new ArgumentException("Level title tiers must be 0, 1, 2… in level order.", nameof(titles));
            }

            if (i > 0 && ordered[i].MinimumLevel <= ordered[i - 1].MinimumLevel)
            {
                throw new ArgumentException("Level title ranges cannot overlap.", nameof(titles));
            }
        }

        Titles = ordered.AsReadOnly();
    }

    public int MaximumTier => Titles[^1].Tier;

    // Maior faixa cujo Level mínimo ≤ level. O último título vale para sempre (sem teto de Level).
    public LevelTitle For(int level)
    {
        if (level < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be at least 1.");
        }

        var title = Titles[0];

        foreach (var candidate in Titles)
        {
            if (candidate.MinimumLevel > level)
            {
                break;
            }

            title = candidate;
        }

        return title;
    }
}
