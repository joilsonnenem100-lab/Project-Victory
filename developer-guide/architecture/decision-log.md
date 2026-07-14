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