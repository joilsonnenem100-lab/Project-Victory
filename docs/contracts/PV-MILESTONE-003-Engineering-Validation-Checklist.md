docs/contracts/PV-MILESTONE-003-Engineering-Validation-Checklist.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Engineering Validation Checklist

## Tipo

Validation Control Document

---

## Objetivo

Definir os critérios mínimos de validação após cada implementação do Vertical Slice 2.0.

---

## Validação por etapa

Cada contrato implementado deve passar pelas seguintes verificações:

---

## Validação técnica

Backend:

Executar:

dotnet test ProjectVictory.sln

Resultado esperado:

- Build concluído.
- Testes executados sem falhas.

---

Client:

Executar:

npm run build

Resultado esperado:

- Build concluído.
- Nenhum erro de compilação.

---

## Validação funcional

Confirmar:

- Fluxo previsto pelo contrato funciona.
- Integração entre camadas permanece válida.
- Não existem regressões no Vertical Slice anterior.

---

## Validação arquitetural

Verificar:

- Código respeita o contrato aprovado.
- Não foram adicionados sistemas fora do escopo.
- Responsabilidades permanecem separadas.
- Nenhuma regra de negócio foi movida para camada incorreta.

---

## Evidências obrigatórias

Cada entrega deve apresentar:

- Arquivos alterados.
- Resultado dos testes.
- Resultado do build.
- Validação funcional realizada.
- Problemas encontrados.

---

## Critérios de reprovação

A implementação deve retornar para revisão quando:

- O contrato não foi seguido.
- Existem alterações não solicitadas.
- A validação funcional falha.
- Existe impacto não previsto em outras fundações.

---

## Aprovação

Uma etapa somente é considerada concluída quando:

Arquitetura

↓

Validação técnica

↓

Validação funcional

↓

Aprovação

---

## Status

PV-MILESTONE-003 — Checklist de validação definido.