docs/contracts/PV-MILESTONE-003-Architecture-Knowledge-Transfer.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Architecture Knowledge Transfer

## Tipo

Architecture Handover Document

---

## Objetivo

Registrar o conhecimento arquitetural necessário para continuidade do desenvolvimento do Vertical Slice 2.0 em novos ciclos ou novas sessões de trabalho.

---

## Contexto do projeto

O Project-Victory utiliza evolução incremental baseada em Vertical Slices.

Cada evolução deve:

- Possuir contrato definido.
- Ter escopo controlado.
- Ser validada antes da próxima etapa.
- Preservar decisões arquiteturais existentes.

---

## Estado arquitetural inicial

Vertical Slice 1.0 validado:

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

## Direção do Vertical Slice 2.0

A evolução deve transformar o protótipo em uma primeira experiência persistente:

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

## Princípios obrigatórios

### Evolução incremental

Implementar somente a fundação necessária para o próximo objetivo.

---

### Separação de responsabilidades

Manter:

Domain

↓

Application

↓

Infrastructure

↓

API

↓

Client

---

### Contratos antes de código

Nenhuma implementação deve iniciar sem contrato aprovado.

---

### Controle de escopo

Evitar criação antecipada de sistemas completos.

---

## Decisões importantes

### Persistência primeiro

Banco de dados deve existir antes da expansão de sistemas de gameplay.

Motivo:

Evitar retrabalho causado por sistemas temporários.

---

### Monster separado de Combat

Monster representa o conteúdo.

Combat representa a resolução.

---

### Reward separado de Combat

Combat determina resultado.

Reward determina recompensas.

---

### Item separado de Inventory

Item deve existir antes de sistemas completos de gerenciamento.

---

## Fluxo de trabalho oficial

Arquitetura

↓

Contrato

↓

Engenharia

↓

Validação técnica

↓

Validação funcional

↓

Aprovação

---

## Responsabilidades

### Product Owner

Responsável por:

- Visão do jogo.
- Prioridades.
- Experiência desejada.

---

### Arquitetura

Responsável por:

- Contratos.
- Decisões técnicas.
- Controle de escopo.
- Revisão arquitetural.

---

### Engenharia

Responsável por:

- Implementação.
- Testes.
- Entrega conforme contrato.

---

## Critério de continuidade

Antes de qualquer novo ciclo:

- Revisar estado atual.
- Confirmar contratos concluídos.
- Avaliar impacto arquitetural.
- Criar novos contratos.

---

## Status

PV-MILESTONE-003 — Transferência de conhecimento definida.