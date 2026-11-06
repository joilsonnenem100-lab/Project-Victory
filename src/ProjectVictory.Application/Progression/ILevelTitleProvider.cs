using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

// PV-PROG-013 (DEC-20261106-30): fonte da tabela de títulos por Level. A API lê da configuração a
// cada consulta (troca sem recompilar nem reiniciar); fora da API vale a tabela padrão.
public interface ILevelTitleProvider
{
    LevelTitleTable Current { get; }
}

public sealed class FixedLevelTitleProvider(LevelTitleTable table) : ILevelTitleProvider
{
    public static FixedLevelTitleProvider Default { get; } = new(LevelTitleTable.Default);

    public LevelTitleTable Current { get; } = table ?? throw new ArgumentNullException(nameof(table));
}
