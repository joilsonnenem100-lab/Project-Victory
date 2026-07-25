docs/contracts/PV-MILESTONE-003-Execution-Control.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Execution Control

## Tipo

Execution Control Document

---

## Objetivo

Definir a ordem oficial de execução, validação e controle do Vertical Slice 2.0.

---

## Ordem oficial de implementação

A Engenharia deve seguir a sequência abaixo:

PV-VS-2.0-001  
Persistence Foundation

↓

PV-VS-2.0-002  
Account Experience Foundation

↓

PV-VS-2.0-003  
Hunt Selection Foundation

↓

PV-VS-2.0-004  
Monster Encounter Foundation

↓

PV-VS-2.0-005  
Combat Evolution Foundation

↓

PV-VS-2.0-006  
Item Drop Foundation

↓

PV-VS-2.0-007  
Integration Validation

---

## Regras de execução

Cada etapa deve:

- Possuir contrato aprovado antes da implementação.
- Ser implementada isoladamente.
- Passar por validação antes do próximo contrato.
- Evitar alterações fora do escopo definido.

---

## Processo obrigatório

Fluxo:

Arquitetura

↓

Contrato aprovado

↓

Engenharia implementa

↓

Build/Testes

↓

Validação funcional

↓

Aprovação

↓

Próximo contrato

---

## Validações técnicas obrigatórias

Após cada implementação:

Backend:

dotnet test ProjectVictory.sln

Client:

npm run build

---

## Controle de escopo

Não adicionar durante o Vertical Slice 2.0:

- Sistemas futuros.
- Melhorias não relacionadas ao contrato atual.
- Refatorações sem necessidade funcional.

---

## Critério de interrupção

A Engenharia deve interromper a implementação quando:

- O contrato estiver incompleto.
- Existir conflito arquitetural.
- Uma decisão de domínio não estiver definida.
- Uma alteração exigir mudança fora do escopo aprovado.

---

## Responsabilidades

### Arquitetura

Responsável por:

- Definir contratos.
- Revisar conflitos.
- Aprovar evolução.

### Engenharia

Responsável por:

- Implementar contratos.
- Reportar bloqueios.
- Não criar arquitetura própria.

### Product Owner

Responsável por:

- Definir experiência desejada.
- Aprovar decisões de produto.

---

## Status

PV-MILESTONE-003 — Controle de execução definido.