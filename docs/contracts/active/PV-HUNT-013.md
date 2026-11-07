# PV-HUNT-013 — Segunda Hunt recusada e logout como saída manual

Origem: MODO FILA 8, item 2 (2026-11-06). Decisões: `DEC-20261106-46` (item a, R-052) e `DEC-20261106-49` (item d). Lastro: ADR-012 (pipeline único), ADR-057 v1.2, `PV-HUNT-011/012`.

## Escopo
- **D1 (R-052):** `ExecuteHuntHandler` recusa iniciar Hunt quando o personagem já tem uma `Running` → `HuntAlreadyRunningException` → **409** (`ApiExceptionHandler`). A Hunt anterior **não** é encerrada.
- **D2 (atomicidade):** conferência + criação sob um portão por personagem (`IHuntStartGate`, singleton). Dois inícios simultâneos → um vence, o outro recebe 409.
- **D3 (retorno automático):** se o heartbeat de retorno colidir com um início manual, o retorno é consumido e a Hunt em andamento continua (sem 409 no heartbeat).
- **D4 (logout, item d):** já implementado no `PV-HUNT-012` (motivo `Logout` → `Completed`, sem retorno automático). Só confirmado por teste (`huntState = Completed`) e comentário.
- **D5 (R-053):** limpar o caminho legado `POST api/gameplay/action` só se coberto por teste. **Não está** (só testes de autorização 401/403) → **não limpo, registrado**. Efeito colateral do D1 no legado: o caminho passa pelo `ExecuteHunt`, então também não cria mais Hunts duplicadas (a 2ª chamada com Hunt em andamento → 409).

## Fora do escopo
Cliente (o cliente atual não tem "sair da Hunt": com uma Hunt em andamento, um novo "caçar" recebe 409 até morte, logout ou queda — requisito do UI-1); remoção de código legado.

## Testes
`DisconnectTests`: `SecondHunt_WhileOneRuns_IsRefused_AndTheFirstContinues`, `ConcurrentStarts_OnlyOneHuntIsCreated`; `Drop_EndsEveryRunningHuntOfTheCharacter` e `Logout_EndsEveryRunningHunt` passam a criar a 2ª Hunt direto no repositório (o `ExecuteHunt` não cria mais), mantendo a defesa "encerra TODAS". `HuntLeaveHttpTests`: `SecondHunt_WhileOneRuns_Is409_AndTheFirstContinues`, `Logout_CountsAsManualExit_HuntCompleted`.
