# ADR-054 - NPC Spawn Integration

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário evitar duplicação da infraestrutura de entidades existentes no mundo.

---

# Decisão

NPC Instances reutilizam integralmente a infraestrutura do módulo Spawn.

O módulo Spawn controla apenas o ciclo de vida físico.

O módulo NPC controla exclusivamente a interação.

---

# Consequências

Benefícios:

- reutilização;
- baixo acoplamento;
- eliminação de duplicidade;
- escalabilidade.

---

# Restrições

O módulo Spawn nunca implementa comportamento específico de NPC.