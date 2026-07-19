docs/contracts/PV-VS-2.0-002-Client-Account-Experience-Contract.md

# PV-VS-2.0-002 — Client Account Experience Contract

## Tipo

Technical Implementation Contract

---

## Objetivo

Implementar o fluxo de criação de Account através do Client, permitindo que o jogador inicie sua experiência sem depender de ferramentas externas.

---

## Contexto

O Vertical Slice atual permite criação de Account através de chamadas manuais.

Esta etapa transfere o fluxo inicial para a experiência real do jogador.

---

## Escopo

Implementar:

- Tela de criação de Account.
- Comunicação Client com API de Account.
- Envio dos dados necessários.
- Tratamento básico de resposta.
- Continuidade para Login.

---

## Fluxo esperado

Jogador acessa Client

↓

Tela Create Account

↓

Envia dados

↓

API cria Account

↓

Persistência salva Account

↓

Usuário realiza Login

↓

Acessa fluxo de Character

---

## Dados mínimos

Account deve utilizar:

- Email.
- Senha.

---

## Requisitos arquiteturais

- Client não deve conter regras de negócio.
- Validações permanecem no Backend.
- API mantém responsabilidade pelo contrato externo.
- Persistência permanece isolada na camada responsável.

---

## Fora do escopo

Não implementar:

- Recuperação de senha.
- Confirmação de email.
- Social Login.
- Perfil de usuário.
- Configurações de conta.
- Segurança avançada.

---

## Critérios de aceite

Considerado aprovado quando:

Novo jogador acessa o Client

↓

Cria Account

↓

Recebe confirmação de criação

↓

Consegue realizar Login

↓

Continua fluxo do jogador

---

## Validação obrigatória

Backend:

dotnet test ProjectVictory.sln

Client:

npm run build

---

## Dependências

Depende de:

PV-VS-2.0-001 — Persistence Foundation

---

## Próxima etapa

Após aprovação:

PV-VS-2.0-003 — Hunt Selection Foundation

---

## Status

PV-VS-2.0-002 — Pronto para implementação.