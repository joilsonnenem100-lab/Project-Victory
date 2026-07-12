# Spell System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-019

---

# Objetivo

Definir o funcionamento oficial do sistema de Spells do Project-Victory.

Spells representam a forma como o personagem manifesta seu poder.

---

# Filosofia

No Project-Victory, uma Spell representa força.

A Spell representa:

- identidade;
- estilo de jogo;
- especialização;
- poder.

---

# Relação entre sistemas

```
Skill

↓

Magic Level

↓

Spell

↓

Spell Upgrade

↓

Combat
```

---

# Tipos de Spell

## Attack Spell

Spell ofensiva.

Exemplo:

```
Fire Bolt
```

Função:

- causar dano;
- aplicar elementos;
- participar do cálculo de combate.

---

## Area Spell

Spell que afeta múltiplos alvos.

Exemplo:

```
Fire Wave
```

Função:

- atingir monstros do Pack;
- aumentar eficiência contra grupos.

---

## Support Spell

Spell defensiva ou auxiliar.

Exemplo:

```
Heal
```

Função:

- recuperação;
- suporte;
- sobrevivência.

---

## Instance Spell

Spell de postura permanente.

Características:

- exclusiva por vocação;
- efeito permanente;
- não consome Mana;
- representa identidade da classe.

---

# Aprendizado de Spell

Spell é desbloqueada automaticamente.

Requisitos:

- Level;
- Vocação;
- Ascension.

O jogador visualiza os requisitos no tooltip.

---

# Spell Upgrade

Cada Spell possui evolução individual.

Exemplo:

```
Fire Wave

Upgrade Level 10


Ice Wave

Upgrade Level 3
```

---

# Limite Inicial

Upgrade máximo inicial:

```
Level 10
```

Esse valor deve ser configurável.

---

# Spell Forge

Sistema responsável pelo desenvolvimento das Spells.

Fluxo:

```
Spell Forge

↓

Selecionar Spell

↓

Aplicar Upgrade

↓

Aumentar poder
```

---

# Progressão por Ascension

Cada Ascension libera novas possibilidades.

Exemplo:

```
Ascension 1

↓

Primeiros upgrades


Ascension 2

↓

Novos upgrades
```

---

# Reset

Somente Spell Upgrade pode possuir reset.

Não existe:

- reset de Skill;
- reset de Magic Level.

O reset permite:

```
Recuperar pontos investidos

↓

Aplicar em outra Spell
```

---

# Elementos

Spells podem possuir elementos:

- Fire;
- Ice;
- Earth;
- Death;
- Holy;
- Physical.

---

# Equipamentos

Equipamentos podem modificar Spells.

Exemplo:

```
Magic Staff

+

Fire Spell Damage
```

---

# Critical Mágico

Spells podem causar Critical.

Fluxo:

```
Dano mágico

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

# Cooldown

Cada Spell possui cooldown próprio.

O cooldown é verificado:

```
A cada turno de combate
```

---

# Sistema de Prioridade

O jogador define a ordem de utilização.

Exemplo:

```
1 - Heal

2 - Fire Wave

3 - Fire Bolt
```

---

# Uso de Spell por Turno

Regra:

O personagem pode utilizar:

```
1 Attack Spell

+

1 Heal/Support Spell
```

no mesmo turno.

---

# Ordem do Turno

```
Turno inicia

↓

Verifica Heal

↓

Verifica Support

↓

Verifica Attack Spell

↓

Executa Auto Attack
```

---

# Mana

Consomem Mana:

- Attack Spell;
- Area Spell;
- Support Spell.

Não consomem Mana:

- Instance Spell;
- Aura Spell.

---

# Falta de Mana

Quando não existe Mana suficiente:

O personagem continua combatendo.

Executa:

```
Auto Attack

+

Skill Damage

+

Rune Damage
```

---

# Integrações

SpellService comunica com:

- CharacterService;
- SkillService;
- CombatService;
- EquipmentService;
- SpellForgeService.

---

# Arquitetura

```
SpellService

↓

SpellTemplate

↓

CharacterSpell

↓

SpellUpgrade

↓

DamageCalculator
```

---

# Pendências futuras

Definir:

- lista inicial de Spells;
- Spells por vocação;
- valores de Mana;
- cooldowns;
- multiplicadores;
- Instance Spells;
- Aura Spells.