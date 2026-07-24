namespace ProjectVictory.SmokeTests;

public static class SmokeTestReporter
{
    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("PROJECT VICTORY SMOKE TEST");
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    public static void StepPassed(
        string step,
        string? details = null)
    {
        Console.WriteLine($"[PASS] {step}");

        if (!string.IsNullOrWhiteSpace(details))
        {
            Console.WriteLine($"       {details}");
        }

        Console.WriteLine();
    }

    public static void StepFailed(
        string step,
        Exception exception)
    {
        Console.WriteLine($"[FAIL] {step}");
        Console.WriteLine($"       {exception.Message}");
        Console.WriteLine();
    }

    public static void Finish()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("RESULT: VERTICAL SLICE HEALTHY");
        Console.WriteLine("========================================");
        Console.WriteLine();
    }
}