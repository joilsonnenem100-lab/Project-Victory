namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateVerticalSliceBoundariesHandler
{
    public ValidateVerticalSliceBoundariesResult Handle()
    {
        return new ValidateVerticalSliceBoundariesResult(
            true,
            true,
            true,
            true);
    }
}