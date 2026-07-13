# VC-LAB Runtime Engine

Status: Draft

Sprint: 034

## Filosofia

O Project-Victory é uma Engine de Simulação.

## Fluxo

API

↓

Command Queue

↓

Game Loop

↓

Simulation Engine

↓

Event Queue

↓

Persistence Queue

↓

PostgreSQL

## Componentes

- Command Queue
- Game Loop
- Simulation Engine
- Event Queue
- Persistence Queue

## Runtime

Objetos temporários:

- CharacterRuntime
- HuntRuntime
- CombatRuntime
- MonsterRuntime

Todos existem apenas em memória.

## Princípios

- Engine First
- Tick Driven
- Event Driven
- Command Driven
- Runtime em memória