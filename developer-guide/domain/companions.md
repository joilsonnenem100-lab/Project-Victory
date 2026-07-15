# Companions

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Companions.

---

# Responsabilidade

O módulo Companions é responsável exclusivamente pelo gerenciamento dos Companions pertencentes ao Character.

---

# Responsabilidades

- gerenciar Companion Instances;
- controlar Active Companion Slots;
- validar capacidade da coleção;
- ativar e desativar Companions;
- fornecer acesso aos Companions ativos.

---

# Dependências

Companions poderá depender apenas de:

- Companion Definitions;
- Companion Service;
- Character.

---

# Proibições

Companions nunca poderá:

- conhecer Combat;
- conhecer Loot;
- conhecer Craft;
- conhecer Inventory;
- duplicar dados da Companion Definition.

---

# Companion Definition

Toda Companion Definition representa uma Definition versionada.

---

# Companion Instance

Cada Companion Instance possui:

- CompanionInstanceId;
- CompanionDefinitionId;
- Progression State.

---

# Active Companion Slots

Cada Character possui exatamente dois Active Companion Slots.

Os Slots pertencem ao Character.

---

# Companion Collection

Cada Character mantém sua própria coleção de Companion Instances.

A coleção possui uma Capacity Policy.

---

# Companion Service

Toda operação envolvendo Companions ocorre através do Companion Service.

---

# Aquisição

Companions são obtidos exclusivamente através de NPCs.

---

# Troca

Companions ativos podem ser alterados apenas fora da Hunt.

---

# Estado

Companions nunca entram em estado de morte.

---

# Progressão

Cada Companion Instance possui progressão própria.

A mecânica será definida em módulo específico.

---

# Filosofia

Companion Definitions definem.

Companion Instances evoluem.

Character mantém os Active Slots.

Companion Service orquestra as operações.

---

# Fluxo

NPC

↓

Companion Service

↓

Companion Collection

↓

Active Companion Slots

↓

Character

---

# Referências

Ver também:

- character.md
- inventory.md