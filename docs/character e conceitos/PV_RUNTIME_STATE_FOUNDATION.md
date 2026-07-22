# PROJECT-VICTORY --- Runtime State Foundation

## Status

Documento de consolidação das decisões de Product Owner relacionadas ao
sistema de Runtime State.

Este sistema representa:

"O que o personagem está fazendo agora"

Separação:

Character State

=

Quem o personagem é

Runtime State

=

Atividade atual em execução

------------------------------------------------------------------------

# 1. Objetivo do Runtime State

Runtime State conecta o personagem aos sistemas de gameplay.

Fluxo:

Character

↓

Runtime State

↓

Gameplay System

↓

Hunt / Quest / Dungeon / Events

------------------------------------------------------------------------

# 2. Quantidade de Runtime Ativos

Decisão:

Cada personagem possui apenas um Runtime ativo.

Modelo:

Character

↓

Current Runtime

↓

Uma atividade atual

Exemplos:

Hunt

ou

Quest

ou

Dungeon

Não existem múltiplas atividades simultâneas inicialmente.

------------------------------------------------------------------------

# 3. Persistência após Logout

Decisão:

Logout encerra o Runtime.

O personagem não continua automaticamente uma atividade após sair.

O comportamento futuro pode evoluir para atividades específicas.

------------------------------------------------------------------------

# 4. Recuperação após Reinício do Servidor

Decisão:

O Runtime não será recuperado após reinicialização inicialmente.

A primeira versão não mantém continuidade automática de execução após
restart.

------------------------------------------------------------------------

# 5. Visualização do Runtime

Decisão:

O Runtime é visível ao jogador.

O jogador pode acompanhar:

-   atividade atual;
-   estado atual;
-   progresso.

Exemplo:

Hunt:

Ascendancy Path

Estado:

Combat

------------------------------------------------------------------------

# 6. Estados do Runtime

Decisão:

Modelo completo de estados.

Estados:

Idle

↓

Starting

↓

Running

↓

Paused

↓

Completed

↓

Failed

------------------------------------------------------------------------

# 7. Tipo do Runtime

Decisão:

O Runtime identifica qual sistema está executando.

Exemplo:

RuntimeType:

HUNT

COMBAT

QUEST

DUNGEON

EVENT

Isso permite expansão futura.

------------------------------------------------------------------------

# 8. Histórico

Decisão:

Não existe histórico persistente inicialmente.

Logs detalhados ficam para sistemas futuros.

------------------------------------------------------------------------

# 9. Checkpoints

Decisão:

Runtime possui checkpoints.

Objetivo:

Registrar pontos importantes da execução.

Exemplo:

Checkpoint:

Hunt iniciada

Checkpoint:

Combat finalizado

Checkpoint:

Reward aplicada

------------------------------------------------------------------------

# 10. Pausa Automática

Decisão:

O Runtime não pausa automaticamente.

Se a atividade for interrompida, o comportamento será tratado pelo
sistema específico.

------------------------------------------------------------------------

# 11. Expansão Futura

Decisão:

Runtime deve suportar múltiplos sistemas.

Preparado para:

-   Hunt;
-   Quest;
-   Dungeon;
-   Eventos;
-   PvP.

------------------------------------------------------------------------

# 12. Relação com Arquitetura Geral

Mapa:

Account

↓

Wallet

↓

Character

↓

Runtime State

↓

Gameplay Systems

------------------------------------------------------------------------

# 13. Primeiro Uso no Project-Victory

Primeiro sistema utilizando Runtime:

Hunt Runtime

Fluxo:

Character

↓

Runtime State

↓

Hunt

↓

Encounter

↓

Combat

↓

Reward

------------------------------------------------------------------------

# 14. Estado do Bloco

Definido:

-   um Runtime ativo por personagem;
-   logout encerra Runtime;
-   restart não recupera automaticamente;
-   Runtime visível;
-   máquina de estados;
-   identificação do sistema;
-   checkpoints;
-   ausência de histórico inicial;
-   expansão para múltiplos sistemas.

Próximos blocos:

-   Game Session Foundation;
-   Final Gameplay Architecture Review;
-   Contratos de implementação.
