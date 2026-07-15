# ADR-052 - Turn Based Combat

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário definir o modelo de execução do combate.

---

# Decisão

O Project-Victory utilizará combate baseado em turnos.

O Combat Orchestrator coordena a sequência de turnos.

---

# Consequências

Benefícios:

- execução determinística;
- facilidade de testes;
- previsibilidade;
- menor complexidade.

---

# Restrições

Cada turno possui exatamente um participante ativo.