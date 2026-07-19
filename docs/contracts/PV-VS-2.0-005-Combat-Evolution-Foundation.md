docs/contracts/PV-VS-2.0-005-Combat-Evolution-Foundation.md

# PV-VS-2.0-005 — Combat Evolution Foundation

## Tipo

Technical Contract

---

## Objetivo

Evoluir o combate abstrato atual para um fluxo baseado em entidades de combate reais.

---

## Motivação

O Vertical Slice atual valida apenas o ciclo de execução da Hunt, mas o combate ainda representa apenas um resultado final.

Esta fundação adiciona a primeira camada de interação entre Player e Monster.

---

## Escopo

Implementar:

- Resolução de combate utilizando Monster Encounter.
- Aplicação de dano.
- Controle de Health do Monster.
- Determinação de vitória ou derrota.
- Retorno do resultado do combate.

---

## Fluxo esperado

Player

↓

Combat Start

↓

Monster Encounter

↓

Aplicação de dano

↓

Monster derrotado

↓

Combat Result

↓

Reward Flow

---

## Requisitos arquiteturais

- Combat deve ser responsável pela resolução da batalha.
- Monster deve fornecer dados necessários para combate.
- Character deve permanecer responsável pelo estado do jogador.
- Reward não deve fazer parte da lógica de combate.
- Combat não deve conhecer regras de progressão.

---

## Dados mínimos

Combat deve considerar:

- Identificação do Character.
- Identificação do Monster.
- Health do Monster.
- Resultado da batalha.

---

## Restrições

Não implementar:

- Skills.
- Classes.
- Equipamentos.
- Elementos.
- Status effects.
- Combate em tempo real.
- IA avançada.
- Balanceamento final.

---

## Critérios de aceite

Considerado aprovado quando:

Jogador inicia uma Hunt

↓

Monster Encounter é criado

↓

Combate é executado

↓

Monster possui estado de vida

↓

Resultado de vitória ou derrota é apresentado

---

## Dependências

Depende de:

PV-VS-2.0-004 — Monster Encounter Foundation

---

## Status

PV-VS-2.0-005 — Planejamento arquitetural.