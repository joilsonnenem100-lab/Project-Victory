docs/contracts/PV-MILESTONE-003-Architecture-Decision-Record.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Architecture Decision Record

## Tipo

Architecture Decision Record

---

## Objetivo

Registrar as principais decisões arquiteturais tomadas para evolução do Vertical Slice 2.0.

---

## Decisão 1 — Evolução incremental

Decisão:

O Vertical Slice 2.0 será desenvolvido através de fundações independentes.

Motivo:

Reduzir retrabalho e manter controle de escopo.

---

## Decisão 2 — Persistência antes de novos sistemas

Decisão:

A implementação deve iniciar pela fundação de persistência.

Motivo:

Evitar criação de sistemas temporários em memória que precisarão ser substituídos posteriormente.

---

## Decisão 3 — Manter arquitetura existente

Decisão:

A evolução deve preservar a separação atual:

Domain

↓

Application

↓

Infrastructure

↓

API

↓

Client

---

## Decisão 4 — Hunt como conteúdo independente

Decisão:

Hunt será tratada como uma definição própria de conteúdo.

Motivo:

Permitir evolução futura sem acoplar combate ou recompensa diretamente.

---

## Decisão 5 — Monster como entidade de combate

Decisão:

Monster será introduzido como entidade própria dentro do fluxo de combate.

Motivo:

Substituir resultado abstrato por uma representação real de batalha.

---

## Decisão 6 — Reward desacoplada de Combat

Decisão:

Combat apenas determina resultado.

Reward será responsável por recompensas.

Motivo:

Manter responsabilidades separadas e permitir evolução futura.

---

## Decisão 7 — Item como fundação inicial

Decisão:

Adicionar Item antes de sistemas completos de inventário.

Motivo:

Permitir apresentação de recompensa sem criar sistemas futuros prematuramente.

---

## Decisão 8 — Controle de escopo

Decisão:

O Vertical Slice 2.0 não deve implementar sistemas completos de MMORPG.

Fora do escopo:

- PvP.
- Economia.
- Guild.
- Trade.
- Crafting.
- Skills completas.
- Classes completas.
- Equipamentos completos.

---

## Resultado esperado

Ao final do Vertical Slice 2.0:

O jogador deverá possuir um fluxo persistente:

Account

↓

Character

↓

Hunt

↓

Monster

↓

Combat

↓

Reward

↓

Item

---

## Status

PV-MILESTONE-003 — Decisões arquiteturais registradas.