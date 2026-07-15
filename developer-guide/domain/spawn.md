# Spawn

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Spawn.

---

# Responsabilidade

O módulo Spawn é responsável exclusivamente pelo ciclo de vida das Spawn Instances.

---

# Responsabilidades

- criar Spawn Instances;
- ativar Spawn Instances;
- desativar Spawn Instances;
- remover Spawn Instances;
- coordenar o Spawn Orchestrator.

---

# Dependências

Spawn poderá depender apenas de:

- Spawn Definitions;
- Spawn Service;
- Spawn Context;
- Respawn Policy;
- Engine Events.

---

# Proibições

Spawn nunca poderá:

- executar Combat;
- executar IA;
- gerar Loot;
- conceder Experience;
- implementar regras de Respawn;
- conhecer regras de Quest.

---

# Spawn Definition

Toda Spawn Definition representa uma configuração versionada.

Cada Definition representa uma única entidade.

---

# Spawn Instance

Cada Spawn Instance possui:

- SpawnInstanceId;
- SpawnDefinitionId;
- CurrentState;
- SpawnedAt;
- DespawnedAt.

---

# Spawn States

Toda Spawn Instance percorre:

- Created;
- Spawning;
- Active;
- Despawning;
- Despawned.

---

# Respawn Policy

A decisão de criar uma nova Spawn Instance pertence exclusivamente à Respawn Policy.

O módulo Spawn apenas executa essa decisão.

---

# Spawn Context

Toda operação utiliza um Spawn Context imutável.

---

# Spawn Result

Toda operação produz um Spawn Result temporário.

O Spawn Result é um DTO.

---

# Persistência

Spawn Instances não são persistidas.

Elas existem apenas durante sua execução.

---

# Eventos

Toda alteração relevante publica Engine Events.

---

# Filosofia

Definitions definem.

Policies decidem.

Spawn executa.

---

# Fluxo

Spawn Definition

↓

Respawn Policy

↓

Spawn Service

↓

Spawn Orchestrator

↓

Spawn Instance

↓

Engine Events

---

# Referências

Ver também:

- hunt.md
- combat.md