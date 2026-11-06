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
        // PV-CONTENT-011: a tabela agora já vem com itens da migration (HasData); o seed procura o
        // Training Sword pelo nome, em vez de "qualquer item", para continuar semeando-o.
        var exists =
            await context.Items
                .AnyAsync(x => x.Name == "Training Sword", cancellationToken);


        if (exists)
        {
            return;
        }


        var item = new PersistentItemState
        {
            Id = Guid.NewGuid(),
            Name = "Training Sword",
            Description = "Basic sword used for validation.",
            // PV-REWARD-010: preço de venda PROVISÓRIO (DEC-20261106-08).
            NpcSellPrice = 10
        };


        context.Items.Add(item);


        await context.SaveChangesAsync(
            cancellationToken);
    }
}