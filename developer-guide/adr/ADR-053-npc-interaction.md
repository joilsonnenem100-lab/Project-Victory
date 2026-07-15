# ADR-053 - NPC Interaction

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário separar a infraestrutura de NPC das regras de negócio dos demais módulos.

---

# Decisão

O módulo NPC coordena apenas interações.

Toda regra permanece em serviços especializados.

---

# Consequências

Benefícios:

- baixo acoplamento;
- reutilização;
- facilidade de testes;
- simplicidade.

---

# Restrições

NPC nunca implementa regras de domínio.