using Microsoft.EntityFrameworkCore;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Item;

namespace ProjectVictory.Persistence.Database.Seed;

public static class ItemDefinitionSeed
{
    public static async Task ExecuteAsync(
        ProjectVictoryDbContext context,
        CancellationToken cancellationToken = default)
    {
        var exists =
            await context.Items
                .AnyAsync(cancellationToken);


        if (exists)
        {
            return;
        }


        var item = new PersistentItemState
        {
            Id = Guid.NewGuid(),
            Name = "Training Sword",
            Description = "Basic sword used for validation."
        };


        context.Items.Add(item);


        await context.SaveChangesAsync(
            cancellationToken);
    }
}