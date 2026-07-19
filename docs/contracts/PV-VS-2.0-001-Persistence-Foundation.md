docs/contracts/PV-VS-2.0-001-Persistence-Foundation.md

# PV-VS-2.0-001 — Persistence Foundation

## Tipo

Technical Contract

---

## Objetivo

Substituir a persistência temporária em memória por uma camada de armazenamento real mantendo a arquitetura existente.

---

## Motivação

O Vertical Slice atual perde dados ao reiniciar a aplicação.

A fundação de persistência é necessária para suportar evolução dos sistemas futuros.

---

## Escopo

Implementar:

- Banco de dados.
- Configuração de conexão.
- Persistência de Account.
- Persistência de Character.
- Implementação dos repositories utilizando armazenamento persistente.

---

## Fluxo esperado

API

↓

Application

↓

Domain

↓

Repository Interface

↓

Database Implementation

---

## Dados obrigatórios persistentes

### Account

Manter:

- Identificador.
- Email.
- Credenciais necessárias para autenticação.

---

### Character

Manter:

- Identificador.
- Account vinculada.
- Nome.
- Level.
- Experience.
- Estado necessário para continuidade do personagem.

---

## Restrições

Não implementar:

- Inventário.
- Items.
- Monsters.
- Hunts.
- Economia.
- Sistemas futuros.

---

## Requisitos arquiteturais

- Manter contratos existentes dos repositories.
- Não alterar regras de domínio sem necessidade.
- Não mover lógica de negócio para persistência.
- Database deve ser responsabilidade de Infrastructure/Persistence.

---

## Critérios de aceite

Considerado aprovado quando:

Criar Account

↓

Reiniciar API

↓

Login continua funcionando

↓

Character existente continua disponível

---

## Status

PV-VS-2.0-001 — Planejamento arquitetural.