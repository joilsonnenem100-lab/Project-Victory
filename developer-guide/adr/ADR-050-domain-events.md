# ADR-050 - Domain Events

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário separar eventos de domínio dos eventos técnicos da infraestrutura.

---

# Decisão

O Event Bus transporta exclusivamente Domain Events.

Eventos técnicos utilizam infraestrutura própria.

---

# Consequências

Benefícios:

- separação de responsabilidades;
- menor acoplamento;
- maior clareza dos contratos;
- evolução independente da infraestrutura.

---

# Restrições

Eventos de infraestrutura nunca trafegam pelo Event Bus do domínio.

---

# Referências

Ver também:

- developer-guide/domain/events.md