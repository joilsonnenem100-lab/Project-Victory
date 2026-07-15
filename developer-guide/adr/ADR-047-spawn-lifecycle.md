# ADR-047 - Spawn Lifecycle

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário definir um ciclo de vida uniforme para entidades temporárias do mundo.

---

# Decisão

Toda Spawn Instance percorre uma máquina de estados explícita.

A Spawn Instance é efêmera e nunca é persistida.

---

# Consequências

Benefícios:

- baixo consumo de armazenamento;
- previsibilidade;
- simplicidade;
- alta escalabilidade.

---

# Restrições

Spawn Instances nunca armazenam regras de domínio.

---

# Referências

Ver também:

- developer-guide/domain/spawn.md