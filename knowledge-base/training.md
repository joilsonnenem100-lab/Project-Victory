# Training System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-020

---

# Objetivo

Definir o funcionamento oficial do sistema de Training do Project-Victory.

Training é uma forma complementar de evolução do personagem.

---

# Filosofia

No Project-Victory, um Training representa poder.

Training representa:

- evolução;
- preparação;
- progresso durante períodos de descanso.

---

# Papel do Training

Training permite evolução de:

- Skills;
- Magic Level.

Training não substitui a Hunt.

A principal fonte de progressão continua sendo:

```
Hunt

↓

Combate

↓

EXP

↓

Loot
```

---

# Regras Gerais

Durante Training:

O personagem:

- não caça;
- não recebe EXP;
- não participa de combate;
- não recebe progressão de outras fontes.

---

# Online e Offline

Training possui o mesmo funcionamento.

Não existe:

- bônus online;
- bônus offline.

O servidor controla todo o progresso.

---

# Item de Training

Cada treinamento necessita de um item específico.

Exemplo:

```
Distance Weapon

↓

Distance Training
```

---

# Training Charge System

O item possui cargas.

As cargas determinam a duração do treinamento.

Fluxo:

```
Jogador inicia Training

↓

Servidor verifica item

↓

Calcula cargas disponíveis

↓

Inicia TrainingSession

↓

Consome cargas

↓

Finaliza quando cargas acabam
```

---

# Training Session

Cada personagem pode possuir:

```
1 Training ativo
```

Não é permitido:

```
Sword Training

+

Shield Training
```

simultaneamente.

---

# Skills treináveis

Training pode evoluir:

- Sword;
- Club;
- Axe;
- Distance;
- Shielding;
- First Fighting;
- Magic Level.

---

# Velocidade de Progressão

Training é inferior à Hunt.

Regra:

```
Hunt

>

Training
```

A Hunt continua sendo a principal forma de evolução.

---

# Interrupção

O jogador pode cancelar Training.

O cancelamento não gera penalidade adicional.

---

# Interface

O jogador poderá visualizar:

- Skill treinada;
- progresso atual;
- cargas restantes;
- tempo restante estimado.

---

# Arquitetura

```
TrainingService

↓

TrainingSession

↓

TrainingItem

↓

ChargeSystem

↓

SkillProgression

↓

Character
```

---

# Integrações

TrainingService comunica com:

- CharacterService;
- SkillService;
- AccountService;
- ItemService.

---

# Pendências futuras

Definir:

- fórmula de progresso;
- consumo de cargas;
- tipos de Training Items;
- diferenças Free/Premium;
- interface visual;
- balanceamento.