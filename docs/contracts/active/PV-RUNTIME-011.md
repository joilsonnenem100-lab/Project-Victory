# PV-RUNTIME-011 — Recovery do Runtime ao iniciar o servidor

- **Status:** Implementado (Fila 7, branch `chief/q7-runtime-011`)
- **Origem:** ordem "MODO FILA 7", item 4; M6 do `roadmap-to-playable.md`.
- **Lastro:**
  - Handbook §18 (Recovery);
  - **ADR-057** (Parte A simplificada): a Hunt Session **não** é recuperada;
  - `PV-RUNTIME-002-A`;
  - classificação do `PV-STATE-001`: o Runtime vivo é semi-permanente e é reconstruído a partir do checkpoint.

## Implementação

- `src/ProjectVictory.Application/Runtime/RuntimeRecoveryService.cs`, `RecoverAsync`. Para cada checkpoint de Runtime (`IRuntimeRepository.GetAllAsync`, novo e aditivo):
  - carrega o **personagem**; um Runtime órfão (sem personagem) é ignorado e contado;
  - reconstrói o `RuntimeExecutionContext` na **tela inicial "Victoria"** (`HomeCity.Location`), na atividade **Idle**, com vida e mana do checkpoint **limitadas ao máximo** do personagem;
  - devolve um relatório (`Recovered`, `SkippedWithoutCharacter`).
- `Program.cs`: o Recovery roda **no startup**, depois das migrations e do seed. Uma falha (ex.: schema desatualizado fora de Development) vai para o **log** e não derruba o servidor; o "entrar no jogo" continua reconstruindo o Runtime sob demanda.
- **Hunt:** as Hunts vivem só em memória (`HuntRepository`), então depois de um reinício não existe nenhuma Hunt, e nada tenta recriá-las. Ao voltar, o jogador está na tela inicial. Pela regra do `PV-RUNTIME-010`, o reinício não é uma "queda" detectada (a presença também começa vazia), então o retorno automático à última Hunt **não** acontece depois de um reinício (ver pergunta ao PO no relatório).

Nenhuma migration nova.

## Testes

- `SmokeTests/RuntimeRecoveryTests`: depois de um "reinício" (memória nova, mesmo banco), o Runtime volta com o mesmo id, Idle, na "Victoria", com a vida do checkpoint limitada ao máximo; o Runtime órfão é ignorado; **nenhuma Hunt** em andamento.
- `Api.Tests/RuntimeRecoveryHttpTests` (API real e banco de teste): o primeiro host para **no meio de uma Hunt**; o segundo sobe e recupera o Runtime no startup, sem "entrar no jogo"; não há Hunt; login e heartbeat funcionam, com `huntId` nulo.

Resultado: build com 0 avisos; **371 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 120, Runtime 17, Smoke 106, Api 97).
