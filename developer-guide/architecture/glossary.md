# Project-Victory

# Glossary

Versão: 1.0

Status: Oficial

---

# Objetivo

Este documento define o significado oficial dos principais termos utilizados na arquitetura do Project-Victory.

Seu objetivo é garantir que toda a equipe utilize a mesma terminologia durante o desenvolvimento.

Em caso de conflito entre interpretações, este documento prevalece.

---

# Aggregate Root

Objeto principal responsável por controlar um Aggregate.

No Project-Victory, Character representa um Aggregate Root.

---

# Action

Representa qualquer ação executada por uma Combat Entity.

Exemplos:

- Auto Attack
- Spell
- Rune
- Consumable
- Future Actions

Toda Action percorre a Combat Engine.

---

# Affix

Modificador permanente gerado durante a criação de uma Item Instance.

Affixes pertencem exclusivamente às Item Instances.

Affixes nunca pertencem ao Item Template.

---

# Buff

Modificador temporário que concede benefícios durante determinado período.

Buffs nunca alteram permanentemente o domínio.

---

# Combat Context

Objeto imutável criado para representar todos os dados necessários para execução de uma Action.

Combat Engine utiliza exclusivamente Combat Context durante seus cálculos.

---

# Combat Engine

Sistema responsável pela resolução de todas as Actions de combate.

Combat Engine nunca implementa regras de Game Design.

---

# Combat Entity

Qualquer entidade capaz de participar de um combate.

Exemplos:

- Character
- Monster
- Boss
- Summon
- Future Entities

---

# Combat Result

Resultado produzido pela Combat Engine após a execução de uma Action.

Toda Action produz exatamente um Combat Result.

---

# Condition

Estado temporário aplicado a uma Combat Entity.

Exemplos:

- Poison
- Burn
- Freeze
- Root

Conditions possuem duração limitada.

---

# Debuff

Modificador temporário prejudicial.

Debuffs nunca alteram permanentemente o domínio.

---

# Equipment Profile

Conjunto de permissões definido por uma Vocation para utilização de equipamentos.

---

# Item Instance

Representa um item existente dentro do mundo do jogo.

Cada Item Instance é única.

---

# Item Template

Representa a definição permanente de um item.

Item Templates nunca representam itens existentes no mundo.

---

# Loot

Sistema responsável por selecionar Item Templates como recompensa.

Loot nunca cria Item Instances.

---

# Progress

Valor interno responsável pela evolução gradual de uma Skill.

Progress representa a barra de evolução da Skill.

---

# Resource

Valor dinâmico pertencente a uma Combat Entity.

Exemplos:

- Health
- Mana

Resources representam estado atual.

---

# Skill

Proficiência permanente evoluída através da utilização correspondente.

Exemplos:

- Sword Fighting
- Magic Level
- Shielding

---

# Skill Progression Profile

Perfil pertencente à Vocation que define a velocidade de evolução de cada Skill.

---

# Target Result

Resultado individual produzido sobre uma Combat Entity específica durante um Combat Result.

Um Combat Result pode conter vários Target Results.

---

# Tier

Camada permanente de progressão pertencente ao Item Template.

Tier influencia diversos sistemas do jogo.

---

# Vocation

Perfil permanente do Character.

Vocation define parâmetros utilizados pelos demais sistemas da Engine.

Vocation nunca implementa regras de combate.

---

# Weapon Category

Categoria funcional de uma arma.

Categorias oficiais:

- Fist
- Club
- Sword
- Axe
- Distance

Weapon Category determina qual Skill será utilizada durante o combate.