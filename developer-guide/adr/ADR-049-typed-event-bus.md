# ADR-049 - Typed Event Bus

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário definir um mecanismo seguro de comunicação entre módulos.

---

# Decisão

A Engine utilizará um Event Bus fortemente tipado.

Todos os Events derivam de EngineEvent.

---

# Consequências

Benefícios:

- type safety;
- baixo acoplamento;
- contratos explícitos;
- refatorações seguras;
- facilidade de testes.

---

# Restrições

O Event Bus nunca utiliza nomes em texto como mecanismo principal de despacho.

---

# Referências

Ver também:

- developer-guide/domain/events.md