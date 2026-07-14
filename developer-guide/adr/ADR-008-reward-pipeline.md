# ADR-008 - Reward Pipeline

Status: ACCEPTED

Version: 1.0

---

# Contexto

Durante a definição da arquitetura de recompensas do Project-Victory foi identificado que experiência, loot e futuros sistemas de recompensa possuem responsabilidades distintas.

Centralizar toda a lógica em um único módulo aumentaria o acoplamento e dificultaria a evolução da Engine.

---

# Decisão

A Engine utilizará um Reward Pipeline.

Cada etapa possui uma responsabilidade única.

Fluxo oficial:

Monster Defeated

↓

Reward Calculation

↓

Bonus Pipeline

↓

Experience Distribution

↓

Loot Distribution

↓

Persistence

↓

Interface Notification

---

# Consequências

Benefícios:

- separação de responsabilidades;
- baixo acoplamento;
- escalabilidade;
- facilidade para novos tipos de recompensa;
- facilidade para testes.

Custos:

- necessidade de manter a ordem do pipeline.

---

# Aplicações

Este princípio aplica-se a:

- Experience Distribution;
- Loot Distribution;
- Reward Calculation;
- Bonus Pipeline;
- Combat Engine.

---

# Restrições

Nenhum módulo poderá assumir responsabilidades pertencentes a outro estágio do pipeline.

Toda recompensa deverá percorrer o Reward Pipeline antes de ser persistida.

---

# Referências

Ver também:

- developer-guide/domain/experience-distribution.md
- developer-guide/domain/loot.md
- developer-guide/domain/combat.md