using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Infrastructure.Services;

public class RewardProcessor : IRewardProcessor
{
    private static readonly Guid RatId =
        Guid.Parse("10000000-0000-0000-0000-000000000001");


    private static readonly Guid CaveRatId =
        Guid.Parse("10000000-0000-0000-0000-000000000002");


    private static readonly Guid RatTailId =
        Guid.Parse("20000000-0000-0000-0000-000000000001");


    private static readonly Guid CaveRatClawId =
        Guid.Parse("20000000-0000-0000-0000-000000000002");



    private readonly Random _random =
        new();



    public RewardResult Process(
        RewardRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);


        if (request.SourceResult is not EncounterMonsterState monster)
        {
            return new RewardResult(
                0,
                0,
                []);
        }



        return monster.MonsterId switch
        {
            var id when id == RatId =>
                CreateRatReward(),


            var id when id == CaveRatId =>
                CreateCaveRatReward(),


            _ =>
                new RewardResult(
                    0,
                    0,
                    [])
        };
    }



    private RewardResult CreateRatReward()
    {
        var drops =
            new List<RewardDrop>();


        if (_random.NextDouble() <= 0.50)
        {
            drops.Add(
                new RewardDrop(
                    RatTailId,
                    "Rat Tail"));
        }


        return new RewardResult(
            50,

            _random.Next(
                1,
                6),

            drops);
    }



    private RewardResult CreateCaveRatReward()
    {
        var drops =
            new List<RewardDrop>();


        if (_random.NextDouble() <= 0.50)
        {
            drops.Add(
                new RewardDrop(
                    CaveRatClawId,
                    "Cave Rat Claw"));
        }


        return new RewardResult(
            100,

            _random.Next(
                3,
                9),

            drops);
    }
}