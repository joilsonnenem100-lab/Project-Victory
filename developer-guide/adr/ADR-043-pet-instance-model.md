# ADR-043 - Pet Instance Model

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário separar os dados estáticos dos dados pertencentes ao Character.

---

# Decisão

Pets são representados por:

- Pet Definition;
- Pet Instance.

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

Aplica-se a todos os Pets.

---

# Restrições

Pet Instances nunca duplicam dados da Definition.

---

# Referências

Ver também:

- developer-guide/domain/pets.md