# ADR-035 - Resource-Oriented Inventory

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário impedir que o Inventory conhecesse regras específicas de cada tipo de item.

---

# Decisão

O Inventory passa a gerenciar apenas Resources.

Todo comportamento pertence ao módulo consumidor.

---

# Consequências

Benefícios:

- baixo acoplamento;
- alta reutilização;
- escalabilidade;
- simplicidade.

Custos:

- necessidade de interpretações externas.

---

# Aplicações

Aplica-se a:

- Equipment;
- Craft;
- Loot;
- Marketplace;
- Trade;
- Quests.

---

# Restrições

O Inventory nunca poderá conhecer regras específicas de um tipo de Resource.

---

# Referências

Ver também:

- developer-guide/domain/inventory.md