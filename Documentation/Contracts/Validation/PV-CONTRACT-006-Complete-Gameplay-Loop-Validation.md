# PV-CONTRACT-006 --- Complete Gameplay Loop Validation

## Local recomendado

Project-Victory/Documentation/Contracts/Validation/

------------------------------------------------------------------------

# Objetivo

Validar o primeiro ciclo jogável completo do Project-Victory.

Este contrato não cria novas mecânicas.

Ele valida a integração entre todos os sistemas do Core Gameplay Loop.

------------------------------------------------------------------------

# Critério principal de conclusão

O primeiro Vertical Slice será considerado funcional quando existir um
ciclo completo de Hunt contendo:

-   dados de combate funcionando;
-   HP e Mana do jogador sendo consumidos e atualizados em tempo real na
    tela;
-   monstros participando do combate;
-   monstros derrotados gerando drop;
-   itens enviados ao inventário;
-   jogador recebendo experiência;
-   jogador evoluindo de Level conforme Experience Table.

------------------------------------------------------------------------

# Fluxo validado

Login

↓

Character

↓

Tela Inicial

↓

Hunt

↓

Encounter

↓

Combat

↓

Monster Death

↓

Reward

↓

Loot

↓

Inventory

↓

Progression

------------------------------------------------------------------------

# 1. Entrada do jogador

Validar:

Account

↓

Login

↓

Tela Inicial

Resultado esperado:

Jogador acessa o jogo.

------------------------------------------------------------------------

# 2. Character

Validar:

-   Classe;
-   Level inicial;
-   HP;
-   Mana;
-   Skill inicial.

Classes:

-   Knight;
-   Paladin;
-   Sorcerer;
-   Druid;
-   Monk.

------------------------------------------------------------------------

# 3. Hunt

Primeira Hunt:

Ascendancy Path

Regras:

-   Hunt disponível na tela inicial;
-   3 monstros por Encounter;
-   geração conforme monstros disponíveis.

------------------------------------------------------------------------

# 4. Combat Validation

Validar:

-   início do combate;
-   turnos;
-   ataque do jogador;
-   ataque do monstro;
-   dano;
-   redução de HP;
-   consumo de Mana por habilidades.

A tela deve apresentar o estado atual:

-   HP;
-   Mana;
-   informações principais de combate.

------------------------------------------------------------------------

# 5. Monster Validation

Monstros iniciais:

Rat:

HP: 20

Damage: 5

XP: 50

Gold: 1-5

Cave Rat:

HP: 50

Damage: 10

XP: 100

Gold: 2-6

------------------------------------------------------------------------

# 6. Reward Validation

Após derrotar monstro:

Monster Dead

↓

XP

↓

Gold

↓

Drop quando aplicável

------------------------------------------------------------------------

# 7. Item Validation

Quando existir drop:

Monster

↓

Item

↓

Inventory

O jogador deve visualizar o item recebido no inventário.

------------------------------------------------------------------------

# 8. Progression Validation

Validar:

Experience recebida.

Quando atingir a Experience Table:

Level Up.

Resultado esperado:

-   Level atualizado;
-   HP/Mana atualizados conforme classe;
-   progressão mantida.

------------------------------------------------------------------------

# 9. Hunt Completion

Objetivo inicial:

Completar uma Hunt.

Após conclusão:

Retornar para fluxo normal do jogo.

Não existe recompensa especial de conclusão.

------------------------------------------------------------------------

# 10. Repetição da Hunt

A primeira Hunt pode ser repetida infinitamente.

------------------------------------------------------------------------

# Primeiro Vertical Slice concluído quando:

O jogador consegue:

1.  Entrar no jogo.
2.  Criar personagem.
3.  Entrar na Hunt.
4.  Combater monstros.
5.  Visualizar HP/Mana em combate.
6.  Derrotar monstros.
7.  Receber loot.
8.  Guardar itens no inventário.
9.  Receber XP.
10. Subir Level pela Experience Table.

------------------------------------------------------------------------

# Fora de Escopo

Não validar:

-   PvP;
-   Crafting;
-   Affixes;
-   Raridade;
-   Bosses;
-   Economia avançada;
-   Sistemas sociais.

------------------------------------------------------------------------

# Estado

Este contrato representa o fechamento do primeiro Core Gameplay Loop do
Project-Victory.
