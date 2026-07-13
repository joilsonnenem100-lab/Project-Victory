# VC-LAB Runtime — ECS Lite

Status: Draft

Sprint: 035

## Objetivo

Organizar o Runtime utilizando composição de Components e Systems.

## Estrutura

CharacterRuntime

↓

Components

↓

Systems

↓

Events

↓

Snapshots (opcional)

↓

Simulation Recorder (opcional)

## Princípios

- Components armazenam estado.
- Systems executam regras.
- Comunicação por eventos.
- Runtime baseado em composição.
- Arquitetura expansível por módulos.