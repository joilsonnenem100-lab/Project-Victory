docs/contracts/PV-MILESTONE-003-Implementation-Scope-Control.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Implementation Scope Control

## Tipo

Scope Control Document

---

## Objetivo

Definir os limites de implementação do Vertical Slice 2.0 para garantir foco no objetivo principal e evitar expansão não planejada.

---

## Escopo aprovado

O Vertical Slice 2.0 deve entregar:

- Persistência real do jogador.
- Criação de Account pelo Client.
- Login persistente.
- Character persistente.
- Seleção de uma Hunt.
- Encontro com um Monster.
- Combate funcional.
- Recompensa com Item.
- Apresentação das características do Item.

---

## Escopo permitido durante implementação

São permitidas alterações necessárias para:

- Cumprir contratos aprovados.
- Corrigir integrações.
- Melhorar consistência arquitetural.
- Ajustar contratos existentes quando houver justificativa.

---

## Escopo proibido

Não implementar:

- Sistema completo de Inventário.
- Equipamentos completos.
- Sistema de Skills.
- Sistema de Classes.
- Sistema de Quests.
- Economia.
- Trade.
- Guild.
- PvP.
- Multiplayer avançado.
- Sistemas de progressão completos.
- Conteúdo em larga escala.

---

## Regra de expansão

Caso uma necessidade fora do escopo seja identificada:

Fluxo obrigatório:

Identificar necessidade

↓

Registrar impacto

↓

Avaliação da Arquitetura

↓

Aprovação do Product Owner

↓

Novo contrato

---

## Critério de controle

Uma implementação deve ser interrompida quando:

- O objetivo do contrato já foi atendido.
- A solução exige sistemas futuros.
- Existe aumento de complexidade sem valor para o Vertical Slice.

---

## Responsabilidade

Arquitetura:

- Garantir controle de escopo.

Engenharia:

- Implementar somente o escopo aprovado.

Product Owner:

- Aprovar mudanças de prioridade.

---

## Status

PV-MILESTONE-003 — Controle de escopo definido.