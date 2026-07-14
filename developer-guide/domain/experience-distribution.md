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