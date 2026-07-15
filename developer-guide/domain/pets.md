# Pets

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Pets.

---

# Responsabilidade

O módulo Pets é responsável exclusivamente pelo gerenciamento dos Pets pertencentes ao Character.

---

# Responsabilidades

- gerenciar Pet Instances;
- controlar o Active Pet Slot;
- validar capacidade da coleção;
- ativar e desativar Pets;
- fornecer acesso ao Pet ativo.

---

# Dependências

Pets poderá depender apenas de:

- Pet Definitions;
- Pet Service;
- Character.

---

# Proibições

Pets nunca poderá:

- conhecer Combat;
- conhecer Loot;
- conhecer Craft;
- conhecer Equipment;
- conhecer Companions;
- duplicar dados da Pet Definition.

---

# Pet Definition

Toda Pet Definition representa uma Definition versionada.

---

# Pet Instance

Cada Pet Instance possui:

- PetInstanceId;
- PetDefinitionId;
- Progression State.

---

# Active Pet Slot

Cada Character possui exatamente um Active Pet Slot.

O Slot pertence ao Character.

---

# Pet Collection

Cada Character mantém sua própria coleção de Pet Instances.

A coleção possui uma Capacity Policy.

---

# Pet Service

Toda operação envolvendo Pets ocorre através do Pet Service.

---

# Aquisição

Pets são obtidos exclusivamente através de NPCs.

---

# Troca

O Character poderá alterar seu Pet ativo apenas fora da Hunt.

---

# Estado

Pets não possuem histórico próprio.

---

# Capacidades

Uma Pet Definition poderá declarar capacidades adicionais.

Sempre que possível essas capacidades reutilizam módulos existentes da Engine.

Exemplo:

- Inventory.

---

# Progressão

Cada Pet Instance possui progressão própria.

A mecânica será definida em módulo específico.

---

# Filosofia

Pet Definitions definem.

Pet Instances evoluem.

Character mantém o Active Pet Slot.

Pet Service orquestra as operações.

---

# Fluxo

NPC

↓

Pet Service

↓

Pet Collection

↓

Active Pet Slot

↓

Character

---

# Referências

Ver também:

- character.md
- inventory.md