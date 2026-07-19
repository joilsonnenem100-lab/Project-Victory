docs/contracts/PV-MILESTONE-003-Release-Criteria.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Release Criteria

## Tipo

Release Control Document

---

## Objetivo

Definir os critérios necessários para considerar o Vertical Slice 2.0 concluído e pronto para encerramento do ciclo.

---

## Critérios funcionais

O Vertical Slice 2.0 deve permitir:

- Criar Account pelo Client.
- Realizar Login.
- Manter dados após reinício da aplicação.
- Criar e acessar Character persistente.
- Selecionar uma Hunt.
- Executar uma Hunt.
- Enfrentar um Monster.
- Resolver combate.
- Receber Reward.
- Receber Item.
- Visualizar características do Item.

---

## Critérios técnicos

Validar:

Backend:

dotnet test ProjectVictory.sln

Resultado esperado:

- Build concluído.
- Nenhuma falha.

---

Client:

npm run build

Resultado esperado:

- Build concluído.
- Nenhum erro de compilação.

---

## Critérios arquiteturais

Confirmar:

- Contratos foram respeitados.
- Não existem implementações fora do escopo.
- Responsabilidades das camadas permanecem separadas.
- Não foram criadas dependências desnecessárias.
- Sistemas futuros permanecem isolados.

---

## Critérios de qualidade

Verificar:

- Código segue padrões existentes.
- Alterações possuem rastreabilidade.
- Fluxos principais possuem validação funcional.
- Documentação correspondente está atualizada.

---

## Fora dos critérios de conclusão

Não bloquear aprovação por:

- Balanceamento final.
- Conteúdo limitado.
- Ausência de sistemas futuros.
- Falta de recursos completos de MMORPG.

---

## Aprovação final

O Vertical Slice 2.0 é considerado concluído quando:

Arquitetura

↓

Validação técnica

↓

Validação funcional

↓

Aprovação do Product Owner

---

## Status

PV-MILESTONE-003 — Critérios de release definidos.