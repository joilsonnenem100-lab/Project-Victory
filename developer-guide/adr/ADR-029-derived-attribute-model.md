# ADR-029 - Derived Attribute Model

Status: ACCEPTED

Version: 1.0

---

# Contexto

A progressão do Character é baseada em Experience permanente e curvas configuráveis.

Era necessário definir como os atributos seriam obtidos.

---

# Decisão

Os Base Attributes passam a representar um estado derivado.

Eles são calculados utilizando:

- Class;
- Experience;
- Ascension;
- Attribute Curve.

Os Base Attributes deixam de ser armazenados como valores editáveis.

---

# Consequências

Benefícios:

- balanceamento simplificado;
- ausência de distribuição manual;
- evolução determinística;
- compatibilidade com Ascensions;
- alta previsibilidade.

Custos:

- necessidade das Attribute Curves.

---

# Aplicações

Este princípio aplica-se a:

- Character;
- Experience;
- Ascension;
- Combat.

---

# Restrições

Os Base Attributes nunca poderão:

- ser editados manualmente;
- depender do Combat;
- depender de Equipment.

---

# Referências

Ver também:

- developer-guide/domain/attributes.md
- developer-guide/domain/experience.md