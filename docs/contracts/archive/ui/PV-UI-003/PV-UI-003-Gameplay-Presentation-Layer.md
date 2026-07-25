# PV-UI-003 — Gameplay Presentation Layer

## Objetivo

Criar uma camada visual específica para apresentação do gameplay utilizando a arquitetura UI definida anteriormente.

O contrato organiza a experiência visual durante gameplay sem alterar regras existentes.

## Estrutura alvo

Gameplay Presentation Layer

├── Player Area
├── Hunt Area
├── Combat Area
├── Result Area
└── Reward Area

## Escopo

- Organização visual do gameplay.
- Apresentação dos estados existentes.
- Reutilização dos componentes UI.

## Restrições

Não alterar:
- Hunt Domain.
- Combat Domain.
- Reward Logic.
- Progression Logic.
- Backend.
- API.

Não implementar:
- Skills.
- Classes.
- Equipamentos.
- Inventário.
- Economia.
