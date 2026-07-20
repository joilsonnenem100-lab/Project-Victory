using ProjectVictory.Domain.Items;

namespace ProjectVictory.Application.Item;

public sealed class GetItemDefinitionHandler
{
    private readonly IItemDefinitionRepository _repository;


    public GetItemDefinitionHandler(
        IItemDefinitionRepository repository)
    {
        _repository = repository;
    }


    public async Task<GetItemDefinitionResult?> Handle(
        GetItemDefinitionQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);


        var item =
            await _repository.GetByIdAsync(
                query.ItemId,
                cancellationToken);


        if (item is null)
        {
            return null;
        }


        return new GetItemDefinitionResult
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Characteristics.Description
        };
    }
}