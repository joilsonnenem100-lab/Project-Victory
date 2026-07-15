# Combat Orchestrator

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Combat Orchestrator.

---

# Responsabilidade

O Combat Orchestrator é responsável exclusivamente por coordenar a execução de um combate.

---

# Responsabilidades

- iniciar Combates;
- finalizar Combates;
- controlar o ciclo de vida;
- coordenar participantes;
- coordenar módulos do combate;
- consolidar Combat Result.

---

# Dependências

Combat Orchestrator poderá depender apenas de:

- Combat Service;
- Combat Context;
- Combat Instance;
- Combat Result;
- Combat Participant;
- Engine Events.

---

# Proibições

Combat Orchestrator nunca poderá:

- calcular dano;
- executar Skills;
- aplicar Loot;
- conceder Experience;
- controlar IA;
- implementar regras de Status Effects.

---

# Combat Participant

Toda entidade combatente implementa o contrato Combat Participant.

O Combat Orchestrator nunca conhece tipos concretos.

---

# Combat Instance

Cada combate é representado por uma Combat Instance.

Campos mínimos:

- CombatInstanceId;
- CurrentState;
- StartedAt;
- FinishedAt.

---

# Combat States

Todo combate percorre:

- Created;
- Preparing;
- Running;
- Finishing;
- Finished.

---

# Combat Context

Toda execução utiliza um Combat Context imutável.

---

# Combat Result

Toda execução produz um Combat Result.

O Combat Result é um DTO temporário.

---

# Turn Based

O combate do Project-Victory é baseado em turnos.

Cada turno possui:

- Turn Context;
- Turn Result.

---

# Eventos

Todo acontecimento relevante publica Engine Events.

---

# Filosofia

Combat coordena.

Módulos especializados executam.

---

# Fluxo

Combat Context

↓

Combat Orchestrator

↓

Turn Pipeline

↓

Combat Result

↓

Engine Events

---

# Referências

Ver também:

- hunt.md
- events.md