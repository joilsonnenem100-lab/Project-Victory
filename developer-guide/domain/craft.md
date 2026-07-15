# Craft

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Craft.

---

# Responsabilidade

O módulo Craft é responsável exclusivamente por transformar Resources existentes em um novo Resource através de uma Craft Recipe.

---

# Responsabilidades

- resolver Craft Recipes;
- validar Requirements;
- validar Inputs;
- consumir Resources;
- gerar Reward Collection;
- utilizar o Reward Pipeline.

---

# Dependências

Craft poderá depender apenas de:

- Craft Recipe Definitions;
- Resource Definitions;
- Craft Context;
- Reward Pipeline;
- Inventory Service.

---

# Proibições

Craft nunca poderá:

- criar novos tipos de Resources;
- entregar Rewards diretamente;
- modificar Resources sem transação;
- consultar módulos diretamente.

---

# Craft Recipe

Toda Recipe representa uma Definition versionada.

Cada Recipe possui:

- RecipeId;
- Version;
- Inputs;
- Output;
- Requirements;
- Batch Configuration;
- Metadata.

---

# Inputs

Os Inputs representam Resources necessários para execução.

Cada Input define sua própria política:

- Consumir;
- Preservar.

---

# Output

Toda Recipe produz exatamente um único tipo de Resource.

A quantidade produzida pertence à Recipe.

---

# Batch

Cada Recipe define:

- se suporta Batch;
- quantidade máxima por execução.

---

# Inventory

Os Inputs poderão ser consumidos de:

- Character Inventory;
- Account Depot.

Prioridade:

1. Character Inventory
2. Account Depot

---

# Output Destination

Todo Output é entregue exclusivamente ao Character Inventory.

---

# Transactions

Toda execução ocorre através de uma única transação atômica.

---

# Reward Pipeline

Toda entrega utiliza obrigatoriamente o Reward Pipeline.

---

# Craft Context

Toda resolução utiliza exclusivamente o Craft Context.

---

# History

O Character mantém um histórico limitado de Crafts executados contendo:

- RecipeId;
- Data/Hora;
- Quantidade produzida.

---

# Filosofia

Craft transforma Resources.

Nunca cria novos tipos de Resources.

---

# Fluxo

Recipe Definition

↓

Craft Context

↓

Validate

↓

Transaction

↓

Reward Collection

↓

Reward Pipeline

↓

Character Inventory

---

# Referências

Ver também:

- inventory.md
- loot.md
- resources.md