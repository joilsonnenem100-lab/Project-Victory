# PROJECT-VICTORY --- CONTENT PIPELINE ARCHITECTURE

## Conceito

Separar conteúdo criado por designers de instâncias durante execução.

Fluxo:

CONTENT DEFINITION

↓

FACTORY

↓

RUNTIME INSTANCE

↓

PERSISTENCE

------------------------------------------------------------------------

Exemplo:

MonsterDefinition

↓

MonsterFactory

↓

MonsterInstance

------------------------------------------------------------------------

Aplica-se a:

-   Monster
-   Item
-   NPC
-   Quest
-   Hunt
-   Map
