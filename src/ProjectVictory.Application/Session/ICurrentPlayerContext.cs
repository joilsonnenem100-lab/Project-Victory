namespace ProjectVictory.Application.Session;

public interface ICurrentPlayerContext
{
    Guid AccountId { get; }
}