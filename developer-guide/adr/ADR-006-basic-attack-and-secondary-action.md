# ADR-006 - Basic Attack e Secondary Action

Status: ACCEPTED

Version: 1.0

---

# Contexto

Durante a definição da arquitetura do combate foi necessário estabelecer uma estrutura única para todos os participantes da Combat Engine.

O objetivo é eliminar regras especiais e manter o fluxo de combate simples, determinístico e escalável.

---

# Decisão

Todo turno de combate será composto por duas etapas sequenciais.

1. Basic Attack.
2. Secondary Action.

O Basic Attack é obrigatório.

A Secondary Action é opcional.

A Combat Engine executa ambas as etapas.

A Monster AI participa apenas da seleção da Secondary Action.

---

# Consequências

Benefícios:

- elimina turnos vazios;
- reduz regras especiais;
- simplifica a Combat Engine;
- simplifica a Monster AI;
- unifica o comportamento de todos os Combat Targets.

Custos:

- necessidade de manter o Basic Attack como parte obrigatória do fluxo de combate.

---

# Aplicações

Este princípio aplica-se a:

- Character;
- Companion;
- Monsters;
- Bosses;
- Summons;
- Future Combat Targets.

---

# Restrições

Todo Combat Target deverá possuir um Basic Attack.

A Monster AI nunca decide a execução do Basic Attack.

A Combat Engine é a única responsável pela execução do turno.

A Secondary Action poderá ser:

- Skill;
- Spell;
- Rune;
- qualquer outra Combat Action configurada.

---

# Referências

Ver também:

- developer-guide/domain/combat.md
- developer-guide/domain/monster-ai.md
- developer-guide/domain/monsters.md