docs/contracts/PV-MILESTONE-003-Architecture-Review-Gate.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Architecture Review Gate

## Tipo

Architecture Control Document

---

## Objetivo

Definir o ponto obrigatório de revisão arquitetural antes do início de cada implementação do Vertical Slice 2.0.

---

## Finalidade

Garantir que cada etapa:

- Possui contrato aprovado.
- Possui dependências definidas.
- Não cria conflitos com a arquitetura existente.
- Não amplia escopo sem aprovação.

---

## Momento de revisão

A revisão deve ocorrer:

Antes da Engenharia iniciar cada contrato.

---

## Checklist arquitetural

Validar:

- O objetivo do contrato está claro.
- O escopo está definido.
- As responsabilidades estão separadas.
- As dependências foram identificadas.
- Existem critérios de aceite definidos.
- Não existem decisões de produto pendentes.

---

## Análise de impacto

Antes da aprovação verificar:

### Domain

- Novas entidades necessárias.
- Alterações em regras existentes.
- Impacto em agregados.

### Application

- Novos casos de uso.
- Alteração de fluxos existentes.
- Dependências necessárias.

### Infrastructure

- Persistência.
- Serviços externos.
- Implementações técnicas.

### API

- Novos endpoints.
- Alterações de contratos existentes.

### Client

- Novas telas.
- Alterações de fluxo do jogador.

---

## Critério de aprovação

O contrato está aprovado quando:

Arquitetura confirma:

- Escopo definido.
- Dependências resolvidas.
- Riscos identificados.
- Engenharia pode iniciar implementação.

---

## Critério de bloqueio

A implementação deve aguardar quando:

- Existe dúvida de comportamento do sistema.
- O contrato exige decisão de produto.
- Existe conflito com arquitetura existente.
- A solução proposta cria retrabalho previsível.

---

## Status

PV-MILESTONE-003 — Gate de revisão arquitetural definido.