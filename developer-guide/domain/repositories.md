# Repository Contracts

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir os contratos de persistência do domínio.

Repositories representam a fronteira entre o domínio e a persistência.

---

# Filosofia

O domínio nunca conhece banco de dados.

O domínio conhece apenas interfaces.

As implementações pertencem à camada Infrastructure.

---

# Aggregate Roots Persistidos

Somente Aggregate Roots possuem Repository próprio.

No MVP:

- Account
- Character
- World

---

# Regras

É proibido criar Repository para:

- Value Objects
- Objetos internos do Aggregate
- Domain Services

---

# Responsabilidades

Todo Repository deve:

- recuperar Aggregates;
- persistir Aggregates;
- remover Aggregates.

Nada além disso.

---

# Proibições

Repositories nunca:

- executam regras de negócio;
- conhecem Runtime;
- conhecem UI;
- conhecem Application.

---

# Filosofia Final

Repositories existem apenas para preservar o estado do domínio.

Nunca para implementar regras de negócio.