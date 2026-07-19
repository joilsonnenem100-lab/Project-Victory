docs/contracts/PV-MILESTONE-003-Engineering-Communication-Protocol.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Engineering Communication Protocol

## Tipo

Engineering Process Document

---

## Objetivo

Definir o formato oficial de comunicação entre Arquitetura e Engenharia durante a implementação do Vertical Slice 2.0.

---

## Solicitação de implementação

Toda solicitação enviada para Engenharia deve conter:

- Identificação do contrato.
- Objetivo da implementação.
- Escopo permitido.
- Restrições.
- Critérios de aceite.
- Validações obrigatórias.

---

## Formato de entrega da Engenharia

A Engenharia deve retornar:

### Implementação

- Lista de arquivos alterados.
- Resumo objetivo das alterações.
- Confirmação de que o contrato foi seguido.

---

### Validação técnica

Informar:

Backend:

dotnet test ProjectVictory.sln

Resultado:

- Sucesso.
- Falha.
- Avisos encontrados.

---

Client:

npm run build

Resultado:

- Sucesso.
- Falha.
- Avisos encontrados.

---

### Validação funcional

Informar:

- Fluxo testado.
- Resultado esperado.
- Resultado obtido.
- Evidências quando necessário.

---

## Regras de comunicação

Arquitetura define:

- Contratos.
- Limites.
- Decisões técnicas.

Engenharia define:

- Implementação dentro do contrato.

Engenharia não deve:

- Criar novos sistemas.
- Alterar escopo.
- Tomar decisões de domínio sem aprovação.

---

## Processo de bloqueio

Quando encontrar impedimento:

Engenharia deve informar:

- Contrato afetado.
- Motivo do bloqueio.
- Impacto identificado.
- Decisão necessária.

---

## Critério de aprovação

Uma entrega é considerada aprovada quando:

Contrato implementado

↓

Validação técnica concluída

↓

Validação funcional concluída

↓

Arquitetura aprovada

---

## Status

PV-MILESTONE-003 — Protocolo de comunicação definido.