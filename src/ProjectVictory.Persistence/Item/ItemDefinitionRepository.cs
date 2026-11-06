using Microsoft.EntityFrameworkCore;
using ProjectVictory.Domain.Items;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.Item;

public sealed class ItemDefinitionRepository : IItemDefinitionRepository
{
    private readonly ProjectVictoryDbContext _context;


    public ItemDefinitionRepository(
        ProjectVictoryDbContext context)
    {
        _context = context;
    }


    public async Task<ItemDefinition?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var state =
            await _context.Items
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);


        if (state is null)
        {
            return null;
        }


        return new ItemDefinition(
            state.Id,
            state.Name,
            new ItemCharacteristics(
                state.Description),
            state.NpcSellPrice);
    }
}