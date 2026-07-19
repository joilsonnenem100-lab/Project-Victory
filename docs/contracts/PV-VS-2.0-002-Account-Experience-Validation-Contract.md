docs/contracts/PV-VS-2.0-002-Account-Experience-Validation-Contract.md

# PV-VS-2.0-002 — Account Experience Validation Contract

## Tipo

Validation Contract

---

## Objetivo

Definir a validação obrigatória do fluxo completo de Account através do Client antes da evolução para novos sistemas dependentes.

---

## Finalidade

Garantir que o jogador consegue iniciar sua jornada utilizando apenas a experiência oficial do produto.

---

## Escopo de validação

Validar:

- Criação de Account pelo Client.
- Comunicação Client/API.
- Persistência da Account.
- Login após criação.
- Continuidade do fluxo para Character.

---

## Fluxo de validação

Acessar Client

↓

Abrir criação de Account

↓

Informar email e senha

↓

Enviar cadastro

↓

Confirmar criação

↓

Realizar Login

↓

Receber autenticação

↓

Acessar próxima etapa do jogador

---

## Requisitos arquiteturais

Validar:

- Client utiliza apenas contratos da API.
- API mantém regras de criação.
- Persistência mantém dados após reinício.
- Não existem fluxos paralelos necessários para iniciar o jogo.

---

## Critérios de aceite

Considerado aprovado quando:

- Novo jogador consegue criar Account pelo Client.
- Account permanece salva após reinício da aplicação.
- Login funciona utilizando os dados criados.
- Fluxo não depende de comandos externos.

---

## Critérios de bloqueio

Reprovar quando:

- Cadastro funciona somente via PowerShell/Postman.
- Dados são perdidos após reinício.
- Client possui lógica de negócio.
- API e Client possuem contratos divergentes.

---

## Validação técnica

Executar:

Backend:

dotnet test ProjectVictory.sln

---

Client:

npm run build

---

## Dependências

Depende de:

PV-VS-2.0-002 — Client Account Experience Contract

---

## Próxima etapa

Após aprovação:

PV-VS-2.0-003 — Hunt Selection Foundation

---

## Status

PV-VS-2.0-002 — Validação de Account Experience definida.