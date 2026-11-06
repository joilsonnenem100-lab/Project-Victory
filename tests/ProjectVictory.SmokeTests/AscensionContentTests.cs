using ProjectVictory.Application.Hunt;
using ProjectVictory.Infrastructure.Content;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-PROG-012/013: conteúdo preparado para expansão — cada área declara o tier mínimo (dados) e o
// jogador vê as áreas do seu tier e dos anteriores (DEC-20261106-30).
public sealed class AscensionContentTests
{
    private sealed class ExpandedCatalog : IHuntAreaCatalog
    {
        private readonly HuntAreaDefinition _spiderCave = new SpiderCaveHuntArea().GetDefault();

        public HuntAreaDefinition GetDefault() => _spiderCave;

        public IReadOnlyList<HuntAreaDefinition> GetAll() =>
        [
            _spiderCave,
            new HuntAreaDefinition("Ascension 1 Area", _spiderCave.EncounterTemplates, RequiredTitleTier: 1)
        ];
    }

    [Fact]
    public void SpiderCave_RequiresTierZero()
    {
        var area = new SpiderCaveHuntArea().GetDefault();

        Assert.Equal(0, area.RequiredTitleTier);
        Assert.Contains(((IHuntAreaCatalog)new SpiderCaveHuntArea()).GetAvailable(0), a => a.Name == "Spider Cave");
    }

    [Fact]
    public void Availability_IncludesNewAndOldAscensions()
    {
        IHuntAreaCatalog catalog = new ExpandedCatalog();

        Assert.Equal(new[] { "Spider Cave" }, catalog.GetAvailable(0).Select(a => a.Name));
        Assert.Equal(new[] { "Spider Cave", "Ascension 1 Area" }, catalog.GetAvailable(1).Select(a => a.Name));
        Assert.Equal(new[] { "Spider Cave", "Ascension 1 Area" }, catalog.GetAvailable(5).Select(a => a.Name));
    }
}
