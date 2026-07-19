docs/contracts/PV-VS-2.0-002-Account-Experience-Foundation.md

# PV-VS-2.0-002 — Account Experience Foundation

## Tipo

Technical Contract

---

## Objetivo

Permitir que o jogador realize o fluxo inicial completo pelo Client, removendo dependência de criação manual via ferramentas externas.

---

## Motivação

O Vertical Slice atual exige criação de Account através de chamadas manuais.

O objetivo desta fundação é aproximar o protótipo da experiência real do jogador.

---

## Escopo

Implementar:

- Tela de criação de Account.
- Integração Client com API de criação.
- Validação básica de dados.
- Fluxo de Login após criação.
- Continuidade para criação ou seleção de Character.

---

## Fluxo esperado

Client

↓

Create Account

↓

Account API

↓

Persistência de Account

↓

Login

↓

Character Flow

---

## Dados obrigatórios

### Account

Utilizar:

- Email.
- Credencial necessária para autenticação.
- Identificador único.

---

## Requisitos arquiteturais

- Utilizar os contratos existentes de Account.
- Manter autenticação JWT existente.
- Não duplicar regras de criação no Client.
- Validações de negócio permanecem na camada Application/Domain.

---

## Restrições

Não implementar:

- Recuperação de senha.
- Confirmação de email.
- Social Login.
- Sistema de segurança avançado.
- Perfil completo de usuário.

---

## Critérios de aceite

Considerado aprovado quando:

Usuário acessa o Client

↓

Cria uma nova Account

↓

Realiza Login

↓

Recebe autenticação válida

↓

Continua para o fluxo de Character

---

## Dependências

Depende de:

PV-VS-2.0-001 — Persistence Foundation

---

## Status

PV-VS-2.0-002 — Planejamento arquitetural.