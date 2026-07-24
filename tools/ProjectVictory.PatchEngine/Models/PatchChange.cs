namespace ProjectVictory.PatchEngine.Models;

public sealed class PatchChange
{
    public char Operation { get; init; }

    public int? OriginalLine { get; init; }

    public int? NewLine { get; init; }

    public string Text { get; init; } = string.Empty;

    public bool IsAddition => Operation == '+';

    public bool IsRemoval => Operation == '-';

    public bool IsContext => Operation == ' ';
}