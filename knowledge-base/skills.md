# Skill System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-018

---

# Objetivo

Definir o funcionamento oficial das Skills do Project-Victory.

---

# Filosofia

No Project-Victory, uma Skill representa poder.

Skills representam:

- evolução;
- treinamento;
- especialização;
- força construída pelo jogador.

---

# Skills Oficiais

O sistema possui:

## Sword

Relacionada ao combate com espadas.

---

## Club

Relacionada ao combate com armas Club.

---

## Axe

Relacionada ao combate com machados.

---

## Distance

Relacionada ao combate à distância.

---

## Shielding

Relacionada à defesa com escudos.

---

## First Fighting

Relacionada ao combate físico sem armas.

---

## Magic Level

Relacionada ao poder mágico.

---

# Evolução

Skills evoluem através de:

```
Uso

+

Treinamento
```

---

# Magic Level

Magic Level evolui através de:

- utilização de magias;
- treinamento.

---

# Progressão

Skills possuem:

```
Level da Skill

↓

Maior eficiência

↓

Maior poder
```

---

# Limite

Skills não possuem limite máximo.

A progressão é infinita.

Porém:

```
Maior Skill

↓

Maior custo de evolução
```

A dificuldade aumenta exponencialmente.

---

# Influência no Combate

Skills influenciam:

- dano;
- eficiência.

Exemplos:

Knight:

```
Sword

↓

Dano físico
```

Paladin:

```
Distance

↓

Dano à distância
```

Sorcerer/Druid:

```
Magic Level

↓

Dano mágico
```

---

# Reset

Skills não possuem reset.

O progresso é permanente.

---

# Spell Upgrade

Spell Upgrade é separado.

Regra:

```
Skill

=

Evolução permanente


Spell Upgrade

=

Especialização alterável
```

---

# Arquitetura

```
SkillService

↓

SkillEntity

↓

SkillProgression

↓

CharacterComponent
```

---

# Integrações

SkillService comunica com:

- CharacterService;
- CombatService;
- SpellService;
- TrainingService.

---

# Pendências futuras

Definir:

- fórmula de ganho;
- multiplicadores;
- custo exponencial;
- treinamento offline;
- bônus de equipamentos.