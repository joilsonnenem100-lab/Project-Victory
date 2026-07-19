docs/contracts/PV-VS-2.0-003-Hunt-Selection-Foundation.md

# PV-VS-2.0-003 — Hunt Selection Foundation

## Tipo

Technical Contract

---

## Objetivo

Transformar a execução direta de Hunt em uma seleção de conteúdo jogável pelo jogador.

---

## Motivação

O Vertical Slice atual executa uma Hunt fixa através do fluxo existente.

Esta fundação cria a primeira representação de conteúdo selecionável pelo jogador.

---

## Escopo

Implementar:

- Definição de uma Hunt.
- Identificação única da Hunt.
- Exibição da Hunt disponível no Client.
- Seleção da Hunt pelo jogador.
- Envio da Hunt selecionada para execução.

---

## Fluxo esperado

Client

↓

Lista de Hunts disponíveis

↓

Jogador seleciona Hunt

↓

Execute Hunt

↓

Combat Flow

↓

Reward Flow

---

## Primeira implementação

Criar apenas uma Hunt funcional.

Exemplo:

Forest Hunt

A primeira Hunt deve servir como base para evolução futura.

---

## Requisitos arquiteturais

- Hunt deve possuir identificação própria.
- A execução deve receber uma referência da Hunt selecionada.
- Não colocar regras de combate dentro da Hunt.
- Não colocar regras de recompensa dentro da Hunt.
- Manter separação entre conteúdo e execução.

---

## Restrições

Não implementar:

- Múltiplas Hunts complexas.
- Sistema de dificuldade.
- Ranking.
- Matchmaking.
- Grupo de jogadores.
- Conteúdo procedural.
- Balanceamento avançado.

---

## Critérios de aceite

Considerado aprovado quando:

Jogador entra no Client

↓

Visualiza Hunt disponível

↓

Seleciona Hunt

↓

Executa Hunt selecionada

↓

Recebe resultado do fluxo existente

---

## Dependências

Depende de:

PV-VS-2.0-001 — Persistence Foundation

PV-VS-2.0-002 — Account Experience Foundation

---

## Status

PV-VS-2.0-003 — Planejamento arquitetural.