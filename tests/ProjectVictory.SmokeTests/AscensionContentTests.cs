using ProjectVictory.Application.Hunt;
using ProjectVictory.Infrastructure.Content;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-PROG-012: conteúdo preparado para expansão — cada área declara a Ascension mínima (dados) e o
// jogador vê a Ascension nova e as antigas.
public sealed class AscensionContentTests
{
    private sealed class ExpandedCatalog : IHuntAreaCatalog
    {
        private readonly HuntAreaDefinition _spiderCave = new SpiderCaveHuntArea().GetDefault();

        public HuntAreaDefinition GetDefault() => _spiderCave;

        public IReadOnlyList<HuntAreaDefinition> GetAll() =>
        [
            _spiderCave,
            new HuntAreaDefinition("Ascension 2 Area", _spiderCave.EncounterTemplates, RequiredAscension: 2)
        ];
    }

    [Fact]
    public void SpiderCave_IsAscensionOneContent()
    {
        var area = new SpiderCaveHuntArea().GetDefault();

        Assert.Equal(1, area.RequiredAscension);
        Assert.Contains(((IHuntAreaCatalog)new SpiderCaveHuntArea()).GetAvailable(1), a => a.Name == "Spider Cave");
    }

    [Fact]
    public void Availability_IncludesNewAndOldAscensions()
    {
        IHuntAreaCatalog catalog = new ExpandedCatalog();

        Assert.Equal(new[] { "Spider Cave" }, catalog.GetAvailable(1).Select(a => a.Name));
        Assert.Equal(new[] { "Spider Cave", "Ascension 2 Area" }, catalog.GetAvailable(2).Select(a => a.Name));
    }
}
