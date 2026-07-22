# PROJECT-VICTORY --- RUNTIME ARCHITECTURE

## Conceito

Runtime representa aquilo que o jogador está executando atualmente.

Separação fundamental:

Character State:

"Quem o jogador é"

Runtime State:

"O que o jogador está fazendo agora"

World State:

"Como o mundo está"

------------------------------------------------------------------------

# Runtime State

Responsabilidade:

-   manter contexto de execução;
-   permitir continuidade;
-   permitir recuperação;
-   separar estado temporário do personagem.

Modelo conceitual:

RuntimeState

-   Id
-   CharacterId
-   RuntimeType
-   Status
-   ReferenceId
-   CreatedAt
-   UpdatedAt

------------------------------------------------------------------------

# Relação

Character

↓

Runtime

↓

Hunt

↓

Encounter

↓

Combat

------------------------------------------------------------------------

# Runtime Coordinator

Conceito:

Orquestrar execução.

Não possui regras profundas.

Ele coordena:

-   Hunt;
-   Combat;
-   Reward;
-   Progression;
-   Persistência.

------------------------------------------------------------------------

# Estados possíveis

Idle

↓

Loading

↓

Running

↓

Paused

↓

Completed

------------------------------------------------------------------------

# Objetivo futuro

Criar uma experiência contínua:

-   servidor pode recuperar estado;
-   jogador pode continuar atividades;
-   sistemas podem evoluir sem acoplamento.
