# Combat System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-017

---

# Objetivo

Definir o funcionamento oficial do sistema de combate do Project-Victory.

O combate é o núcleo matemático entre:

```
Character

↓

Combat

↓

Monster

↓

EXP

↓

Loot
```

---

# Filosofia

No Project-Victory, um combate representa desafio.

O combate representa:

- evolução;
- preparação;
- equipamentos;
- escolhas de build;
- estratégia.

---

# Modelo

O combate utiliza:

```
Turnos fixos
```

Não existe inicialmente:

- Attack Speed;
- Cast Speed.

> *Emenda (2026-11-06, Fila 5, `DEC-20261106-27`): Attack Speed e Cast Speed **não existem** (não só "inicialmente"): não são stats do Tibia.*

---

# Fluxo de Turno

```
Turno inicia

↓

Jogador ataca

↓

Monstros atacam

↓

Calcula dano

↓

Atualiza HP

↓

Próximo turno
```

---

# Ordem do Turno

Prioridade:

```
1. Jogador

2. Monstros
```

---

# Seleção de Alvo

Ataque Single Target:

Afeta:

```
Primeiro monstro da Pack List
```

---

# Troca de Alvo

Quando um monstro morre:

```
Novo alvo selecionado instantaneamente
```

---

# Packs

O combate acontece dentro de Packs.

Modelo:

```
Pack

├── Monster 1
├── Monster 2
└── Monster 3
```

---

# Ataques

## Single Target

Afeta:

```
1 Monster Instance
```

---

## Area Damage

Depende da Spell.

Pode afetar:

```
Todos os monstros do Pack
```

ou quantidade específica.

---

# Dano Físico

Ordem de importância:

```
Weapon

↓

Skill

↓

Level
```

---

# Dano Mágico

Ordem de importância:

```
Level

↓

Magic Level

↓

Equipment

↓

Spell
```

---

# Spell Upgrade

Spells podem possuir evolução.

Objetivo:

Permitir diferentes estilos de jogo.

Exemplo:

Jogador pode investir em uma Spell favorita.

---

# Variação de Dano

Todo ataque acerta.

Não existe Miss.

Existe variação:

```
Dano mínimo

↓

Dano calculado

↓

Dano máximo
```

---

# Cálculo Elemental

Ordem:

```
Dano Calculado

↓

Defesa

↓

Resistência

↓

Fraqueza

↓

Critical
```

---

# Defesa

Cada tipo de dano possui defesa própria.

Exemplos:

- Physical Defense;
- Fire Defense;
- Ice Defense;
- Death Defense.

---

# Critical

Critical é calculado no final.

Fluxo:

```
Dano final

↓

Critical

↓

Resultado final
```

Critical representa grandes picos de dano.

---

# Morte do Personagem

Quando:

```
HP <= 0
```

Fluxo:

```
Morte

↓

Cidade Principal

↓

HP cheio

↓

MP cheio
```

---

# Morte do Monstro

Fluxo:

```
Monster Death

↓

EXP

↓

Loot

↓

Collection

↓

Achievement
```

---

# Vocação e Dano

## Knight

Fonte principal:

```
Weapon

+

Sword/Club/Axe Skill
```

---

## Paladin

Fonte principal:

```
Distance Weapon

+

Distance Skill
```

---

## Monk

Fonte principal:

```
Weapon

+

Combat Skill
```

---

## Sorcerer / Druid

Fonte principal:

```
Spell

+

Magic Level
```

---

# Arquitetura

```
CombatService

↓

DamageCalculator

↓

DefenseCalculator

↓

ResistanceCalculator

↓

CriticalCalculator
```

---

# Objetivo da Arquitetura

Permitir:

- balanceamento independente;
- ajustes sem quebrar sistemas;
- criação de novas mecânicas futuramente.

---

# Integrações

CombatService comunica com:

- CharacterService;
- EquipmentService;
- SkillService;
- MonsterService;
- ExperienceService;
- LootService.

---

# Pendências futuras

Definir:

- fórmulas matemáticas;
- valores iniciais;
- dano base das armas;
- multiplicadores das skills;
- cálculo de resistência;
- cálculo de critical.