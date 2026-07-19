docs/contracts/PV-MILESTONE-003-Architecture-Governance-Rules.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Architecture Governance Rules

## Tipo

Architecture Governance Document

---

## Objetivo

Definir regras permanentes de governança arquitetural aplicáveis durante a evolução do Vertical Slice 2.0.

---

## Regra 1 — Contrato como fonte de verdade

Todo desenvolvimento deve possuir contrato correspondente aprovado.

O contrato define:

- Objetivo.
- Escopo.
- Limitações.
- Critérios de aceite.

---

## Regra 2 — Implementação orientada por necessidade

Não implementar:

- Abstrações sem uso.
- Sistemas futuros antecipadamente.
- Complexidade sem benefício imediato.

---

## Regra 3 — Preservação de responsabilidades

Cada camada deve manter sua função:

Domain:

- Regras de negócio.
- Estados válidos.

Application:

- Casos de uso.
- Orquestração.

Infrastructure:

- Implementações técnicas.

API:

- Comunicação externa.

Client:

- Experiência do jogador.

---

## Regra 4 — Evolução incremental

Cada entrega deve:

- Possuir valor funcional.
- Ser validada isoladamente.
- Preparar evolução futura sem criar dependência desnecessária.

---

## Regra 5 — Controle de escopo

Qualquer funcionalidade fora do contrato atual deve:

- Ser registrada.
- Ter impacto avaliado.
- Possuir novo contrato antes da implementação.

---

## Regra 6 — Validação obrigatória

Nenhuma etapa é considerada concluída apenas por compilação.

Deve existir:

- Validação técnica.
- Validação funcional.
- Revisão arquitetural.

---

## Regra 7 — Decisões importantes registradas

Decisões que alterem:

- Fluxos.
- Responsabilidades.
- Modelos de domínio.
- Dependências.

Devem ser documentadas.

---

## Regra 8 — Comunicação entre equipes

Arquitetura define:

- Direção.
- Contratos.
- Limites.

Engenharia define:

- Implementação dentro do contrato.

Product Owner define:

- Prioridade e experiência desejada.

---

## Critério de aplicação

Estas regras permanecem válidas durante todo o ciclo do Vertical Slice 2.0.

---

## Status

PV-MILESTONE-003 — Regras de governança definidas.