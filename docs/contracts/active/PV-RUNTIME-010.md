# PV-RUNTIME-010 — Game Session + Runtime Coordinator

- **Status:** Implementado (Fila 7, branch `chief/q7-runtime-010`)
- **Origem:** ordem "MODO FILA 7", item 1; M6 do `roadmap-to-playable.md` e a nota "Atualização 2026-11-06" do M6.
- **Lastro:**
  - **ADR-057 v1.2** e **`DEC-20261106-20`:** queda de **mais de 1 min** encerra a Hunt; no primeiro minuto ela segue. Ao voltar, o jogador cai na tela inicial; depois de **mais de 10 s** online, volta à **última Hunt escolhida** com uma **nova** Hunt.
  - Diretriz econômica: heartbeat **sem WebSocket**, presença **em memória thread-safe**, "última Hunt escolhida" como **checkpoint mínimo** do Runtime State.
  - Handbook §9–10 (Runtime Coordinator sem inflar handlers).
  - Server Authority: a identidade vem do token.

## Implementação

- `src/ProjectVictory.Application/GameSession/GameSession.cs`:
  - `GameSessionSettings`: `DisconnectTimeout` (60 s), `ReturnDelay` (10 s) e `SweepInterval` (5 s), em **configuração** (`GameSession:DisconnectTimeoutSeconds`, `GameSession:ReturnDelaySeconds`, `GameSession:SweepIntervalSeconds`).
  - `IPresenceTracker` / `PresenceTracker` (singleton, `ConcurrentDictionary`, trocas atômicas):
    - `Touch` renova a presença; depois de uma queda, começa a contagem de retorno;
    - `CollectDropped` marca e devolve quem está sem sinal há **mais** que o limite, uma vez por queda (um heartbeat concorrente cancela a marcação);
    - `IsReturnDue` diz se o personagem está online há **mais** que o atraso depois de uma queda.
  - `GameSessionCoordinator` (Runtime Coordinator): `SweepAsync` detecta as quedas e as entrega aos `IDisconnectHandler`. O encerramento da Hunt na queda é ligado no `PV-HUNT-012`.
  - `HeartbeatHandler`:
    - a identidade vem do **token** (`ICurrentPlayerContext`), e o personagem é o da conta;
    - renova a presença;
    - depois de uma queda, com **mais de 10 s** online, sem Hunt em andamento e com o Runtime carregado (o jogador já entrou na tela inicial), **inicia uma nova Hunt** na última Hunt escolhida (`ExecuteHuntHandler`). A Hunt anterior nunca é retomada (ADR-057).
- `GameActivity` (`IGameActivity`): iniciar uma Hunt e atacar também contam como presença, então uma Hunt sem heartbeat também é acompanhada.
- **Última Hunt escolhida:**
  - `IRuntimeRepository.SaveLastHuntAsync` grava a área no início de cada Hunt;
  - `RuntimeRecoveryData.LastHuntArea` (aditivo);
  - coluna `RuntimeStates.LastHuntArea`.
- `IHuntRepository` ganhou `GetRunningByCharacterIdAsync` e `RemoveAsync` (a remoção é usada no `PV-HUNT-011`).
- **API:**
  - **`POST api/game/heartbeat`** (novo, `[Authorize]`): devolve `characterId`, `status`, `huntId`, `returnedToLastHunt`, `lastHuntArea` e `encounter`;
  - `GameSessionSweepService` (BackgroundService) faz a varredura periódica;
  - os tempos vêm de configuração (`appsettings.json`, seção `GameSession`).

## Migration (NOVA e aditiva)

`*_RuntimeLastHuntArea`: só `AddColumn<string>("LastHuntArea", "RuntimeStates", nullable: true)`; o `Down` remove a coluna. Nenhuma migration existente foi editada. Foi gerada com chave JWT aleatória só no ambiente do comando e banco temporário; **o banco real do PO não foi tocado**.

## Testes

- `SmokeTests/GameSessionTests`:
  - queda só depois de **mais** de 1 min (com exatamente 1 min, a Hunt segue) e uma vez só;
  - retorno só depois de **mais** de 10 s online após uma queda (com exatamente 10 s, não volta);
  - o coordenador entrega cada queda aos handlers;
  - fluxo completo com relógio manual: a última Hunt é gravada ("Spider Cave"); queda; o primeiro heartbeat não volta à Hunt; com 10 s ainda não; com 11 s volta numa **nova** Hunt; os heartbeats seguintes não criam outra.
- `Api.Tests/HeartbeatHttpTests`: o heartbeat pela API real devolve o personagem do token.
- `AuthorizationHttpTests`: `POST api/game/heartbeat` exige token.

Resultado: build com 0 avisos; **354 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 119, Runtime 17, Smoke 95, Api 92).

## PROVISÓRIO / limites

- Hoje só existe uma área (Spider Cave). O retorno usa a área padrão do catálogo, e a área gravada serve para saber **se** houve uma Hunt.
- Os tempos (60 s, 10 s, varredura de 5 s) estão no `appsettings.json`, seção `GameSession`.
