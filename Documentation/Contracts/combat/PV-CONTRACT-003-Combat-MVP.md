# PV-CONTRACT-003 --- Combat MVP

## Local recomendado

Project-Victory/Documentation/Contracts/Combat/

------------------------------------------------------------------------

# Objetivo

Implementar o combate mínimo funcional do primeiro ciclo jogável.

Fluxo:

Encounter

↓

Combat Start

↓

Player Turn

↓

Monster Turn

↓

Damage Resolution

↓

Death

↓

Reward

------------------------------------------------------------------------

# 1. Ordem dos Turnos

Decisão:

O jogador sempre inicia o combate.

Fluxo:

Player Turn

↓

Monster Turn

------------------------------------------------------------------------

# 2. Ataque Básico

O dano do ataque básico utiliza:

-   classe;
-   equipamento;

A fórmula completa será definida pela arquitetura de combate.

O MVP não limita o dano somente a uma fonte.

------------------------------------------------------------------------

# 3. Skills Ofensivas

Skills iniciais possuem dano baseado em progressão.

Não utilizar somente valor fixo.

O dano pode considerar:

-   Magic Level;
-   Skills;
-   outros modificadores futuros.

------------------------------------------------------------------------

# 4. Cura

Heal não possui valor fixo.

A cura escala com Magic Level.

------------------------------------------------------------------------

# 5. Custo de Mana

Cada skill possui custo fixo de Mana.

Exemplo:

Exori Vis:

Mana Cost definido pela skill.

------------------------------------------------------------------------

# 6. Chance de Acerto

Inicialmente:

Existe chance de acerto fixa.

Não depende de:

-   equipamento;
-   atributos;
-   level.

------------------------------------------------------------------------

# 7. Dano Recebido

Inicialmente:

O dano dos monstros é aplicado diretamente.

Exemplo:

Rat:

Damage 5

↓

Character recebe dano

Sistemas de mitigação ficam para evolução futura.

------------------------------------------------------------------------

# 8. Armour

Não participa do cálculo de dano no MVP.

Existe como conceito futuro.

------------------------------------------------------------------------

# 9. Morte do Jogador

Ao morrer:

-   perde a Hunt atual;
-   retorna para cidade/tela inicial.

------------------------------------------------------------------------

# 10. Log de Combate

O log mostra apenas eventos principais.

Exemplo:

Knight attacked Rat

Damage: 10

Rat HP: 10/20

Não utilizar log extremamente detalhado inicialmente.

------------------------------------------------------------------------

# 11. Recompensa do Combate

Cada monstro fornece sua própria recompensa.

Fluxo:

Monster Dead

↓

Reward Individual

↓

Próximo alvo

Para packs de monstros:

Cada morte gera sua recompensa.

------------------------------------------------------------------------

# 12. Equipamento Durante Combate

Não é permitido trocar equipamento durante combate.

------------------------------------------------------------------------

# Estado Final do Contrato

Definido:

-   combate por turnos;
-   jogador inicia;
-   ataque básico;
-   skills;
-   mana;
-   chance de acerto;
-   dano inicial simplificado;
-   morte;
-   log;
-   recompensa individual;
-   bloqueio de troca durante combate.

------------------------------------------------------------------------

# Fora de Escopo

Não implementar:

-   crítico;
-   resistências;
-   armour calculation;
-   status effects;
-   PvP;
-   IA avançada de monstros.

------------------------------------------------------------------------

# Próximo contrato

PV-CONTRACT-004 --- Reward Integration
