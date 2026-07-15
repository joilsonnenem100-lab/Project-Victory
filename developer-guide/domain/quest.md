# Quest

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Quest.

---

# Responsabilidade

O módulo Quest coordena exclusivamente a evolução das missões dos Characters.

---

# Responsabilidades

- iniciar Quests;
- atualizar progresso;
- concluir Quests;
- coordenar Completion Policies;
- publicar Domain Events.

---

# Dependências

Quest poderá depender apenas de:

- Quest Definitions;
- Quest Service;
- Quest Context;
- Quest Result;
- Engine Events;
- Reward Pipeline.

---

# Proibições

Quest nunca implementa:

- Combat;
- Hunt;
- Inventory;
- Craft;
- Loot;
- Pets;
- Companions;
- NPC.

---

# Quest Definition

Toda Quest é definida por uma Quest Definition versionada.

Definitions são imutáveis.

---

# Quest Objectives

Cada Quest Definition possui uma coleção de Quest Objectives.

Cada objetivo possui progresso independente.

---

# Completion Policy

A conclusão da Quest pertence exclusivamente à Completion Policy.

---

# Quest Instance

Cada Character possui sua própria Quest Instance.

Quest Instances são persistidas.

---

# Quest Context

Toda atualização utiliza um Quest Context imutável.

---

# Quest Result

Quest Result é um DTO temporário.

---

# Event Driven

Quest reage exclusivamente a Domain Events.

Nunca consulta diretamente outros módulos.

---

# Reward Pipeline

Após a conclusão, as recompensas são entregues pelo Reward Pipeline da Engine.

---

# Filosofia

Events notificam.

Quest avalia.

Policies decidem.

Reward Pipeline entrega.

---

# Fluxo

Domain Event

↓

Quest Service

↓

Objective Evaluators

↓

Completion Policy

↓

Quest Result

↓

Reward Pipeline

↓

Engine Events