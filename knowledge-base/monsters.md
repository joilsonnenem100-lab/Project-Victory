# Monster System

Version: 1.1

Status: EM DESENVOLVIMENTO

Last Sprint: SPRINT-004

---

# Objetivo

Definir o funcionamento oficial dos monstros do Project-Victory.

Este documento contém apenas decisões aprovadas.

---

# Filosofia

No Project-Victory, um monstro representa uma nova oportunidade de conseguir um item raro, um recurso importante ou evoluir o personagem.

Todo monstro existe para sustentar a progressão do jogador.

---

# Classificações Oficiais

- Normal
- Magic
- Elite
- Boss
- World Boss

---

# Normal

Monstro padrão das Hunts.

Representa a maior parte da Monster Pool.

---

# Magic

Versão fortalecida do monstro comum.

Possui atributos superiores.

Mantém o comportamento do monstro original.

---

# Elite

Versão rara do monstro.

Pode aparecer através da Monster Pool da Hunt.

Características:

- Mantém o mesmo visual do monstro original.
- Possui atributos superiores.
- Possui chance própria de Spawn.
- Inicialmente apenas um Elite pode existir por Pack.

Exemplo:

Pack

- Dragon
- Dragon
- Dragon Elite

---

# Boss

Bosses não aparecem naturalmente nas Hunts.

Cada Boss possui:

- Boss Room própria;
- Boss Key específica;
- Loot exclusivo;
- Skills próprias.

---

# World Boss

World Bosses aparecem apenas durante eventos oficiais do servidor.

Nunca fazem parte da Monster Pool das Hunts.

---

# Monster Pool

Cada Hunt possui sua própria Monster Pool.

Exemplo:

Dragon Lair South

Monster Pool

- Dragon
- Dragon Elite

A Monster Pool pode ser modificada temporariamente durante eventos.

Essa alteração não exige criação de uma nova Hunt.

---

# Loot

O Loot pertence exclusivamente ao Monstro.

Cada monstro possui sua própria Loot Table.

As Hunts não possuem Loot próprio.

---

# Spawn

O Spawn ocorre através do Spawn Controller da Hunt.

Fluxo:

Monster Pool

↓

Pack

↓

Todos derrotados

↓

1 segundo

↓

Novo Pack

---

# IA

O Project-Victory utiliza uma arquitetura simplificada.

Os monstros não possuem IA complexa.

Não existe:

- patrulha;
- retorno ao spawn;
- navegação;
- perseguição complexa.

Os monstros existem apenas para sustentar o combate da Hunt.

---

# Resistências

Alguns monstros podem possuir:

- Resistências Elementais;
- Fraquezas Elementais.

Essas propriedades dependem do monstro.

---

# Skills

Apenas Bosses possuem Skills exclusivas.

Monstros comuns utilizam apenas atributos básicos de combate.

---

# Progressão

Versões mais fortes de monstros poderão aparecer em Ascensions superiores.

O comportamento permanece o mesmo.

Mudam apenas:

- atributos;
- drops;
- resistências;
- habilidades especiais (quando aplicável).

---

# Filosofia Idle

O sistema de monstros foi projetado para minimizar processamento.

A prioridade está em:

- estabilidade;
- simplicidade;
- escalabilidade.

---

# Pendências

Ainda precisam ser definidos:

- atributos base;
- fórmula de HP;
- fórmula de dano;
- velocidade de ataque;
- tabela oficial de resistências;
- Magic Monsters.

---

# Decisões Relacionadas

DEC-0018

DEC-0019

DEC-0020

DEC-0021