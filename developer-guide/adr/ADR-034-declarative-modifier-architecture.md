# ADR-034 - Declarative Modifier Architecture

Status: ACCEPTED

Version: 1.0

---

# Contexto

Equipamentos compartilham diversos comportamentos semelhantes.

Era necessário evitar duplicação de lógica.

---

# Decisão

Equipamentos passam a declarar apenas Modifiers.

Toda interpretação pertence à Engine.

---

# Consequências

Benefícios:

- reutilização;
- modularidade;
- baixo acoplamento;
- facilidade de balanceamento;
- facilidade para novos equipamentos.

Custos:

- necessidade do Modifier Pipeline.

---

# Aplicações

Este princípio aplica-se a:

- Equipment;
- Combat Formula;
- futuros sistemas de encantamentos;
- imbuements;
- relíquias.

---

# Restrições

Equipamentos nunca poderão implementar lógica própria.

Toda lógica pertence aos Modifiers e à Engine.

---

# Referências

Ver também:

- developer-guide/domain/equipment.md
- developer-guide/domain/attributes.md