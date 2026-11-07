# PV-RUNTIME-012 — Reinício como queda, vida cheia no retorno, EnterGame × Recovery, escala do Recovery

Origem: MODO FILA 8, item 3 (2026-11-06). Decisões: `DEC-20261106-47` (item b), `DEC-20261106-48` (item c, PROVISÓRIO). Revisões: R-055, R-056. Lastro: ADR-057 v1.2, Handbook §18, `PV-RUNTIME-011`, `knowledge-base/character.md` (LOCKED: "Morte → Cidade Principal → HP cheio → Mana cheia").

## Escopo
- **D1 (R-055):** reconstrução única `RuntimeReconstruction.FromCheckpoint` (Application/Runtime): "Victoria" (`HomeCity.Location`), Idle, vida/mana limitadas a [0, máximo]. Usada pelo Recovery e pelo `EnterGame`.
- **D2 (R-055):** o `EnterGame` **não sobrescreve** mais um contexto já em memória (reconstruído pelo Recovery ou vivo); sem ele, usa a mesma reconstrução (antes: `WorldContext` vazio e vida/mana sem limite). Personagem novo: inalterado.
- **D3 (DEC-48, PROVISÓRIO):** vida 0 no checkpoint → vida **e** Mana no máximo.
- **D4 (DEC-47):** reinício = queda. O Recovery marca como caído (`IPresenceTracker.MarkDroppedByRestart`, aditivo) quem tem "última Hunt escolhida"; o heartbeat aplica a regra da queda (nova Hunt depois de **mais** de 10 s online).
- **D5 (R-056):** personagens lidos em lote (`ICharacterRepository.GetByIdsAsync`, aditivo, com implementação padrão; uma consulta por lote de 200); ordem determinística (`GetAllAsync` por CharacterId/RuntimeId; `GetByCharacterIdAsync` pelo menor RuntimeId na ordem do banco) — Recovery e `EnterGame` escolhem o **mesmo** Runtime; os extras são contados (`SkippedDuplicates`).

## Fora do escopo
Unicidade de Runtime por personagem no schema (sem migration); `IHostedService` (o Recovery continua antes do `app.Run`, agora sem N+1); distinguir quem fez **logout** antes do reinício (hoje também recebe o retorno) → `PV-RUNTIME-013` (R-054).

## Testes
Novo `SmokeTests/RestartAsDropTests` (8): retorno após reinício (10 s não, 11 s sim, nova Hunt), sem última Hunt nada agendado, vida 0 → cheia, reinício → enter mantém "Victoria" e limites, enter sem Recovery usa a mesma reconstrução, Runtimes duplicados → o mesmo escolhido, leitura em lote, colisão retorno × início manual (ressalva 1 do `PV-HUNT-013`). Alterado (decisão b): E2E `ServerRestart_…_NoAutomaticReturn` → `ServerRestart_…_ReturnsAfter10sOnlineLikeADrop`.
