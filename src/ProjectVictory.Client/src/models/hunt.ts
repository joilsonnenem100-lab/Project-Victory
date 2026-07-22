export interface EncounterResult
{
    id: string;

    huntId: string;

    currentTargetId: string;

    monsterName: string;

    currentHealth: number;

    maximumHealth: number;
}


export interface HuntResult
{
    huntId: string;

    completed: boolean;

    experience: number;

    gold: number;

    level: number;

    encounter: EncounterResult;
}