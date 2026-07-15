# ADR-039 - Atomic Craft Transactions

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário garantir integridade durante transformações de Resources.

---

# Decisão

Toda operação de Craft será executada como uma transação atômica.

---

# Consequências

Benefícios:

- consistência;
- rollback completo;
- prevenção de exploits;
- simplicidade.

Custos:

- necessidade de transações.

---

# Aplicações

Aplica-se a:

- Craft;
- futuras transformações de Resources.

---

# Restrições

Nunca existirão consumos parciais.

---

# Referências

Ver também:

- developer-guide/domain/craft.md