docs/contracts/PV-VS-2.0-007-Vertical-Slice-2.0-Integration-Validation.md

# PV-VS-2.0-007 — Vertical Slice 2.0 Integration Validation

## Tipo

Technical Contract

---

## Objetivo

Definir a validação final da integração entre as fundações do Vertical Slice 2.0.

---

## Motivação

As fundações individuais precisam formar um fluxo completo de experiência do jogador antes da próxima evolução do projeto.

---

## Escopo

Validar a integração:

- Account persistente.
- Login persistente.
- Character persistente.
- Seleção de Hunt.
- Monster Encounter.
- Combat Evolution.
- Reward.
- Item Drop.
- Apresentação do resultado.

---

## Fluxo esperado

Create Account

↓

Login

↓

Character

↓

Selecionar Hunt

↓

Criar Encounter

↓

Combater Monster

↓

Receber Reward

↓

Receber Item

↓

Visualizar resultado

---

## Requisitos arquiteturais

- Cada fundação deve manter sua responsabilidade.
- O fluxo completo deve utilizar os contratos aprovados.
- Não adicionar sistemas fora do escopo do Vertical Slice 2.0.
- Problemas encontrados devem ser corrigidos na fundação responsável.

---

## Critérios de aceite

Considerado aprovado quando:

Novo jogador consegue iniciar pelo Client.

↓

Dados permanecem após reinício da aplicação.

↓

Jogador executa uma Hunt completa.

↓

Monster é enfrentado.

↓

Reward é recebido.

↓

Item é apresentado.

---

## Restrições

Não validar:

- Conteúdo final do jogo.
- Balanceamento.
- Economia.
- Multiplayer.
- Sistemas futuros.

---

## Dependências

Depende de:

PV-VS-2.0-001 — Persistence Foundation

PV-VS-2.0-002 — Account Experience Foundation

PV-VS-2.0-003 — Hunt Selection Foundation

PV-VS-2.0-004 — Monster Encounter Foundation

PV-VS-2.0-005 — Combat Evolution Foundation

PV-VS-2.0-006 — Item Drop Foundation

---

## Status

PV-VS-2.0-007 — Planejamento arquitetural.