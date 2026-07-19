# EXP-VS-206 --- Hunt Interaction Experience

## Tipo

Client Experience Contract

------------------------------------------------------------------------

# Referência arquitetural

Este contrato pertence ao:

PV-MILESTONE-002 --- Player Experience Vertical Slice

------------------------------------------------------------------------

# Dependências

EXP-VS-200 --- Client Foundation

↓

EXP-VS-203 --- Game Entry Experience

↓

EXP-VS-204 --- First World Scene Foundation

↓

EXP-VS-205 --- Player State Presentation

------------------------------------------------------------------------

# Objetivo

Definir a primeira interação visual do jogador com o sistema de Hunt
através do Client.

O Client deve apenas solicitar a ação e apresentar o resultado recebido.

------------------------------------------------------------------------

# Fluxo oficial

Player dentro do mundo

↓

Solicita Hunt

↓

Client envia request autenticada

↓

API executa fluxo existente

↓

Resultado retorna ao Client

------------------------------------------------------------------------

# Responsabilidades do Client

✅ Apresentação visual

✅ Interação do jogador

✅ Consumo da API

✅ Apresentação do resultado

------------------------------------------------------------------------

# O Client não deve possuir

❌ Regras de Hunt

❌ Cálculo de combate

❌ Cálculo de recompensa

❌ Progression

❌ Persistência

------------------------------------------------------------------------

# Critérios de aceite

✅ Jogador inicia Hunt pelo Client

✅ Backend executa a Hunt

✅ Resultado retorna corretamente

✅ Client não possui regras de gameplay

------------------------------------------------------------------------

# Status

EXP-VS-206 --- Hunt Interaction Experience

🟢 Liberado para implementação após aprovação arquitetural.
