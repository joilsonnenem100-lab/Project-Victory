export interface RewardDrop
{
    itemId: string;

    itemName: string;
}


export interface RewardResult
{
    experience: number;

    gold: number;

    drops: RewardDrop[];
}