# ADR-036 - Centralized Inventory Service

Status: ACCEPTED

Version: 1.0

---

# Contexto

Diversos módulos movimentam Resources.

Era necessário evitar duplicação de lógica.

---

# Decisão

Toda movimentação passa obrigatoriamente pelo Inventory Service.

---

# Consequências

Benefícios:

- validação única;
- auditoria centralizada;
- consistência;
- reutilização.

Custos:

- necessidade de um serviço central.

---

# Aplicações

Aplica-se a:

- Loot;
- Craft;
- Marketplace;
- Trade;
- Depot;
- Rewards.

---

# Restrições

Nenhum Aggregate poderá movimentar Resources diretamente.

---

# Referências

Ver também:

- developer-guide/domain/inventory.md