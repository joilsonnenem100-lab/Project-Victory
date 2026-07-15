# Inventory

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Inventory.

---

# Responsabilidade

O Inventory é responsável exclusivamente por gerenciar os Resources pertencentes ao Character.

Ele não interpreta Resources.

Ele apenas gerencia sua posse e movimentação.

---

# Responsabilidades

- armazenar Resources;
- armazenar Resource Stacks;
- armazenar Resource Instances;
- controlar capacidade;
- disponibilizar consultas;
- permitir movimentações através do Inventory Service.

---

# Dependências

Inventory poderá depender apenas de:

- Resource Definitions;
- Resource Instances;
- Resource Stacks;
- Capacity Policy.

---

# Proibições

Inventory nunca poderá:

- interpretar Equipment;
- interpretar Craft;
- interpretar Loot;
- interpretar Quest Items;
- executar Combat;
- executar Skills.

---

# Resource-Oriented

Todo elemento armazenado representa um Resource.

Seu comportamento pertence ao módulo consumidor.

---

# Resource Stack

Resources empilháveis utilizam Resource Stack.

Exemplos:

- moedas;
- materiais;
- poções;
- flechas.

---

# Resource Instance

Resources únicos utilizizam Resource Instance.

Exemplos:

- equipamentos;
- relíquias;
- artefatos.

---

# Capacity

O limite do Inventory é determinado por uma Capacity Policy.

Nunca por um valor fixo.

---

# Depot

O Depot pertence à Conta.

Todos os Characters compartilham o mesmo Depot.

O Inventory pertence exclusivamente ao Character.

---

# Views

Categorias, filtros e ordenações pertencem exclusivamente à Interface.

O Inventory representa apenas uma coleção de Resources.

---

# Inventory Service

Toda movimentação ocorre exclusivamente através do Inventory Service.

---

# Filosofia

Inventory representa posse.

Nunca comportamento.

---

# Fluxo

Resource Definition

↓

Resource Stack
ou
Resource Instance

↓

Inventory

↓

Inventory Service

↓

Consumer Modules

---

# Referências

Ver também:

- equipment.md
- loot.md
- craft.md