docs/contracts/PV-MILESTONE-003-Decision-Log.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Decision Log

## Tipo

Architecture Record Document

---

## Objetivo

Registrar decisões relevantes tomadas durante o planejamento do Vertical Slice 2.0.

---

## Decisão 1 — Persistência como primeira fundação

Decisão:

A evolução deve iniciar pela implementação de persistência real.

Motivo:

Evitar criação de sistemas temporários que necessitem migração futura.

---

## Decisão 2 — Client como ponto principal de experiência

Decisão:

Fluxos principais do jogador devem ser executados pelo Client.

Motivo:

O Vertical Slice deve representar uma experiência próxima do produto final.

---

## Decisão 3 — Hunt como conteúdo selecionável

Decisão:

Hunts devem possuir definição própria e serem selecionadas pelo jogador.

Motivo:

Permitir expansão futura de conteúdo sem alterar o fluxo principal.

---

## Decisão 4 — Monster como entidade própria

Decisão:

Monster deve existir separado da lógica de combate.

Motivo:

Permitir evolução independente de conteúdo e mecânica.

---

## Decisão 5 — Combat desacoplado de Reward

Decisão:

Combat apenas determina resultado da batalha.

Reward é responsável pela entrega de recompensas.

Motivo:

Manter responsabilidades independentes.

---

## Decisão 6 — Item antes de Inventory

Decisão:

Criar definição de Item antes de criar sistema completo de inventário.

Motivo:

Validar identidade de recompensa sem adicionar complexidade prematura.

---

## Decisão 7 — Evitar sistemas completos no protótipo

Decisão:

O Vertical Slice 2.0 deve implementar somente fundações.

Não criar:

- Sistema completo de skills.
- Sistema completo de classes.
- Sistema completo de equipamentos.
- Economia.
- Multiplayer avançado.

---

## Decisão 8 — Contratos obrigatórios antes de implementação

Decisão:

Nenhuma implementação deve iniciar sem contrato técnico aprovado.

Motivo:

Reduzir retrabalho e manter alinhamento entre Produto, Arquitetura e Engenharia.

---

## Status

PV-MILESTONE-003 — Registro de decisões concluído.