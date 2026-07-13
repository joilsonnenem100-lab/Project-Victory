---

# DEC-0247

## Título

Arquitetura Event-Driven.

## Origem

DB-001 — Persistence Model

## Status

LOCKED

## Decisão

Os principais acontecimentos do jogo serão propagados através de eventos.

Exemplo:

MonsterDeathEvent

↓

LootService

↓

ExperienceService

↓

CollectionService

↓

AchievementService

↓

QuestService

Cada sistema é responsável apenas pela sua própria lógica.

Nenhum sistema deve conhecer a implementação interna dos demais.

Objetivo:

- baixo acoplamento;
- alta escalabilidade;
- facilidade de expansão;
- manutenção simplificada.
---

# DEC-0255

## Título

Git First Workflow

## Status

LOCKED

## Decisão

Toda Sprint que alterar arquivos do projeto deve finalizar com:

- lista de arquivos alterados;
- indicação de novos arquivos;
- status "Pronto para Commit".

O repositório deve permanecer sincronizado com a documentação.

---

# DEC-0256

## Título

Bounded Context

## Origem

VC-LAB-002

## Status

LOCKED

## Decisão

Cada domínio da aplicação deve ser independente.

A comunicação entre domínios ocorre apenas por:

- interfaces;
- eventos;
- contratos.

Não permitir dependências circulares entre módulos.
---

# DEC-0257

## Título

Account Identity Only

## Origem

SPRING-029

## Status

LOCKED

## Decisão

A entidade Account representa apenas a identidade do jogador.

Nunca armazenar:

- inventário;
- progresso;
- estatísticas;
- economia de jogo.

---

# DEC-0258

## Título

Single Responsibility Database

## Origem

SPRINT-029

## Status

LOCKED

## Decisão

Cada tabela do banco deve possuir apenas uma responsabilidade.

Evitar tabelas grandes contendo múltiplos domínios.
---

# DEC-0259

## Título

Character Component Model

## Origem

SPRINT-030

## Status

LOCKED

## Decisão

Character representa apenas identidade e progressão principal.

Todos os demais sistemas são separados em componentes independentes.

---

# DEC-0260

## Título

Character Component Architecture

## Origem

SPRINT-030

## Status

LOCKED

## Decisão

Todo novo sistema relacionado ao personagem deve ser avaliado como um componente independente antes de ser incorporado à entidade Character.

---

# DEC-0261

## Título

Vertical Growth

## Origem

SPRINT-030

## Status

LOCKED

## Decisão

A arquitetura deve crescer através de novos módulos e novas tabelas.

Evitar crescimento horizontal da tabela Character.
---

# DEC-0262

## Título

Inventory as Container

## Origem

SPRINT-031

## Status

LOCKED

## Decisão

Inventory atua apenas como um Container.

Não armazena itens diretamente.

Os itens são organizados através de InventorySlot.

---

# DEC-0263

## Título

Container Ownership

## Origem

SPRINT-031

## Status

LOCKED

## Decisão

ItemInstance pertence ao Container onde está localizado.

Nunca diretamente ao Character.

---

# DEC-0264

## Título

Unique Item Location

## Origem

SPRINT-031

## Status

LOCKED

## Decisão

Todo ItemInstance deve possuir exatamente uma localização válida.

Nunca pode existir um ItemInstance sem localização.

---

# DEC-0265

## Título

InventorySlot Entity

## Origem

SPRINT-031

## Status

LOCKED

## Decisão

InventorySlot passa a ser uma entidade própria.

Permite evolução futura do sistema de inventário sem alterações estruturais.
---

# DEC-0266

## Título

Immutable Item Identity

## Status

LOCKED

## Decisão

ItemInstance representa uma identidade única e permanente.

A identidade nunca muda durante todo o ciclo de vida do item.

---

# DEC-0267

## Título

Item Metadata Separation

## Status

LOCKED

## Decisão

Separar ItemInstance de ItemMetadata.

ItemInstance representa identidade.

ItemMetadata representa atributos mutáveis.

---

# DEC-0268

## Título

Generic Container Slot

## Status

LOCKED

## Decisão

Todo Container utiliza um tipo derivado de ContainerSlot.

---

# DEC-0269

## Título

Location Owned by Slot

## Status

LOCKED

## Decisão

A localização de um item pertence ao Slot.

Nunca ao ItemInstance.

---

# DEC-0270

## Título

Equipment as Container

## Status

LOCKED

## Decisão

Equipment implementa a arquitetura de Container.

---

# DEC-0271

## Título

Everything is a Container

## Status

LOCKED

## Decisão

Todo sistema que armazena itens implementa a interface IContainer.
---

# DEC-0272

## Título

Template World

## Status

LOCKED

## Decisão

Todo o mundo do jogo é composto por Templates imutáveis.

---

# DEC-0273

## Título

Disposable HuntInstance

## Status

LOCKED

## Decisão

HuntInstance existe apenas durante a execução da Hunt.

---

# DEC-0274

## Título

Reusable PackTemplate

## Status

LOCKED

## Decisão

PackTemplate pode ser reutilizado por múltiplas Hunts.

---

# DEC-0275

## Título

Spawn Owned by Hunt

## Status

LOCKED

## Decisão

MonsterTemplate não possui informação de spawn.

HuntTemplate define quais monstros aparecem.

---

# DEC-0276

## Título

Passive World

## Status

LOCKED

## Decisão

O mundo nunca conhece os jogadores.

Jogadores entram em instâncias temporárias.

---

# DEC-0277

## Título

Hunt Pipeline

## Status

LOCKED

## Decisão

Uma Hunt é um pipeline contínuo:

Pack → Combat → Loot → Próximo Pack.

---

# DEC-0278

## Título

WorldService Responsibility

## Status

LOCKED

## Decisão

WorldService apenas fornece dados do mundo.

Nunca executa lógica de gameplay.

---

# DEC-0279

## Título

Read Model vs Runtime Model

## Status

LOCKED

## Decisão

Templates representam o estado persistente.

Instances representam a simulação em tempo de execução.
---

# DEC-0280

## Título

Simulation Engine

## Status

LOCKED

## Decisão

O núcleo do Project-Victory é uma Engine de Simulação.

A API apenas envia comandos.

---

# DEC-0281

## Título

Runtime separado da API

## Status

LOCKED

## Decisão

A API nunca executa regras de negócio.

---

# DEC-0282

## Título

Game Loop

## Status

LOCKED

## Decisão

Toda a simulação do jogo é processada em um Game Loop central.

---

# DEC-0283

## Título

Tick Rate

## Status

LOCKED

## Decisão

Tick inicial:

250ms

Configuração alterável.

---

# DEC-0284

## Título

Tick Driven Runtime

## Status

LOCKED

## Decisão

Todos os sistemas do runtime são processados pelo Tick.

---

# DEC-0285

## Título

Runtime Components

## Status

LOCKED

## Decisão

Estados temporários do personagem existem apenas em memória.

---

# DEC-0286

## Título

Command Queue

## Status

LOCKED

## Decisão

Toda alteração no jogo ocorre através de Commands.

---

# DEC-0287

## Título

Event Queue

## Status

LOCKED

## Decisão

Todo Command gera Events.

---

# DEC-0288

## Título

Runtime Memory

## Status

LOCKED

## Decisão

Toda Hunt existe apenas na memória.

---

# DEC-0289

## Título

Engine First

## Status

LOCKED

## Decisão

A Engine é o núcleo do projeto.

A API é apenas uma interface de acesso.
---

# DEC-0290

## Título

ECS Lite Runtime

## Status

LOCKED

## Decisão

O Runtime utilizará composição através de Components, sem implementar uma ECS completa.

---

# DEC-0291

## Título

Systems Execute Logic

## Status

LOCKED

## Decisão

Toda regra de negócio do Runtime será executada por Systems.

Components armazenam apenas estado.

---

# DEC-0292

## Título

Composition Over Inheritance

## Status

LOCKED

## Decisão

O Runtime utiliza composição em vez de herança.

---

# DEC-0293

## Título

Systems Communicate by Events

## Status

LOCKED

## Decisão

Systems comunicam-se por eventos.

Não modificam diretamente o estado de outros Components.

---

# DEC-0294

## Título

Runtime Snapshot

## Status

LOCKED

## Decisão

A Engine pode gerar Snapshots por Tick para replay, depuração e renderização futura.

---

# DEC-0295

## Título

Simulation Recorder

## Status

LOCKED

## Decisão

A Engine pode registrar os eventos da simulação para reconstrução posterior da Hunt.

---

# DEC-0296

## Título

Runtime Plugin Architecture

## Status

LOCKED

## Decisão

Novos Systems devem ser adicionados como módulos independentes, sem alterar o núcleo da Engine.
---

# DEC-0297

## Título

Pure Domain

## Status

LOCKED

## Decisão

A Domain representa exclusivamente as regras do jogo e não depende de frameworks ou infraestrutura.

---

# DEC-0298

## Título

Small Entities

## Status

LOCKED

## Decisão

Entities representam estado. Regras vivem em Services e Systems.

---

# DEC-0299

## Título

Value Objects First

## Status

LOCKED

## Decisão

Conceitos imutáveis devem ser representados por Value Objects.

---

# DEC-0300

## Título

Small Aggregates

## Status

LOCKED

## Decisão

Aggregates devem permanecer pequenos e especializados.

---

# DEC-0301

## Título

Stateless Services

## Status

LOCKED

## Decisão

Services da Domain não armazenam estado interno.

---

# DEC-0302

## Título

Persistence Ignorance

## Status

LOCKED

## Decisão

Entities nunca acessam Repositories diretamente.

---

# DEC-0303

## Título

Testable Domain

## Status

LOCKED

## Decisão

Toda regra da Domain deve ser executável sem banco ou infraestrutura.

---

# DEC-0304

## Título

Deterministic Simulation

## Status

LOCKED

## Decisão

A simulação deve produzir sempre o mesmo resultado para os mesmos dados de entrada.

---

# DEC-0305

## Título

Zero Framework Domain

## Status

LOCKED

## Decisão

A Domain não referencia frameworks externos.

---

# DEC-0306

## Título

Domain First

## Status

LOCKED

## Decisão

Toda regra nasce primeiro na Domain.
---

# DEC-0307

## Título

Monorepo Architecture

## Status

LOCKED

## Decisão

Todo o Project-Victory será mantido em um único repositório Git.

---

# DEC-0308

## Título

Solution Structure

## Status

LOCKED

## Decisão

A Solution será organizada em projetos independentes por responsabilidade.

---

# DEC-0309

## Título

Dependency Direction

## Status

LOCKED

## Decisão

As dependências sempre apontam para o núcleo (Domain).

---

# DEC-0310

## Título

Dedicated Runtime Project

## Status

LOCKED

## Decisão

A Engine de Runtime será um projeto independente da API.

---

# DEC-0311

## Título

Test First Structure

## Status

LOCKED

## Decisão

A estrutura de testes fará parte da Solution desde o primeiro dia.

---

# DEC-0312

## Título

Small Shared Kernel

## Status

LOCKED

## Decisão

O projeto Shared deve permanecer pequeno e conter apenas elementos realmente compartilhados.
---

# DEC-0313

## Título

Architecture Safety Check

## Status

LOCKED

## Decisão

Sempre que uma limitação do ambiente conversacional puder comprometer a consistência do projeto, o roadmap deverá ser interrompido para criação ou atualização da documentação necessária antes da implementação.
---

# DEC-0314

## Título

Code Review Philosophy

## Status

LOCKED

## Decisão

Toda revisão de código deve validar:

- problema resolvido;
- aderência à arquitetura;
- conformidade com o Developer Guide;
- existência de testes;
- impacto na escalabilidade e manutenção.
---

# DEC-0315

## Título

Balanced Rich Domain Model

## Status

LOCKED

## Decisão

As Entities podem conter comportamento relacionado ao próprio estado.

Regras que envolvem múltiplas entidades ou infraestrutura permanecem em Services.

Objetivo:

- evitar Anemic Domain Model;
- evitar God Objects;
- manter alta coesão e baixo acoplamento.
---

# DEC-0316

## Título

Event Contract Stability

## Status

LOCKED

## Decisão

Todo Event publicado deve ser tratado como um contrato estável.

Mudanças incompatíveis exigem um novo Event ou uma nova versão explícita.

Objetivo:

- preservar compatibilidade;
- facilitar replay;
- permitir integrações futuras;
- evitar quebra de consumidores.
---

# DEC-0317

## Título

Persistence Ignorance

## Status

LOCKED

## Decisão

A Domain não possui conhecimento sobre o mecanismo de persistência utilizado.

Repositories são a única ponte entre Domain e Persistence.

Objetivos:

- baixo acoplamento;
- alta testabilidade;
- facilidade de substituição da infraestrutura.
---

# DEC-0318

## Título

Runtime Isolation

## Status

LOCKED

## Decisão

A Runtime Engine deve permanecer completamente isolada das demais camadas.

Ela comunica-se apenas através de Commands, Events e contratos definidos.

Objetivos:

- Engine reutilizável;
- Simulações offline;
- Replay;
- Alta testabilidade;
- Baixo acoplamento.
---

# DEC-0319

## Título

Test Before Fix

## Status

LOCKED

## Decisão

Todo bug deve ser reproduzido por um teste antes da correção.

Nenhuma correção crítica deve ser aplicada sem um teste que a valide.

---

# DEC-0320

## Título

Deterministic Tests

## Status

LOCKED

## Decisão

Testes que envolvam aleatoriedade devem utilizar Seeds fixas.

Objetivos:

- reprodutibilidade;
- estabilidade;
- depuração facilitada;
- confiança nos resultados.
---

# DEC-0321

## Título

Explicit Naming

## Status

LOCKED

## Decisão

Todo nome utilizado no Project-Victory deve priorizar clareza e legibilidade.

Nomes explícitos têm prioridade sobre nomes curtos.

Objetivos:

- facilitar manutenção;
- reduzir ambiguidades;
- acelerar entendimento do código;
- manter consistência entre módulos.
