# Hunt

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Hunt.

---

# Responsabilidade

O módulo Hunt é responsável exclusivamente por orquestrar o ciclo de vida de uma Hunt.

Não implementa regras de Combat, Loot, Experience, Inventory ou Progressão.

---

# Responsabilidades

- iniciar Hunts;
- finalizar Hunts;
- controlar o ciclo de vida;
- coordenar os módulos participantes;
- consolidar o Hunt Result.

---

# Dependências

O módulo Hunt poderá depender apenas de:

- Hunt Service;
- Hunt Context;
- Hunt Instance;
- Hunt Result;
- Engine Events.

---

# Proibições

O módulo Hunt nunca poderá:

- executar Combat;
- gerar Loot;
- conceder Experience;
- modificar Inventory;
- implementar regras de Skills;
- implementar regras de Companions;
- implementar regras de Pets.

Toda lógica pertence aos módulos especializados.

---

# Hunt Instance

Cada execução de Hunt é representada por uma Hunt Instance.

Campos mínimos:

- HuntInstanceId;
- CharacterId;
- CurrentState;
- StartedAt;
- FinishedAt.

---

# Hunt States

Toda Hunt percorre obrigatoriamente os estados:

- Created;
- Preparing;
- Running;
- Finishing;
- Finished.

As transições são controladas exclusivamente pelo Hunt Orchestrator.

---

# Hunt Context

Toda execução utiliza um Hunt Context.

O Hunt Orchestrator nunca consulta diretamente outros módulos.

---

# Hunt Result

O Hunt Result representa a consolidação temporária dos resultados produzidos durante a Hunt.

É um DTO sem comportamento de domínio.

Cada módulo contribui apenas com suas próprias informações.

---

# Hunt Service

Toda operação relacionada à Hunt ocorre através do Hunt Service.

Responsabilidades:

- iniciar Hunt;
- finalizar Hunt;
- consultar Hunt ativa;
- validar início da Hunt;
- coordenar o Hunt Orchestrator.

---

# Character

O Character mantém apenas:

- CurrentHuntInstanceId.

Essa referência é criada e removida exclusivamente pelo Hunt Service.

---

# Persistência

Toda Hunt Instance é persistida ao término.

O histórico utiliza uma Capacity Policy configurável.

---

# Eventos

A Hunt publica Engine Events.

Eventos nunca alteram diretamente o estado da Hunt.

O Hunt Orchestrator é o único responsável pelo encerramento da execução.

---

# Filosofia

A Hunt coordena.

Os demais módulos executam.

---

# Fluxo

Character

↓

Hunt Service

↓

Hunt Context

↓

Hunt Orchestrator

↓

Hunt Result

↓

Finished

↓

Persistência

---

# Referências

Ver também:

- combat.md
- loot.md
- inventory.md
- experience.md