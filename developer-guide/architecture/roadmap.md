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
- Offline Training
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
- Monster AI (PLANNED)
- Experience Distribution (PLANNED)
- Spawn System (PLANNED)

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

Consolidar a documentação oficial do módulo Monsters, finalizar sua revisão arquitetural e prosseguir para os próximos módulos da Gameplay Foundation.

Próximo módulo:

Monster AI