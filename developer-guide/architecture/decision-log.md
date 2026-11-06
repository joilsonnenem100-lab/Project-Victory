# Project-Victory

# Architecture Decision Log

Versão: 1.0

Status: Oficial

---

# Objetivo

Este documento registra todas as decisões arquiteturais oficiais do Project-Victory.

Seu objetivo é preservar o motivo de cada decisão tomada durante o desenvolvimento.

Toda decisão registrada neste documento passa a fazer parte da arquitetura oficial do projeto.

---

# Como utilizar

Cada decisão recebe um identificador único.

Formato:

ADR-XXX

Exemplo:

ADR-001

Uma decisão registrada somente poderá ser alterada através de uma nova ADR.

---

# ADR-001

Título:

Character como Aggregate Root

Status:

Accepted

Resumo:

Character representa o Aggregate Root principal do domínio.

Todos os sistemas pertencentes ao personagem são acessados através dele.

---

# ADR-002

Título:

Skills pertencem ao Character

Status:

Accepted

Resumo:

Skills fazem parte do Aggregate Character.

Skills não são Entity.

Skills não são Aggregate Root.

---

# ADR-003

Título:

Progressão baseada em uso

Status:

Accepted

Resumo:

Skills evoluem através da utilização correspondente.

A velocidade de evolução depende da Vocation.

---

# ADR-004

Título:

Item Template separado de Item Instance

Status:

Accepted

Resumo:

Templates representam definições permanentes.

Instances representam itens existentes no mundo.

---

# ADR-005

Título:

Affixes são permanentes

Status:

Accepted

Resumo:

Affixes são gerados durante a criação da Item Instance.

Nunca fazem parte do Template.

---

# ADR-006

Título:

Imbuements são temporários

Status:

Accepted

Resumo:

Imbuements representam modificações temporárias aplicadas posteriormente pelo jogador.

São independentes dos Affixes.

---

# ADR-007

Título:

Loot gera Templates

Status:

Accepted

Resumo:

Loot nunca cria Item Instances.

Loot seleciona apenas Item Templates.

A Item Factory cria as Instances.

---

# ADR-008

Título:

Combat utiliza Combat Context

Status:

Accepted

Resumo:

Combat Engine nunca consulta Aggregates diretamente durante seus cálculos.

Toda execução utiliza exclusivamente um Combat Context imutável.

---

# ADR-009

Título:

Combat produz Combat Result

Status:

Accepted

Resumo:

Toda Action produz exatamente um Combat Result.

Combat Result representa exclusivamente a saída da Combat Engine.

---

# ADR-010

Título:

Combat Pipeline determinístico

Status:

Accepted

Resumo:

Toda Action percorre exatamente o mesmo Pipeline.

Não existem pipelines específicos por tipo de Action.

---

# ADR-011

Título:

Vocation representa um Profile

Status:

Accepted

Resumo:

Vocation fornece parâmetros para os demais sistemas.

Ela não implementa regras de combate.

---

# ADR-012

Título:

Skill Multipliers pertencem à Vocation

Status:

Accepted

Resumo:

A velocidade de evolução das Skills é determinada pela Vocation.

Skills permanecem independentes.

---

# ADR-013

Título:

Resources desacoplados da Combat Engine

Status:

Accepted

Resumo:

Combat apenas utiliza Resources.

Resources gerenciam seu próprio ciclo de vida.

---

# ADR-014

Título:

Documentação é a única fonte de verdade

Status:

Accepted

Resumo:

Conversas servem apenas para produzir documentação.

Em caso de conflito, a documentação oficial prevalece.

---

# ADR-015

Título:

Roadmap obrigatório

Status:

Accepted

Resumo:

Todo novo módulo deverá consultar obrigatoriamente:

- roadmap.md
- project-state.md
- architecture-index.md

antes de iniciar qualquer evolução do projeto.

---

# ADR-016

Título:

Separação de responsabilidades

Status:

Accepted

Resumo:

Product Owner:

- Game Design
- Mecânicas
- Experiência do jogador
- Universo do jogo

Arquiteto:

- Arquitetura
- Engenharia
- Organização dos módulos
- Roadmap
- Documentação
- Revisões arquiteturais

---

# ADR-017

Título:

Documentação incremental

Status:

Accepted

Resumo:

Toda decisão consolidada deverá ser documentada imediatamente.

Grandes módulos poderão ser escritos em capítulos.

A implementação nunca deverá preceder a documentação oficial.

---

# ADR-018

Título:

Transição segura entre conversas

Status:

Accepted

Resumo:

A continuidade do Project-Victory não depende do histórico da conversa.

A continuidade depende exclusivamente da documentação oficial consolidada.

---

# Registro da Revisão de 2026-11-05

> **Nota de numeração (R-009):** este arquivo possui uma série própria de decisões `ADR-001…018`, distinta da série de `developer-guide/adr/ADR-001…057`. Para evitar nova colisão, as decisões abaixo usam o prefixo `DEC-`. Unificar as séries exige decisão do Product Owner/Arquiteto e não foi feito.

## DEC-20261105-01 — Código se conforma aos ADRs LOCKED

Status: Accepted (Arquitetura)

Onde o código diverge de ADR ACCEPTED sem decisão que o autorize (Level persistido × ADR-027/028; tabela fixa × ADR-025), o código é corrigido. Lastro: hierarquia da fonte da verdade; `PROJECT-VICTORY-ARCHITECTURE-RULES.md` (substituição só com decisão + contrato + validação).

## DEC-20261105-02 — Segurança e Server Authority em todo endpoint de gameplay

Status: Accepted (Arquitetura)

Todo endpoint de gameplay exige autenticação; identidade vem do token; recurso é verificado quanto ao dono. Lastro: `PRINCIPLES` ("O cliente solicita. O servidor decide."), Handbook V3 §23–24.

## DEC-20261105-03 — Stubs são substituídos atrás da mesma interface

Status: Accepted (Arquitetura)

`CombatResolver`, `RewardProcessor` e `ProgressionProcessor` (stubs/provisórios) são substituídos nos marcos M2–M3 mantendo interfaces, DTOs e endpoints. Lastro: Regra Aditiva (Backend).

## DEC-20261105-04 — Código sem referência em quarentena

Status: Accepted (Arquitetura)

`Application/Architecture`, `Playtest` e `Production` (42 arquivos, sem referências) só são removidos após `dotnet build` verde, em contrato de limpeza (M1). Lastro: Regra Aditiva ("builds continuam passando").

## DEC-20261105-05 — Roadmap de execução

Status: Accepted (Arquitetura) — **aguarda aprovação do Product Owner no M0**

`roadmap-to-playable.md` é o roadmap de execução; `roadmap.md` permanece a ordem oficial de módulos. Resolve R-005. Lastro: ADR-016 (Arquiteto responde por Roadmap) e `roadmap.md` (Roadmap Rules).

## DEC-20261105-06 — Política de desconexão da Hunt

Status: **Pendente** — ver `ADR-057` (PROPOSED). A parte de Game Design pertence ao Product Owner (ADR-016 deste log).

## DEC-20261105-07 — Higiene do repositório

Status: Accepted (Arquitetura)

`node_modules/` e `dist/` deixam de ser versionados; binários e listagens sem valor de fonte são removidos; `projectvictory.db` permanece até o `PV-PLATFORM-001`. Lastro: Regra Aditiva (nenhuma funcionalidade removida) e clone limpo como pré-requisito de baseline.
