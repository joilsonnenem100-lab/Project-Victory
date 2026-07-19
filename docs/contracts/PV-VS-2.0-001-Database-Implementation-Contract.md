docs/contracts/PV-VS-2.0-001-Database-Implementation-Contract.md

# PV-VS-2.0-001 — Database Implementation Contract

## Tipo

Technical Implementation Contract

---

## Objetivo

Definir a implementação da camada de banco de dados necessária para substituir a persistência temporária em memória do Vertical Slice atual.

---

## Contexto

O Vertical Slice atual utiliza armazenamento temporário durante a execução da aplicação.

Esta etapa cria a base persistente necessária para evolução dos sistemas futuros.

---

## Escopo

Implementar:

- Configuração do banco de dados.
- Conexão da aplicação com o banco.
- Estrutura inicial de persistência.
- Migração dos dados necessários para Account.
- Migração dos dados necessários para Character.

---

## Entidades persistentes

### Account

Persistir:

- Id.
- Email.
- Dados necessários para autenticação.

---

### Character

Persistir:

- Id.
- AccountId.
- Name.
- Level.
- Experience.
- Dados mínimos necessários para continuidade do personagem.

---

## Requisitos arquiteturais

A implementação deve manter:

Domain

↓

Application

↓

Persistence / Infrastructure

↓

API

---

## Regras

- Interfaces de Repository existentes devem ser preservadas quando possível.
- A lógica de negócio não deve ser movida para a camada de persistência.
- O banco deve ser responsável apenas pelo armazenamento.
- Application continua responsável pelos casos de uso.

---

## Fora do escopo

Não implementar:

- Inventory.
- Items.
- Monsters.
- Hunts.
- Combat.
- Economy.
- Progression avançada.

---

## Critérios de aceite

Considerado aprovado quando:

Account criada

↓

Aplicação reiniciada

↓

Login continua funcionando


Character criado

↓

Aplicação reiniciada

↓

Character continua disponível

---

## Validação obrigatória

Executar:

dotnet test ProjectVictory.sln

Validar:

- Build concluído.
- Testes executados.
- Nenhuma regressão no fluxo existente.

---

## Dependências

Depende de:

PV-MILESTONE-003 — Vertical Slice 2.0 Foundation

---

## Status

PV-VS-2.0-001 — Pronto para implementação.