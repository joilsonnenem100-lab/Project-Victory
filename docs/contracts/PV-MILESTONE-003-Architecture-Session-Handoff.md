docs/contracts/PV-MILESTONE-003-Architecture-Session-Handoff.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Architecture Session Handoff

## Tipo

Session Transition Document

---

## Objetivo

Garantir continuidade arquitetural entre sessões de desenvolvimento sem depender do histórico completo da conversa anterior.

---

## Contexto do projeto

O Project-Victory utiliza desenvolvimento orientado por contratos.

A Arquitetura é responsável por:

- Definir direção técnica.
- Criar contratos.
- Controlar escopo.
- Validar decisões.

A Engenharia é responsável por:

- Implementar contratos aprovados.
- Executar validações.
- Reportar bloqueios.

---

## Estado atual

Vertical Slice 1.0 concluído:

Account

↓

Login JWT

↓

Character

↓

World Entry

↓

Hunt Execution

↓

Combat Result

↓

Reward Presentation

---

## Próximo ciclo

Vertical Slice 2.0.

Objetivo:

Evoluir o protótipo para uma experiência persistente:

Account

↓

Character

↓

Hunt

↓

Monster

↓

Combat

↓

Reward

↓

Item

---

## Primeiro contrato de implementação

PV-VS-2.0-001

Persistence Foundation

Objetivo:

Adicionar persistência real mantendo a arquitetura existente.

---

## Ordem oficial

PV-VS-2.0-001

↓

PV-VS-2.0-002

↓

PV-VS-2.0-003

↓

PV-VS-2.0-004

↓

PV-VS-2.0-005

↓

PV-VS-2.0-006

↓

PV-VS-2.0-007

---

## Regras mantidas

- Contrato antes de implementação.
- Não criar sistemas futuros antecipadamente.
- Não alterar arquitetura sem revisão.
- Validar cada etapa antes da próxima.
- Manter separação entre Domain, Application, Infrastructure, API e Client.

---

## Documentação de referência

Documentos oficiais:

- PV-MILESTONE-003 — Vertical Slice 2.0 Foundation.
- PV-VS-2.0-001 até PV-VS-2.0-007.
- Documentos de execução e controle do milestone.

---

## Próxima ação

Após iniciar nova sessão:

1. Carregar esta documentação.
2. Confirmar estado atual do repositório.
3. Revisar PV-VS-2.0-001.
4. Preparar solicitação para Engenharia.

---

## Status

PV-MILESTONE-003 — Sessão preparada para continuidade.