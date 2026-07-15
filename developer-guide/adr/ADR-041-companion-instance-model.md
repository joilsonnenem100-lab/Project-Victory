# ADR-041 - Companion Instance Model

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário separar dados estáticos dos dados pertencentes ao Character.

---

# Decisão

Companions são representados por:

- Companion Definition;
- Companion Instance.

A Definition contém os dados estáticos.

A Instance contém apenas o estado individual.

---

# Consequências

Benefícios:

- baixo acoplamento;
- reutilização;
- evolução independente;
- menor consumo de armazenamento.

---

# Aplicações

Aplica-se a todos os Companions.

---

# Restrições

Companion Instances nunca duplicam dados da Definition.

---

# Referências

Ver também:

- developer-guide/domain/companions.md