export interface GameState
{
    accountId: string;

    characterId: string;

    characterName: string;

    level: number;

    experience: number;

    // PV-CLIENT-020 (DEC-20261106-30): título e tier vêm do servidor, derivados do Level; o cliente só exibe.
    title?: string;

    titleTier?: number;
}