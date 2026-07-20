using System;

namespace ProjectVictory.Application.Item;

public sealed class GetItemDefinitionQuery
{
    public Guid ItemId { get; }


    public GetItemDefinitionQuery(
        Guid itemId)
    {
        ItemId = itemId;
    }
}