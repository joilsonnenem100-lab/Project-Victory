# ADR-051 - Combat Orchestrator

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário separar a coordenação do combate das regras de domínio.

---

# Decisão

O combate será coordenado pelo Combat Orchestrator.

Toda regra especializada permanece em módulos próprios.

---

# Consequências

Benefícios:

- baixo acoplamento;
- alta coesão;
- facilidade de testes;
- evolução independente.

---

# Restrições

O Combat Orchestrator nunca implementa regras de dano, Skills, IA, Loot ou Experience.