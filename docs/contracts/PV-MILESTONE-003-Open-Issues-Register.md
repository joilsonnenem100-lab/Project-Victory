docs/contracts/PV-MILESTONE-003-Open-Issues-Register.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Open Issues Register

## Tipo

Architecture Control Document

---

## Objetivo

Registrar pontos que devem ser avaliados durante a implementação do Vertical Slice 2.0.

---

## Issue 1 — Definição do banco de dados

Status:

Pendente.

Descrição:

Definir tecnologia, configuração e estratégia de persistência.

Impacto:

Necessário para iniciar Persistence Foundation.

---

## Issue 2 — Migração dos repositories

Status:

Pendente.

Descrição:

Adaptar implementações atuais em memória para persistência real mantendo os contratos existentes.

Impacto:

Pode afetar Account e Character.

---

## Issue 3 — Persistência de estado do jogador

Status:

Pendente.

Descrição:

Definir quais dados mínimos do Character precisam sobreviver ao reinício da aplicação.

Impacto:

Evitar armazenamento desnecessário.

---

## Issue 4 — Modelo inicial de Hunt

Status:

Pendente.

Descrição:

Definir estrutura mínima para representar uma Hunt selecionável.

Impacto:

Necessário para Hunt Selection Foundation.

---

## Issue 5 — Modelo inicial de Monster

Status:

Pendente.

Descrição:

Definir atributos mínimos necessários para o primeiro Monster.

Impacto:

Necessário para Monster Encounter Foundation.

---

## Issue 6 — Modelo inicial de Item

Status:

Pendente.

Descrição:

Definir quais propriedades mínimas serão apresentadas ao jogador.

Impacto:

Necessário para Item Drop Foundation.

---

## Issue 7 — Contrato entre Client e API

Status:

Pendente.

Descrição:

Validar novos contratos de comunicação conforme novas fundações forem implementadas.

Impacto:

Evitar divergência entre Client e Backend.

---

## Regra de tratamento

Nenhuma Issue deve gerar implementação sem:

- Decisão arquitetural.
- Atualização do contrato correspondente.
- Aprovação.

---

## Status

PV-MILESTONE-003 — Registro de pontos abertos criado.