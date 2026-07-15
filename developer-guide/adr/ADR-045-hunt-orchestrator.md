# ADR-045 - Hunt Orchestrator

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário definir um componente responsável por coordenar a execução de uma Hunt sem concentrar regras de outros domínios.

---

# Decisão

A Hunt será implementada como um Orchestrator.

Toda regra de Combat, Loot, Experience, Skills, Pets e Companions permanece em seus respectivos módulos.

---

# Consequências

Benefícios:

- baixo acoplamento;
- alta coesão;
- facilidade de testes;
- evolução independente dos módulos.

---

# Restrições

O Hunt Orchestrator nunca implementa regras pertencentes a outros domínios.