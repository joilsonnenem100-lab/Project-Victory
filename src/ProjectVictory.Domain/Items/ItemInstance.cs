public sealed class ItemInstance
{
    public Guid Id { get; private set; }

    public Guid ItemDefinitionId { get; private set; }


    public ItemInstance()
    {
    }


    public ItemInstance(
        Guid id,
        Guid itemDefinitionId)
    {
        Id = id;

        ItemDefinitionId = itemDefinitionId;
    }
}