# ADR-032 - Composable Effects Architecture

Status: ACCEPTED

Version: 1.0

---

# Contexto

Diversas Skills compartilham comportamentos semelhantes.

Era necessário eliminar duplicação de lógica.

---

# Decisão

Skills deixam de executar lógica diretamente.

Cada Skill passa a representar um orquestrador de Effects.

Effects tornam-se componentes independentes e reutilizáveis.

---

# Consequências

Benefícios:

- reutilização;
- modularidade;
- baixo acoplamento;
- facilidade para criação de novas Skills;
- balanceamento simplificado.

Custos:

- necessidade de um Pipeline de Effects.

---

# Aplicações

Este princípio aplica-se a:

- Skills;
- Combat;
- Buffs;
- Debuffs;
- Effects futuros.

---

# Restrições

Skills nunca poderão conter lógica específica de execução.

Toda execução pertence exclusivamente aos Effects.

---

# Referências

Ver também:

- developer-guide/domain/skills.md
- developer-guide/domain/combat.md