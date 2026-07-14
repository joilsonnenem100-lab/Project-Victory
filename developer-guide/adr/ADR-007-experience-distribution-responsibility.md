# ADR-007 - Responsabilidade do Experience Distribution

Status: ACCEPTED

Version: 1.0

---

# Contexto

Durante a definição da arquitetura do sistema de experiência foi identificado que cálculo, bonificações e distribuição representam responsabilidades distintas.

Concentrar essas responsabilidades em um único módulo aumentaria o acoplamento da Engine e dificultaria futuras expansões.

---

# Decisão

O módulo Experience Distribution será responsável exclusivamente pela distribuição da experiência.

O cálculo da recompensa pertence ao Reward Calculation.

A aplicação de bônus pertence ao Bonus Pipeline.

Experience Distribution receberá apenas o valor final da recompensa.

---

# Consequências

Benefícios:

- separação clara de responsabilidades;
- baixo acoplamento;
- facilidade para testes;
- escalabilidade;
- facilidade para adicionar novos sistemas de bônus.

Custos:

- necessidade de um pipeline de recompensas antes da distribuição.

---

# Aplicações

Este princípio aplica-se a:

- Character;
- Companion;
- Experience Distribution;
- Reward Calculation;
- Bonus Pipeline.

---

# Restrições

Experience Distribution nunca poderá:

- calcular experiência;
- aplicar bônus;
- conhecer Premium;
- conhecer Eventos;
- conhecer Equipamentos;
- conhecer Boosts.

---

# Referências

Ver também:

- developer-guide/domain/experience-distribution.md
- developer-guide/domain/combat.md