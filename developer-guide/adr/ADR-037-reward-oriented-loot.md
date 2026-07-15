# ADR-037 - Reward-Oriented Loot

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário desacoplar geração de recompensas de sua entrega.

---

# Decisão

Loot gera exclusivamente Reward Collections.

Toda entrega pertence ao Reward Pipeline.

---

# Consequências

Benefícios:

- reutilização;
- baixo acoplamento;
- suporte a qualquer origem de recompensa;
- excelente escalabilidade.

Custos:

- necessidade do Reward Pipeline.

---

# Aplicações

Aplica-se a:

- Monsters;
- Bosses;
- Quests;
- Events;
- Achievements;
- Rewards administrativos.

---

# Restrições

Loot nunca entrega Rewards.

---

# Referências

Ver também:

- developer-guide/domain/loot.md