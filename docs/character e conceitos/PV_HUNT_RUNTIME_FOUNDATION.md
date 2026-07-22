# PROJECT-VICTORY --- Hunt Runtime Foundation

## Status

Documento de consolidação das decisões de Product Owner relacionadas ao
ciclo de execução da Hunt.

Este documento define a evolução da Hunt de uma ação isolada para uma
atividade contínua do personagem.

Fluxo principal:

Character

↓

Hunt Session

↓

Encounter

↓

Monster Instance

↓

Combat

↓

Reward

↓

Progression

↓

Novo Encounter

------------------------------------------------------------------------

# 1. Início da Hunt

Decisão:

A Hunt é iniciada manualmente pelo jogador.

Fluxo:

Cidade

↓

Selecionar Hunt

↓

Entrar

↓

Executar Hunt

------------------------------------------------------------------------

# 2. Primeiro Encontro

Decisão:

Ao entrar na Hunt, o primeiro encontro inicia automaticamente.

Fluxo:

Entrar Hunt

↓

Gerar Encounter

↓

Criar Monster Instance

↓

Iniciar Combat

------------------------------------------------------------------------

# 3. Duração da Hunt

Decisão:

A Hunt não possui duração fixa.

Ela permanece ativa até ocorrer uma condição de parada.

A Hunt continua enquanto:

-   jogador não enviar comando de parada;
-   personagem não morrer;
-   personagem não ficar offline.

------------------------------------------------------------------------

# 4. Loop da Hunt

Decisão:

A Hunt possui loop contínuo.

Modelo:

Enquanto Hunt ativa:

Gerar Encounter

↓

Executar Combat

↓

Aplicar Reward

↓

Atualizar Progression

↓

Gerar próximo Encounter

------------------------------------------------------------------------

# 5. Quantidade de Encontros

Decisão:

A Hunt possui loop infinito.

Não existe quantidade fixa inicial de encontros.

O encerramento ocorre por evento:

-   parada manual;
-   morte;
-   offline.

------------------------------------------------------------------------

# 6. Finalização da Hunt

Decisão:

A Hunt pode finalizar por múltiplos motivos.

Estados:

Running

↓

Completed

ou

Cancelled

ou

Failed

------------------------------------------------------------------------

# 7. Estados da Hunt

Decisão:

A Hunt possui máquina de estados.

Estados definidos:

Created

↓

Running

↓

Paused

↓

Completed

↓

Cancelled

------------------------------------------------------------------------

# 8. Pausa da Hunt

Decisão:

Não existe pausa manual inicialmente.

A saída da Hunt encerra a atividade.

------------------------------------------------------------------------

# 9. Execução Automática

Decisão:

O loop da Hunt é automático.

Fluxo:

Encounter

↓

Combat

↓

Reward

↓

Next Encounter

O jogador não inicia cada combate individualmente.

------------------------------------------------------------------------

# 10. Hunts Simultâneas

Decisão:

Um personagem possui apenas uma atividade ativa.

Modelo:

Character

↓

Current Hunt

------------------------------------------------------------------------

# 11. Visualização da Hunt

Decisão:

O jogador acompanha o progresso da Hunt.

Informações:

-   Hunt atual;
-   encontro atual;
-   monstros derrotados;
-   recompensas recebidas;
-   evolução obtida.

------------------------------------------------------------------------

# 12. Relação com Runtime

A Hunt é uma atividade executada pelo personagem.

Arquitetura:

Character State

↓

Runtime State

↓

Hunt Runtime

↓

Combat Runtime

------------------------------------------------------------------------

# 13. Primeiro Loop de Gameplay

O ciclo completo:

Cidade

↓

Selecionar Ascendancy Path

↓

Entrar Hunt

↓

Encounter automático

↓

Combat automático

↓

Reward

↓

Progression

↓

Novo Encounter

------------------------------------------------------------------------

# 14. Estado do Bloco

Definido:

-   início manual;
-   primeiro combate automático;
-   loop contínuo;
-   sem duração fixa;
-   parada por comando, morte ou offline;
-   encontros infinitos;
-   estados da Hunt;
-   ausência de pausa manual;
-   uma Hunt ativa por personagem;
-   acompanhamento visual.

Próximos blocos:

-   Reward System Foundation;
-   Loot System Foundation;
-   Runtime State Foundation.
