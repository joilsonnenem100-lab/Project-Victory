# VC-LAB Database — Character

Status: Draft

Sprint: 030

## Objetivo

Definir a estrutura da entidade Character e seus componentes.

## Character

Campos:

- Id
- AccountId
- Name
- Vocation
- Gender
- Experience
- Level
- Ascension
- Legend
- CurrentRegionId
- CreatedAt
- UpdatedAt
- DeletedAt

## Componentes

- CharacterStatistics
- SkillProgress
- SpellProgress
- QuestProgress
- AchievementProgress
- CollectionProgress

## Filosofia

Character representa apenas identidade e progressão principal.

Toda funcionalidade independente deve possuir sua própria tabela.