namespace ProjectVictory.Domain.Entities;

public sealed class Skills
{
    public int Fist { get; private set; } = 10;
    public int Club { get; private set; } = 10;
    public int Sword { get; private set; } = 10;
    public int Axe { get; private set; } = 10;
    public int Distance { get; private set; } = 10;
    public int Shielding { get; private set; } = 10;
    public int MagicLevel { get; private set; } = 10;
    public int Fishing { get; private set; } = 10;


    public void IncreaseFist(int amount)
    {
        ValidateIncrease(amount);
        Fist += amount;
    }

    public void IncreaseClub(int amount)
    {
        ValidateIncrease(amount);
        Club += amount;
    }

    public void IncreaseSword(int amount)
    {
        ValidateIncrease(amount);
        Sword += amount;
    }

    public void IncreaseAxe(int amount)
    {
        ValidateIncrease(amount);
        Axe += amount;
    }

    public void IncreaseDistance(int amount)
    {
        ValidateIncrease(amount);
        Distance += amount;
    }

    public void IncreaseShielding(int amount)
    {
        ValidateIncrease(amount);
        Shielding += amount;
    }

    public void IncreaseMagicLevel(int amount)
    {
        ValidateIncrease(amount);
        MagicLevel += amount;
    }

    public void IncreaseFishing(int amount)
    {
        ValidateIncrease(amount);
        Fishing += amount;
    }


    private static void ValidateIncrease(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Skill increase must be greater than zero.");
        }
    }
}
