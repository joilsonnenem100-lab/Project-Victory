# NPC

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo NPC.

---

# Responsabilidade

O módulo NPC coordena exclusivamente a interação entre Characters e os serviços da Engine.

---

# Responsabilidades

- iniciar interações;
- finalizar interações;
- coordenar Interaction Handlers;
- publicar Domain Events.

---

# Dependências

NPC poderá depender apenas de:

- NPC Definition;
- NPC Service;
- NPC Context;
- NPC Result;
- Spawn;
- Engine Events.

---

# Proibições

NPC nunca poderá implementar:

- Craft;
- Shop;
- Inventory;
- Pets;
- Companions;
- Quest;
- Banco;
- Loot.

---

# NPC Definition

Toda NPC Definition representa um único tipo de NPC.

Definitions são:

- versionadas;
- imutáveis;
- carregadas durante o startup da Engine.

---

# NPC Instance

Toda NPC Instance:

- referencia exatamente uma NPC Definition;
- possui localização própria;
- reutiliza o ciclo de vida do módulo Spawn.

---

# NPC Interaction

Cada Character possui no máximo uma interação ativa.

Um NPC pode atender inúmeros Characters simultaneamente.

---

# Interaction Handler

Cada NPC utiliza exatamente um Interaction Handler.

Toda regra pertence ao Handler.

---

# Spawn

O módulo NPC reutiliza a infraestrutura do módulo Spawn para gerenciamento do ciclo de vida físico das NPC Instances.

---

# Filosofia

Definition define.

Spawn controla existência.

Handler executa.

Service orquestra.

---

# Fluxo

Character

↓

NPC Service

↓

Interaction Handler

↓

Target Service

↓

NPC Result

↓

Engine Events

---

# Referências

Ver também:

- spawn.md
- events.md
- hunt.md