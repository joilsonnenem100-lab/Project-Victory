# Experience Distribution

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Experience Distribution.

---

# Responsabilidade

O módulo Experience Distribution é responsável exclusivamente por distribuir a experiência aos participantes elegíveis do combate.

O cálculo da recompensa não pertence a este módulo.

---

# Responsabilidades

O módulo Experience Distribution é responsável por:

- receber a recompensa de experiência;
- identificar participantes elegíveis;
- distribuir a experiência;
- garantir consistência da distribuição.

---

# Dependências

O módulo Experience Distribution poderá depender apenas de:

- Combat Participant;
- Experience Reward;
- Combat Context.

---

# Proibições

O módulo Experience Distribution não poderá:

- calcular experiência;
- aplicar bônus;
- conhecer Monsters;
- conhecer Loot;
- conhecer UI;
- acessar banco de dados;
- controlar Hunts.

---

# Regras

A experiência é distribuída imediatamente após a derrota do Monster.

A experiência já distribuída nunca poderá ser removida.

Cada Combat Participant elegível recebe sua própria recompensa.

Character recebe experiência.

Companion recebe experiência própria.

Summons não recebem experiência.

NPCs aliados não recebem experiência.

O módulo trabalha apenas com o valor final recebido da Engine.

A distribuição é atômica.

---

# Filosofia

Experience Distribution distribui recompensas.

Reward Calculation calcula recompensas.

Bonus Pipeline aplica modificadores.

Cada módulo possui apenas uma responsabilidade.

---

# Fluxo

Monster Defeated

↓

Reward Calculation

↓

Bonus Pipeline

↓

Experience Distribution

↓

Persistence

↓

Interface Notification

---

# Referências

Ver também:

- combat.md
- monsters.md
- monster-ai.md
- experience.md

---

# Implementação atual (`PV-XP-010`, Fila 6)

> Status do documento continua DRAFT: a regra de grupo é decisão do PO.

> *Nota de superação (2026-11-07, `DEC-20261107-02`, PO-07 em `docs/product/PO-DECISOES-BASE.md`): **sem grupo até a Fase 9**: a Hunt é solo; a regra de XP em grupo (R-045) fica adiada para quando houver conteúdo de Party.*

- `IExperienceDistributor.Distribute(finalExperience, participants)` (`src/ProjectVictory.Application/Reward/ExperienceDistribution.cs`) recebe só a Experience **final** (já com XP rate) e devolve as partes (`ExperienceShare`); não altera Experience.
- O Reward Pipeline (`RewardResolutionHandler`) aplica cada parte pelo `ApplyResolvedRewardToCharacterHandler` (ADR-024).
- Regra **PROVISÓRIA** (`EvenSplitExperienceDistributor`): divisão igual; o resto vai 1 a 1 aos primeiros participantes; com 1 participante (hoje), recebe tudo.
- Pendência: atomicidade com vários participantes (R-045).
