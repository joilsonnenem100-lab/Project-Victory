namespace ProjectVictory.Domain.Runtime;

public static class RuntimeActivityTransitionRules
{
    public static bool CanTransition(
        RuntimeActivity current,
        RuntimeActivity next)
    {
        if (current == next)
        {
            return true;
        }

        return current switch
        {
            RuntimeActivity.Idle =>
                next == RuntimeActivity.Exploring,

            RuntimeActivity.Exploring =>
                next is RuntimeActivity.Hunting
                    or RuntimeActivity.Traveling
                    or RuntimeActivity.Dialogue,

            RuntimeActivity.Traveling =>
                next == RuntimeActivity.Exploring,

            RuntimeActivity.Hunting =>
                next == RuntimeActivity.Combat
                || next == RuntimeActivity.Exploring
                || next == RuntimeActivity.Reward,

            RuntimeActivity.Combat =>
                next == RuntimeActivity.Reward,

            RuntimeActivity.Reward =>
                next == RuntimeActivity.Exploring,

            RuntimeActivity.Dialogue =>
                next == RuntimeActivity.Exploring,

            _ => false
        };
    }
}
