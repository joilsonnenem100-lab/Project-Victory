# Project-Victory Roadmap

## Objetivo

Este documento representa o roadmap oficial de desenvolvimento do Project-Victory.

Seu objetivo é definir a ordem de construção da arquitetura, documentar o estado atual do projeto e servir como referência obrigatória para qualquer implementação futura.

Toda decisão de engenharia deverá consultar este documento antes de iniciar um novo módulo.

---

# Fonte de Verdade

Este Roadmap é a referência oficial para a ordem de desenvolvimento do projeto.

Nenhum módulo deverá ser iniciado fora da sequência definida sem uma revisão arquitetural formal.

---

# Estados dos Módulos

Cada módulo deve possuir exatamente um dos seguintes estados:

- Planned
- In Progress
- Review
- Locked

Definições:

Planned

O módulo ainda não foi iniciado.

In Progress

O módulo está em discussão e documentação.

Review

O módulo está em revisão arquitetural.

Locked

O módulo foi aprovado e passa a fazer parte da arquitetura oficial do Project-Victory.

Após um módulo ser marcado como Locked, alterações somente poderão ocorrer através de uma revisão arquitetural documentada.

---

# Fase 0 — Foundation

Status: LOCKED

Módulos:

- Aggregate Root
- Entities
- Value Objects
- Character

---

# Fase 1 — Character Domain

Status: LOCKED

Módulos:

- Health
- Mana
- Experience
- Level
- Inventory
- Equipment
- Skills

---

# Fase 2 — Progression

Status: LOCKED

Módulos:

- Skill Progression
- Trainer
- Offline Training *(emenda 2026-11-06, `DEC-20261106-21`: passa a ser **Treino com a página aberta**, item de cargas (1 h por carga), uma skill por item, fórmula do Tibia; não há progresso offline)*
- Skill Progress

---

# Fase 3 — Item System

Status: LOCKED

Módulos:

- Items
- Item Template
- Item Instance
- Item Factory
- Weapons
- Weapon Categories
- Affixes
- Imbuements

---

# Fase 4 — Loot

Status: LOCKED

Módulos:

- Loot
- Loot Tables
- Loot Entries

---

# Fase 5 — Engine Foundation

Status: LOCKED

Módulos:

- Vocations
- Resources
- Combat Architecture

---

# Fase 6 — Gameplay Foundation

Status: IN PROGRESS

Módulos:

- Monsters (LOCKED)
- Monster AI (~~PLANNED~~ REVIEW — implementado no `PV-MONSTER-010`; LOCK depois do R-050 e do R-043 — revisão de fechamento do M4, 2026-11-06: `docs/architecture/historico/REVISAO-FECHAMENTO-M4.md`)
- Experience Distribution (~~PLANNED~~ REVIEW — implementado no `PV-XP-010`; LOCK depois do R-045 — mesma revisão)
- Spawn System (~~PLANNED~~ REVIEW — implementado no `PV-SPAWN-010`; ~~LOCK no M6, depois do R-044/R-046~~ R-044/R-046 fechados na Fila 7; LOCK depois do R-057 — `docs/architecture/historico/REVISAO-FECHAMENTO-FILA-7.md`)

---

# Capacidades de Runtime e Eventos (M6/M7) — fora das fases

Status: REVIEW (revisão de fechamento da Fila 7, `docs/architecture/historico/REVISAO-FECHAMENTO-FILA-7.md`); Fila 8 (`PV-GOV-013`): Recovery e Event System **LOCKED**, os demais continuam REVIEW

Módulos (implementados; LOCKED onde indicado):

- Game Session + Runtime Coordinator (REVIEW — `PV-RUNTIME-010`; ~~falta R-051, R-054, R-055~~ fechados na Fila 8; **falta (PV-GOV-013):** Handbook §10 × código (o Coordinator do Handbook encadeia Hunt→Encounter→Combat→Reward), documento de domínio `game-session.md`, R-051 resto (encounters por área), R-062)
- Hunt Loop + encerramento por queda (REVIEW — `PV-HUNT-011/012`; falta R-059, R-052, R-053, R-027, R-030, R-060, R-061)
- Recovery do Runtime — **LOCKED** (Fila 8, `PV-GOV-013`, `docs/architecture/REVISAO-FECHAMENTO-FILA-8.md`: Handbook §18/§19 e ADR-057 Parte A cumpridos; R-055/R-056 fechados). ~~(REVIEW — `PV-RUNTIME-011`; falta R-055, R-056)~~
- Event System: Typed Event Bus e Domain Events — **infraestrutura de Engine Events**, não confundir com o módulo "Events" (eventos de jogo) da Fase 10 — **LOCKED** (Fila 8, `PV-GOV-013`: `events.md` cumprido com a emenda do `ADR-058`; R-058 fechado). ~~(REVIEW — `PV-EVENT-010/011`; falta R-058)~~

---

# Fase 7 — World

Status: PLANNED

Módulos:

- NPCs
- Cities
- Maps
- Areas
- Regions

---

# Fase 8 — Economy

Status: PLANNED

Módulos:

- Market
- Trading
- Vendors
- Currency

---

# Fase 9 — Systems

Status: PLANNED

Módulos:

- Crafting
- Fishing
- Achievements
- Guilds
- Party
- Social Systems

---

# Fase 10 — Endgame

Status: PLANNED

Módulos:

- Bosses
- Raids
- Events
- Seasons
- Future Expansions

---

# Roadmap Rules

Antes de iniciar qualquer módulo, o arquiteto deverá obrigatoriamente:

1. Consultar este Roadmap.
2. Verificar as dependências do módulo.
3. Confirmar que não existem módulos anteriores pendentes.
4. Verificar se existe documentação pendente.
5. Atualizar o estado do Roadmap.

Nenhuma implementação poderá ser iniciada sem seguir esta sequência.

---

# Revisões Arquiteturais

Caso seja identificado um problema estrutural durante o desenvolvimento:

- o Roadmap deverá ser atualizado;
- a revisão deverá ser documentada;
- o impacto deverá ser registrado;
- o módulo afetado deverá retornar para o estado "Review", caso necessário.

---

# Situação Atual

Sprint Atual:

Gameplay Foundation

Objetivo:

~~Consolidar a documentação oficial do módulo Monsters, finalizar sua revisão arquitetural e prosseguir para os próximos módulos da Gameplay Foundation.~~ *(Atualizado na revisão de fechamento do M4, 2026-11-06.)* Monster AI, Experience Distribution e Spawn System estão implementados e em REVIEW; cada LOCK depende das pendências listadas na revisão de fechamento do M4 (R-050/R-043, R-045, ~~R-044/R-046~~). *(Fila 7, `docs/architecture/historico/REVISAO-FECHAMENTO-FILA-7.md`: R-044/R-046 fechados; o Spawn agora depende do R-057; os módulos novos do M6/M7 estão na seção "Capacidades de Runtime e Eventos", todos em REVIEW.)*

Próximo módulo:

~~Monster AI~~ Fechamento da Fase 6 (~~Spawn com o M6, `PV-HUNT-011`~~ Spawn com o R-057 — Respawn Policy, Spawn Delay, Spawn Definition versionada; Monster AI com as skills de monstro; Experience Distribution com a Hunt em grupo) e das capacidades de Runtime e Eventos (R-051 a R-061)