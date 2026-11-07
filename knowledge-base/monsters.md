# Monster System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-016

---

# Objetivo

Definir o funcionamento oficial do sistema de monstros do Project-Victory.

Monstros são o núcleo do ciclo:

```
Hunt

↓

Monster

↓

Combat

↓

EXP

↓

Loot

↓

Progressão
```

---

# Filosofia

No Project-Victory, um monstro representa desafio.

O monstro existe para gerar:

- desafio;
- EXP;
- Loot;
- evolução do personagem.

---

# Estrutura de Hunt

A Hunt funciona através de Packs.

Modelo inicial:

```
Pack

├── Monster Instance 1
├── Monster Instance 2
└── Monster Instance 3
```

---

# Pack System

Regra inicial:

Cada Pack possui:

```
3 monstros
```

A arquitetura permite alterar esse valor futuramente.

---

# Monster Individual

Cada monstro dentro do Pack é independente.

Cada Monster Instance possui:

- HP próprio;
- dano próprio;
- defesa própria;
- morte própria;
- EXP própria;
- Loot próprio.

---

# Ataques

## Single Target

Ataque comum:

Afeta:

```
1 Monster Instance
```

---

## Area Damage

Magias de área:

Afetam:

```
Todos os monstros do Pack
```

---

# Arquitetura

Separação:

```
MonsterTemplate

↓

MonsterInstance
```

---

# MonsterTemplate

Representa o modelo do monstro.

Exemplo:

```
Dragon

HP Base:
Damage Base:
Defense Base:
EXP:
Loot Table:
```

---

# MonsterInstance

Representa uma criatura criada durante uma Hunt.

Exemplo:

```
Dragon Instance #92831
```

Possui:

- estado atual;
- HP atual;
- combate ativo;
- resultado de loot.

---

# Tipos de Monstros

Tipos oficiais:

- Normal;
- Magic;
- Elite;
- Boss;
- World Boss.

---

# Normal Monster

Monstro padrão.

Possui:

- atributos base;
- loot comum;
- EXP.

---

# Magic Monster

Versão especial.

Possui modificadores superiores.

---

# Elite Monster

Elite é uma variação mais rara.

Características:

- maior dificuldade;
- maior recompensa;
- maior chance de loot raro.

---

# Elite Spawn

Cada Hunt possui:

```
Elite Spawn Chance
```

---

# Regra Elite

Máximo:

```
1 Elite por Pack
```

Exemplo permitido:

```
Dragon

Dragon

Dragon Elite
```

Exemplo não permitido inicialmente:

```
Dragon Elite

Dragon Elite

Dragon
```

---

# Boss

Boss possui:

- acesso próprio;
- portal;
- chave específica.

Boss não aparece na Hunt normal.

---

# World Boss

World Boss ocorre através de eventos do servidor.

Possui:

- escala especial;
- participação global;
- recompensas especiais.

---

# Monster Attributes

Todo monstro pode possuir:

## Base

- HP;
- Damage;
- Defense;
- EXP;
- Loot Table.

---

## Avançados

- Resistências;
- Fraquezas;
- Modificadores.

---

# Monster Level

Monstros não possuem Level tradicional.

A dificuldade vem através de:

- atributos internos;
- raridade;
- versões de Ascension.

---

# Resistências

Alguns monstros possuem:

- Fire Resistance;
- Ice Resistance;
- Earth Resistance;
- Death Resistance;
- Holy Resistance.

---

# Fraquezas

Alguns monstros possuem fraquezas elementais.

Exemplo:

```
Ice Dragon

+50% Fire Damage

-80% Ice Damage
```

---

# Monster Skills

> *Nota de superação (2026-11-07, `DEC-20261107-02`, PO-06/PO-12 em `docs/product/PO-DECISOES-BASE.md`): monstros comuns **sem habilidades por enquanto** (veneno por segundo fica para depois); Bosses só na Fase 9.*

Somente Bosses possuem Skills especiais.

Monstros comuns utilizam comportamento simples.

---

# Monster AI

Modelo:

```
Spawn

↓

Combate automático

↓

Morte
```

Não existe IA complexa inicialmente.

---

# Movimento

Monstros não possuem movimentação real.

O foco é:

- dados;
- combate;
- progressão.

---

# Spawn Controller

Responsável por:

- criar Packs;
- controlar respawn;
- aplicar Elite Chance.

Fluxo:

```
Player inicia Hunt

↓

Spawn Pack

↓

Combate

↓

Todos monstros mortos

↓

1 segundo

↓

Novo Pack
```

---

# EXP

Ao morrer:

```
Monster Death Event

↓

ExperienceService
```

---

# Loot

Cada Monster Instance possui cálculo independente.

Exemplo:

Pack:

```
Dragon

Dragon

Dragon Elite
```

Resultado:

```
Dragon #1

↓

Loot Calculation


Dragon #2

↓

Loot Calculation


Dragon Elite

↓

Loot Calculation
```

---

# Integrações

MonsterService comunica com:

- HuntService;
- CombatService;
- ExperienceService;
- LootService;
- CollectionService;
- AchievementService.

---

# Pendências futuras

Definir:

- atributos exatos;
- fórmula de EXP;
- escala por Ascension;
- tabelas de monstros;
- resistências específicas;
- bosses.