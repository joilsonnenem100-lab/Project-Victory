# Loot

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Loot.

---

# Responsabilidade

O módulo Loot é responsável exclusivamente por determinar quais Rewards serão gerados por um evento.

Loot nunca entrega Rewards.

Loot apenas os produz.

---

# Responsabilidades

- resolver Loot Tables;
- resolver Loot Context;
- compor múltiplas Loot Tables;
- gerar Reward Collections;
- utilizar Quantity Curves;
- utilizar Random Service.

---

# Dependências

Loot poderá depender apenas de:

- Loot Definitions;
- Loot Context;
- Quantity Curves;
- Random Service.

---

# Proibições

Loot nunca poderá:

- conhecer Character;
- conhecer Inventory;
- conhecer Equipment;
- executar Reward Pipeline;
- entregar Rewards.

---

# Loot Tables

Toda Loot Table representa uma Definition versionada.

A Engine apenas interpreta sua configuração.

---

# Reward Collection

Toda resolução produz obrigatoriamente uma Reward Collection.

Mesmo que exista apenas uma recompensa.

---

# Loot Context

Todas as informações necessárias são recebidas através do Loot Context.

Loot nunca consulta outros módulos diretamente.

---

# Quantity Curves

Quantidade das recompensas poderá utilizar Quantity Curves reutilizáveis.

---

# Random Service

Toda aleatoriedade pertence ao Random Service.

---

# Filosofia

Loot produz Rewards.

Reward Pipeline distribui Rewards.

---

# Fluxo

Loot Definition

↓

Loot Context

↓

Loot Resolver

↓

Reward Collection

↓

Reward Pipeline

---

# Referências

Ver também:

- inventory.md
- reward-pipeline.md
- combat.md