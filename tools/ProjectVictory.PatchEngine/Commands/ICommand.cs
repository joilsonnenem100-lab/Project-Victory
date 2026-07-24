namespace ProjectVictory.PatchEngine.Commands;

public interface ICommand
{
    string Name { get; }

    int Execute(string[] args);
}