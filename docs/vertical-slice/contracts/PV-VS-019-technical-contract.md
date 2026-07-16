# PV-VS-019 — Technical Contract

## Nome

Executar Hunt com Estado Válido

---

# Objetivo Técnico

Implementar a capacidade de executar uma Hunt previamente criada somente quando ela estiver em um estado permitido pela arquitetura.

A execução deve utilizar o fluxo existente de Hunt, Combat, Reward e Progression sem criar novos sistemas.

---

# Escopo Autorizado

Implementar exclusivamente:

- validação do estado atual da Hunt antes da execução;
- início da execução somente para Hunts em estado permitido;
- atualização correta do estado da Hunt durante o fluxo;
- integração com o fluxo existente de execução.

---

# Fluxo Técnico Esperado
