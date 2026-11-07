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

## Revisão (`pv-architecture-reviewer`): APROVADO COM RESSALVAS — aplicadas/registradas na branch `chief/q7-event-010`

1. **R-055:** o `EnterGame` desfaz a reconstrução do Recovery ("Victoria" e limites). Alinhar exige contrato próprio (muda comportamento validado). A vida 0 no checkpoint também está registrada (PO).
2. O reuso de `RuntimeId` depois do reinício: o store recuperado já recusa o id; continua aberto só para Runtime órfão e `create` simultâneo.
3. e 7. **R-056:** escalabilidade do Recovery no startup e ordem com mais de um Runtime por personagem.
4. O teste agora confere também o limite da **mana** (a). A vida 0 (b) está no R-055. A falha do Recovery sem derrubar o host (c) é coberta pelo try/catch; um teste com repositório que falha fica para o contrato do R-056.
5. Comentário do `Program.cs` corrigido.
6. A pergunta "o reinício deve disparar o retorno à última Hunt?" está no `RELATORIO-FILA-7.md`. O Recovery já cumpre a condição `Contains`; só falta a presença.
