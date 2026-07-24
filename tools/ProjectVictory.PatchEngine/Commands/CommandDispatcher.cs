namespace ProjectVictory.PatchEngine.Commands;

public sealed class CommandDispatcher
{
    private readonly Dictionary<string, ICommand> _commands =
        new(StringComparer.OrdinalIgnoreCase);

    public void Register(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        _commands[command.Name] = command;
    }

    public int Dispatch(string[] args)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return 1;
        }

        var commandName = args[0];

        if (!_commands.TryGetValue(commandName, out var command))
        {
            Console.WriteLine($"Unknown command: {commandName}");
            Console.WriteLine();

            ShowHelp();

            return 1;
        }

        return command.Execute(args.Skip(1).ToArray());
    }

    private static void ShowHelp()
    {
        Console.WriteLine("Project Victory Patch Engine");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  pvpatch preview <patch-file>");
    }
}