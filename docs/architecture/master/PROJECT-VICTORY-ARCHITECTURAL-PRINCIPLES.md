# PROJECT-VICTORY --- ARCHITECTURAL PRINCIPLES

## Separação de responsabilidades

Cada sistema possui seu próprio domínio.

------------------------------------------------------------------------

# Aggregate Ownership

Um sistema não altera diretamente o estado interno de outro.

Exemplo incorreto:

Combat altera Level.

Correto:

Combat Result

↓

Reward

↓

Progression

↓

Character

------------------------------------------------------------------------

# Content versus Runtime

Conteúdo:

Definição criada pelo design.

Runtime:

Instância criada durante execução.

Fluxo:

CONTENT

↓

RUNTIME INSTANCE

↓

PERSISTENCE

------------------------------------------------------------------------

# Server Authority

O cliente solicita.

O servidor decide.

------------------------------------------------------------------------

# Estado permanente versus temporário

Permanente:

-   Account
-   Character
-   Inventory
-   Skills

Temporário:

-   Combat atual
-   Encounter atual
-   Runtime atual

------------------------------------------------------------------------

# Eventos futuros

Possível evolução:

CombatCompletedEvent

↓

Reward

↓

Achievement

↓

Quest Progress
