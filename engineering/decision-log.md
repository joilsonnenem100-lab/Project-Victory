---

# DEC-0247

## Título

Arquitetura Event-Driven.

## Origem

DB-001 — Persistence Model

## Status

LOCKED

## Decisão

Os principais acontecimentos do jogo serão propagados através de eventos.

Exemplo:

MonsterDeathEvent

↓

LootService

↓

ExperienceService

↓

CollectionService

↓

AchievementService

↓

QuestService

Cada sistema é responsável apenas pela sua própria lógica.

Nenhum sistema deve conhecer a implementação interna dos demais.

Objetivo:

- baixo acoplamento;
- alta escalabilidade;
- facilidade de expansão;
- manutenção simplificada.