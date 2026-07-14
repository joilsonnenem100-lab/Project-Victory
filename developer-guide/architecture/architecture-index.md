# Project-Victory

# Architecture Index

Versão: 1.0

Status: Oficial

---

# Objetivo

Este documento representa o índice oficial da arquitetura do Project-Victory.

Seu objetivo é organizar toda a documentação produzida durante o projeto.

Nenhum documento deverá existir fora desta estrutura sem justificativa arquitetural.

---

# Estrutura da Documentação

## Coordenação do Projeto

- roadmap.md
- project-state.md
- architecture-index.md
- decision-log.md
- glossary.md

---

## Foundation

- aggregate-root.md
- entities.md
- value-objects.md

---

## Character

- character.md
- health.md
- mana.md
- experience.md
- level.md
- inventory.md
- equipment.md
- skills.md

---

## Progression

- progression.md
- trainer.md
- offline-training.md

---

## Items

- items.md
- item-template.md
- item-instance.md
- item-factory.md
- weapons.md
- affixes.md
- imbuements.md

---

## Loot

- loot.md

---

## Engine

- vocations.md
- resources.md
- combat.md

---

## Gameplay

Planejado

- monsters.md
- monster-ai.md
- spawn.md
- experience-distribution.md

---

## World

Planejado

- maps.md
- cities.md
- regions.md
- npcs.md

---

## Economy

Planejado

- market.md
- vendors.md
- trading.md
- currency.md

---

## Systems

Planejado

- crafting.md
- fishing.md
- achievements.md
- guilds.md
- party.md

---

## Endgame

Planejado

- bosses.md
- raids.md
- events.md
- seasons.md

---

# Organização

Cada documento possui exatamente uma responsabilidade.

Nenhum documento deverá duplicar regras existentes em outro módulo.

Caso um documento comece a possuir múltiplas responsabilidades, deverá ser dividido através de uma revisão arquitetural.

---

# Dependências

Os documentos deverão respeitar as dependências definidas pelo Roadmap.

Nenhum módulo poderá depender de um módulo ainda não consolidado.

---

# Estado da Arquitetura

Todos os documentos deverão possuir um dos seguintes estados:

- Planned
- In Progress
- Review
- Locked

---

# Fonte de Verdade

A documentação representa a única fonte oficial de verdade do Project-Victory.

Em caso de conflito entre uma conversa e a documentação:

A documentação prevalece.

A conversa deverá ser utilizada apenas para produzir uma revisão arquitetural.