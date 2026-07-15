# ADR-042 - Companion Service

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário centralizar todas as regras relacionadas aos Companions.

---

# Decisão

Toda operação envolvendo Companions será executada através do Companion Service.

---

# Responsabilidades

- adquirir Companion;
- remover Companion;
- ativar Companion;
- desativar Companion;
- validar Active Slots;
- validar Capacity Policy.

---

# Consequências

Benefícios:

- centralização das regras;
- menor acoplamento;
- Character mais enxuto;
- facilidade de testes.

---

# Restrições

O Companion Service nunca persiste os Active Slots.

Os Active Slots pertencem ao Character.

---

# Referências

Ver também:

- developer-guide/domain/companions.md