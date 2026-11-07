# PV-RUNTIME-013 — Última área escolhida de verdade (R-051) e heartbeat depois do logout (R-054)

Origem: MODO FILA 8, item 4 (2026-11-06). Lastro: ADR-057 v1.2 (última Hunt escolhida), `DEC-20261106-20/47/49`, `DEC-20261106-30` (tier das áreas), revisões R-051, R-054 (e R-041).

## Escopo
- **D1 (R-051):** `ExecuteHuntCommand.HuntArea` (opcional, aditivo; corpo `huntArea` de `POST api/hunt/execute`). Ausente → área padrão (comportamento anterior). O servidor valida: nome do catálogo (sem diferenciar maiúsculas) e tier do título ≥ `RequiredTitleTier` (fecha o R-041) → senão `InvalidHuntAreaException` → **400**. Grava o **nome do catálogo** escolhido como última Hunt.
- **D2 (R-051):** o retorno automático (queda/reinício) inicia a Hunt **na área guardada**; se ela deixou de existir/estar disponível, o retorno é consumido sem Hunt.
- **D3 (R-054):** marca de **deslogado** (`LoggedOutMarker`): o logout tira a presença e marca o personagem; enquanto marcado, o heartbeat responde `status = "LoggedOut"` e **não** recria a presença (fechar a página não vira queda). "Entrar no jogo" ou iniciar Hunt encerram a marca.
- **D4 (R-054 × DEC-47):** a marca é gravada no checkpoint do Runtime (coluna nova `LoggedOut`); o Recovery mantém deslogado e **não** agenda retorno para quem fez logout antes do reinício (DEC-49 prevalece).
- **Migration nova e aditiva:** `20261107024024_RuntimeLoggedOut` — `AddColumn<bool>("LoggedOut", "RuntimeStates", nullable: false, defaultValue: false)`; o `Down` remove a coluna. Gerada com `dotnet ef migrations add`, chave JWT aleatória só no ambiente do comando e banco temporário. O schema exige: a marca precisa sobreviver ao reinício.

## Fora do escopo
Geração de encounters por área (só existe a Spider Cave: a Hunt não guarda a área e o Hunt Loop usa o conteúdo padrão — fazer junto com a 2ª área); juntar as duas gravações do início (última Hunt + checkpoint `HuntStart`); cliente (seletor de área; parar o heartbeat no logout continua recomendado).

## Testes
Novo `SmokeTests/LogoutMarkAndHuntAreaTests` (5): heartbeat pós-logout ignorado e sem queda/retorno; iniciar Hunt limpa a marca; logout antes do reinício → sem retorno, "entrar" limpa a marca gravada; área escolhida gravada e usada no retorno; área inexistente/acima do tier recusada, sem área → padrão. `Api.Tests/HuntLeaveHttpTests`: área inexistente → 400; heartbeat pós-logout → `LoggedOut` até o `enter`.
