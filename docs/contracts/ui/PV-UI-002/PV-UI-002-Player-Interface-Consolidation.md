# PV-UI-002 — Player Interface Consolidation

## Objetivo

Consolidar a interface principal do jogador utilizando a arquitetura visual definida no PV-UI-001.

Este contrato organiza a apresentação das informações existentes sem criar novas regras de gameplay.

## Estrutura alvo

Player Interface

├── Character Identity
├── Progression
├── Actions
├── Combat Status
└── Rewards

## Escopo

- Organização da tela principal.
- Composição das áreas do jogador.
- Uso dos componentes existentes.
- Separação visual das informações.

## Restrições

Não alterar:
- Backend.
- API.
- Domain.
- Character Rules.
- Hunt.
- Combat.
- Reward.
- Progression.

Não implementar:
- Skills.
- Classes.
- Equipamentos.
- Inventário.
- Economia.
