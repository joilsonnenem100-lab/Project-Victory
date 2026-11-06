# PV-HUNT-012 — Encerramento por queda de conexão e logout

- **Status:** Implementado (Fila 7, branch `chief/q7-hunt-012`)
- **Origem:** ordem "MODO FILA 7", item 3; M6 do `roadmap-to-playable.md`.
- **Lastro:**
  - **ADR-012:** pipeline único de encerramento.
  - **ADR-057 v1.2** e **`DEC-20261106-20`:** queda de **mais de 1 min** encerra a Hunt; no primeiro minuto ela segue; qualquer outro logout leva à tela inicial.
  - **`DEC-20261106-04`:** nenhum ganho já obtido é perdido.
  - **`DEC-20261106-24`:** tela inicial "Victoria".
  - **Fecha o R-048** (checkpoint de logout da Fila 6).

## Implementação

`src/ProjectVictory.Application/Hunt/DisconnectHandling.cs`:
- **`DisconnectHuntHandler`** (`IDisconnectHandler`, chamado pela varredura do `GameSessionCoordinator`, `PV-RUNTIME-010`):
  - para o personagem que caiu, encerra **cada** Hunt em andamento pelo **mesmo** `EndHuntHandler`, com o motivo próprio **`Disconnect`**, segurando o **mesmo lock** do ataque e da saída manual (`IHuntLock`);
  - o jogador vai para a cidade "Victoria" (atividade Idle);
  - grava o checkpoint de **logout** (`CheckpointReason.Logout`).
- **`LogoutHandler`** + endpoint **`POST api/game/logout`** (novo, `[Authorize]`, identidade pelo token):
  - encerra a Hunt em andamento pelo pipeline único, com o motivo novo **`Logout`** (valor acrescentado ao enum, aditivo);
  - grava o checkpoint de logout;
  - **remove a presença**, para o logout não virar "queda" nem disparar o retorno automático;
  - responde `huntEnded`, `returnCity` e `ending`.
- `IRuntimeExecutionStateStore.TryGetByCharacterId` (consulta sem exceção).

Queda curta (até 60 s, inclusive) não encerra nada. Ao voltar, o jogador cai na tela inicial; depois de mais de 10 s online, o heartbeat inicia uma **nova** Hunt (`PV-RUNTIME-010`).

## Também nesta branch: ressalvas da revisão do `PV-HUNT-011`

1. A queda e o logout encerram **todas** as Hunts em andamento do personagem. **R-052:** o cliente ainda não usa `nextEncounter` e `leave`, e falta decidir se iniciar uma Hunt com outra em andamento deve ser recusado ou encerrar a anterior (pergunta ao PO).
2. **R-053:** o caminho legado `POST api/gameplay/action` (contextos de combate nunca limpos; sem o lock por Hunt). O D2/D3 vale para `api/hunt/*`.
3. **R-029** fica resolvido para `api/hunt/attack` e `leave`; o `gameplay/action` continua pendente.
4. `POST api/hunt/leave`: Hunt inexistente ou de outra conta dá **403** (sem revelar o id). Hunt já encerrada dá **409** (`HuntAlreadyFinishedException`, mapeada no `ApiExceptionHandler`). Antes era 500.
5. Testes que faltavam:
   - Domain da medição da Hunt (`HuntTimingTests`);
   - "NoCharacter";
   - retorno consumido com Hunt em andamento;
   - 403 na saída.
6. O lock é liberado nos caminhos "já terminou", para não recriar semáforo de Hunt morta.
7. Na vitória, o próximo encounter é gerado **antes** de descartar o vencido.
8. A queda pega o lock da Hunt (conferido acima).

## Testes

- `SmokeTests/DisconnectTests`:
  - queda de **61 s** encerra pelo pipeline: a Hunt sai da memória, o jogador volta à "Victoria"/Idle, há checkpoint de conclusão e de **logout**, e a XP fica;
  - queda de **45 s e de exatamente 60 s** não encerra; a Hunt segue e o ataque funciona;
  - logout encerra com o motivo `Logout`, grava o checkpoint, remove a presença e não gera retorno;
  - a queda encerra **todas** as Hunts em andamento;
  - "NoCharacter";
  - retorno consumido.
- `Domain.Tests/HuntTimingTests`.
- `Api.Tests/HuntLeaveHttpTests`: 403 para Hunt alheia ou desconhecida; a saída encerra com `Manual`; a segunda saída dá 403; logout pela API real encerra com `Logout` e "Victoria".
- `AuthorizationHttpTests`: `POST api/game/logout` exige token.
- `HuntLoopTests.LeavingTwice_TheSecondIsRejected`: a segunda saída agora é `OwnershipViolationException` (403), porque a Hunt saiu da memória. O teste é desta fila.

Resultado: build com 0 avisos; **369 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 120, Runtime 17, Smoke 105, Api 96).
