docs/contracts/PV-MILESTONE-003-Dependency-Map.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Dependency Map

## Tipo

Architecture Planning Document

---

## Objetivo

Definir a relação de dependência entre os contratos do Vertical Slice 2.0.

---

## Dependências principais

PV-VS-2.0-001  
Persistence Foundation

Base necessária para:

- Account.
- Character.
- Estado persistente do jogador.

---

PV-VS-2.0-002  
Account Experience Foundation

Depende de:

- Persistence Foundation.

Fornece base para:

- Entrada completa do jogador.
- Fluxo inicial pelo Client.

---

PV-VS-2.0-003  
Hunt Selection Foundation

Depende de:

- Persistence Foundation.
- Account Experience Foundation.

Fornece base para:

- Seleção de conteúdo jogável.

---

PV-VS-2.0-004  
Monster Encounter Foundation

Depende de:

- Hunt Selection Foundation.

Fornece base para:

- Criação de encontros de combate.

---

PV-VS-2.0-005  
Combat Evolution Foundation

Depende de:

- Monster Encounter Foundation.

Fornece base para:

- Resolução de combate baseada em entidades.

---

PV-VS-2.0-006  
Item Drop Foundation

Depende de:

- Combat Evolution Foundation.

Fornece base para:

- Recompensas com identidade.

---

PV-VS-2.0-007  
Integration Validation

Depende de:

- Todos os contratos anteriores.

Responsável por:

- Validar fluxo completo.

---

## Ordem obrigatória

A implementação deve respeitar:

PV-VS-2.0-001

↓

PV-VS-2.0-002

↓

PV-VS-2.0-003

↓

PV-VS-2.0-004

↓

PV-VS-2.0-005

↓

PV-VS-2.0-006

↓

PV-VS-2.0-007

---

## Regra de alteração

Nenhuma etapa deve criar dependência futura desnecessária.

Caso uma dependência não prevista seja identificada:

- interromper implementação;
- informar Arquitetura;
- revisar contrato.

---

## Status

PV-MILESTONE-003 — Mapa de dependências definido.