# PV-LEGACY-002 — Remoção dos órfãos do caminho legado (Fila 10, item 6)

Status: IMPLEMENTADO (2026-11-07) · Lastro: **PO-61** (`DEC-20261107-05`: remoção do `ExplorationDecisionHandler` autorizada) e `DEC-20261107-04` (cadeia interna do `gameplay/action`, autorizada; correção do Arquiteto no `PV-LEGACY-001`). Exceção à Regra Aditiva com decisão documentada. Commit isolado; lista no `docs/architecture/CLEANUP-MANIFEST-2026-11-06.md` (seção PV-LEGACY-002).

## O que sai
`ExplorationDecision*` (3) + o que só ele e o endpoint retirado usavam: `GameplayActionDispatcher`, `ExecuteLocationAction*`, `StartEncounterCombat*`, `BindEncounterToCombat*`, `ResolveCombat*` (15 arquivos) e os registros de DI.

## Leitura registrada
- O `CoreGameplayFlowSmokeTest` não referenciava nada do que saiu. O ajuste pedido seria tirar dele o `CreateCombatContextHandler`, mas esse handler **fica**: o teste o usa com o `ExecuteCombatActionHandler`, e esse caminho não faz parte da autorização. Por isso o teste só ganhou um comentário. A retirada do `CreateCombatContext*`/`ExecuteCombatAction*` fica como candidata a um contrato futuro (R-074), a decidir pelo Arquiteto.

## Resultado
Build sem avisos; **526 testes verdes**, sem teste removido. Sem migration.
