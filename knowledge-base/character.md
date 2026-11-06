# Character System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-022

---

# Objetivo

Definir o funcionamento oficial do sistema de personagem do Project-Victory.

O Character representa a identidade e história do jogador dentro do mundo.

---

# Filosofia

No Project-Victory, um personagem representa identidade.

O personagem registra:

- evolução;
- escolhas;
- conquistas;
- equipamentos;
- habilidades;
- história.

---

# Relação com Account

Estrutura:

```
Account

↓

Character
```

A Account representa o jogador.

O Character representa a jornada dentro do mundo.

---

# Criação do Personagem

Ao criar um personagem:

Possui:

- Nome;
- Vocação;
- Sexo;
- Aparência padrão;
- Cidade inicial.

---

# Aparência

Não existe customização avançada inicialmente.

Todos iniciam utilizando:

```
Preset padrão
```

---

# Identidade Visual

O personagem possui:

## Outfit

Pode ser alterado.

---

## Mount

Sistema futuro.

Características:

- não customizável inicialmente.

---

# Nome

O nome do personagem possui:

- unicidade no servidor;
- regras de caracteres;
- proteção contra nomes proibidos.

---

# Vocação

A vocação é escolhida na criação.

Regra:

```
Vocação

=

Permanente
```

---

# Vocação Inicial

Vocação define:

- Skills;
- Spells;
- Equipamentos;
- estilo de combate.

---

# Progressão

O personagem evolui através de:

```
EXP

↓

Level

↓

Ascension

↓

Legend
```

---

# EXP

EXP pode ser obtida através de:

- Hunts;
- Quests;
- Eventos;
- Sistemas futuros.

A principal fonte é:

```
Hunt
```

---

# Level

O Level é baseado na EXP acumulada.

---

# Ascension

> *Emenda (2026-11-06, Fila 5, `DEC-20261106-30`; revisão formal do documento LOCKED; resolve R-034): o modelo abaixo ("5 Ascensions", "Legend = Level 500 + Ascension 5") foi **SUPERADO**. Títulos por Level, em configuração: Level 1–499 **Novice** (tier 0) · 500–999 **Ascension 1** (tier 1) · 1000–1499 **Ascension 2** (tier 2) · 1500–1999 **Ascension 3** (tier 3) · 2000–2499 **Ascension 4** (tier 4) · 2500 em diante **Legend** (tier 5, permanente, sem teto de Level). O Level é derivado da Experience, nunca reseta e não tem teto.*

O personagem possui:

```
5 Ascensions
```

---

# Legend

Ao alcançar:

```
Level 500

+

Ascension 5
```

O personagem se torna:

```
Legend
```

Após Legend:

```
Level sem limite máximo
```

---

# Sistema de Poder

> *Emenda (2026-11-06, Fila 5, `DEC-20261106-26/27`): os atributos do personagem são **somente os do Tibia** — Skills: Level, Magic Level, Fist, Club, Sword, Axe, Distance Fighting, Shielding, Fishing; Status: HP e Mana; sem Capacidade. Força, Destreza, Inteligência, Vitalidade, Sorte e Attack Speed **não existem** (nem "inicialmente").*

O personagem não possui atributos tradicionais.

Não existem inicialmente:

- Strength;
- Intelligence;
- Dexterity;
- Vitality.

O poder vem de:

```
Skills

+

Equipment

+

Spells
```

---

# HP e Mana

Todo personagem possui:

- HP;
- Mana.

---

# Morte

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

Mana cheia
```

---

# Penalidade de Morte

Não existe perda.

O jogador não perde:

- EXP;
- Gold;
- Items.

---

# Buff Após Morte

Buffs ativos permanecem.

Exemplo:

```
Buff de dano ativo

↓

Morte

↓

Retorno à cidade

↓

Buff continua ativo
```

---

# Informações Públicas

Outros jogadores podem visualizar:

- Level;
- Ascension;
- Guild;
- Títulos.

---

# Controle do Jogador

O jogador controla exposição de:

- Achievements;
- Equipamentos.

---

# Dados Persistentes

Character salva:

- Progressão;
- Inventário;
- Skills;
- Spells;
- Equipamentos;
- Histórico;
- Conquistas.

---

# Arquitetura

```
CharacterService

↓

CharacterEntity

├── IdentityComponent

├── ProgressionComponent

├── SkillComponent

├── SpellComponent

├── EquipmentComponent

├── InventoryComponent

└── AchievementComponent
```

---

# Integrações

CharacterService comunica com:

- AccountService;
- SkillService;
- SpellService;
- EquipmentService;
- InventoryService;
- CombatService;
- AchievementService.

---

# Pendências futuras

Definir:

- atributos internos;
- fórmula de EXP;
- tabela de Level;
- Ascension detalhada;
- cidades iniciais;
- aparência;
- títulos.