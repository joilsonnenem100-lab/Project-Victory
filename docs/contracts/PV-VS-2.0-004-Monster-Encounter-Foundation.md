docs/contracts/PV-VS-2.0-004-Monster-Encounter-Foundation.md

# PV-VS-2.0-004 — Monster Encounter Foundation

## Tipo

Technical Contract

---

## Objetivo

Criar a primeira estrutura de encontro contra um Monster real dentro do fluxo de Hunt.

---

## Motivação

O Vertical Slice atual possui combate abstrato com resultado direto.

Esta fundação introduz uma entidade de combate representando um inimigo real.

---

## Escopo

Implementar:

- Definição de Monster.
- Identificação única do Monster.
- Dados básicos de combate.
- Criação de Encounter durante uma Hunt.
- Associação entre Hunt e Monster Encounter.

---

## Fluxo esperado

Hunt selecionada

↓

Criar Encounter

↓

Criar Monster

↓

Executar Combat

↓

Resolver resultado

↓

Processar Reward

---

## Primeira implementação

Criar apenas um Monster funcional.

Exemplo:

Rat

Dados mínimos:

- Identificador.
- Nome.
- Health.

---

## Requisitos arquiteturais

- Monster deve possuir definição própria.
- Encounter deve representar uma instância de combate.
- Regras de combate permanecem no Combat.
- Monster não deve controlar recompensa diretamente.
- Monster não deve possuir lógica de Hunt.

---

## Restrições

Não implementar:

- IA de Monster.
- Movimento.
- Spawn avançado.
- Múltiplos tipos complexos.
- Boss System.
- Grupo de monstros.
- Balanceamento avançado.

---

## Critérios de aceite

Considerado aprovado quando:

Jogador inicia uma Hunt

↓

Um Monster Encounter é criado

↓

Combat utiliza um Monster definido

↓

Resultado da batalha é retornado

---

## Dependências

Depende de:

PV-VS-2.0-003 — Hunt Selection Foundation

---

## Status

PV-VS-2.0-004 — Planejamento arquitetural.