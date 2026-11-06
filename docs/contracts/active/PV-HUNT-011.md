# PV-HUNT-011 — Hunt Loop contínuo

- **Status:** Implementado (Fila 7, branch `chief/q7-hunt-011`)
- **Origem:** ordem "MODO FILA 7", item 2; M6 do `roadmap-to-playable.md`.
- **Lastro:**
  - **`DEC-20261106-22`:** a Hunt termina só por **morte, saída manual ou queda**; o fim de suprimento não encerra. **Fecha o R-036.**
  - ADR-012 (pipeline único de fim de Hunt).
  - ADR-009/047 (instâncias descartadas no fim; **fecha o R-044**).
  - ADR-057 (a Hunt nunca é retomada).
  - Checklist do `PV-STATE-001`: D2 (limpar a memória) e D3 (serializar comandos sobre o mesmo agregado).
  - Handbook §11.

## Comportamento

| Momento | Antes | Agora |
|---|---|---|
| Vitória de um encounter | encerrava a Hunt (`HuntEndReason.Victory`) e voltava à cidade | o encounter vencido é **despawnado e descartado**; o **Spawn** gera o próximo encounter; a resposta traz `nextEncounterId` e `nextEncounter` (aditivos); **a Hunt continua** |
| Morte | encerra pelo pipeline único | igual |
| Saída manual | não havia endpoint | **`POST api/hunt/leave`** (novo, `[Authorize]`, posse pelo token) → `EndHunt(Manual)` |
| Queda | — | `PV-HUNT-012` |
| Fim da Hunt (qualquer motivo) | Hunt e encounters ficavam na memória para sempre | horário de fim gravado; encounters da Hunt removidos e Spawn Instances **despawnadas**; Hunt removida da memória; lock da Hunt descartado |

**Medição (XP/hora):**
- a Hunt guarda `StartedAt`, `EndedAt` e `ExperienceGained`;
- o `EndHuntResult` ganha `elapsedSeconds`, `experienceGained` e `experiencePerHour` (aditivos);
- a resposta do ataque ganha `huntElapsedSeconds` e `huntExperienceGained`.

**Ciclo do Handbook §11** (Starting → Active → Ending → Finished), mapeado no domínio existente: `Created` → `Running` → encerramento pelo pipeline único (`Completed`/`Failed`/`Cancelled`) → descarte da memória.

## Implementação

- `Domain/Hunt/Hunt.cs`: `Start(DateTimeOffset)`, `MarkEnded`, `RecordExperience` e `ElapsedAt` (o `Start()` antigo continua).
- `Application/Hunt/HuntLoopSupport.cs`:
  - `IHuntLock` (um `SemaphoreSlim` por Hunt): ataque e saída manual pegam o lock, e a queda também (`PV-HUNT-012`);
  - `IActiveSpawnRegistry`: as Spawn Instances em uso por encounter, guardadas até o despawn. O Spawn System continua stateless.
- `EncounterGenerator` registra as instâncias do pack (construtor novo, aditivo).
- `IEncounterStateStore`: `RemoveByHunt` e `Contains`.
- `AttackEncounterHandler`: lock por Hunt; na vitória, despawn, descarte e próximo encounter; XP da rodada somada à Hunt.
- `EndHuntHandler` (pipeline único): medição e limpeza.
- `LeaveHuntHandler` + endpoint `POST api/hunt/leave`.

Nenhum endpoint, DTO ou campo foi removido; os campos novos são opcionais.

## Testes alterados (comportamento antigo → `DEC-20261106-22`)

| Teste | Mudança |
|---|---|
| `SmokeTests/HuntEndingPipelineTests.Victory_EndsHuntThroughSamePipeline_AndKeepsRewards` | **Substituído** por `Victory_OfEncounter_ContinuesTheHunt_ManualLeaveEndsThroughSamePipeline_AndKeepsRewards`: a vitória gera o próximo encounter; a saída manual encerra pelo mesmo pipeline; XP mantida |
| `SmokeTests/HuntEndingPipelineTests.Death_…` | Em vez de conferir `HuntState.Failed` no repositório, confere que a Hunt e o encounter **saíram da memória** (D2) |
| `SmokeTests/SupplyTests.Potions_AreConsumedFromInventory_AndRunningOutDoesNotEndTheHunt` | O fim do encounter é vitória (Hunt **segue**, sem motivo de fim) ou morte |
| `SmokeTests/SaveStrategyTests.Victory_RewardAndConclusionCheckpoints_ExperienceKept` | A conclusão vem da **saída manual** depois da vitória do encounter |
| `Api.Tests/SpiderCaveEndToEndTests.Victory_RewardsByMonster_…` | A vitória não encerra a Hunt (`nextEncounterId` presente); `POST api/hunt/leave` encerra com `Manual`, volta à cidade e mantém a XP |
| `Api.Tests/SpiderCaveEndToEndTests.Potion_PolicyAppliesFromNextTurn_…` | Desfecho conferido no `combatState` do encounter; a Hunt segue |

## Testes novos

- `SmokeTests/HuntLoopTests`:
  - **três encounters seguidos** na mesma Hunt, cada um gerado ao vencer o anterior;
  - só o encounter em andamento fica na memória;
  - saída depois de 1 h dá `elapsedSeconds = 3600` e XP/hora igual à XP;
  - memória limpa (encounters, instâncias, Hunt e lock);
  - sair duas vezes falha;
  - atacar depois de sair falha;
  - dois ataques simultâneos são serializados (sem turno repetido).
- `AuthorizationHttpTests`: `POST api/hunt/leave` exige token.

## Também nesta branch: ressalvas da revisão do `PV-RUNTIME-010`

1. `TryConsumeReturn` atômico: dois heartbeats simultâneos não criam duas Hunts (teste paralelo).
2. Se já há Hunt em andamento quando o retorno vence, o retorno é consumido.
3. Heartbeat sem personagem responde `status = "NoCharacter"` (antes era 500).
4. e 7. **R-051:** a área gravada e retomada é sempre a padrão, e o início faz duas gravações; isso será revisto quando houver a 2ª área.
5. `IHuntRepository.RemoveAsync` passa a ser usado e testado aqui.
6. `IPresenceTracker.Remove` entra com o logout no `PV-HUNT-012`.
8. `using` no `GameController`. O arquivo `GameSession.cs` será dividido quando for mexido de novo.

Resultado: build com 0 avisos; **359 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 119, Runtime 17, Smoke 99, Api 93).

## Revisão (`pv-architecture-reviewer`): APROVADO COM RESSALVAS — aplicadas na branch `chief/q7-hunt-012`

Ver a seção "ressalvas da revisão do `PV-HUNT-011`" no contrato `PV-HUNT-012` (R-052, R-053, R-029, 403/409 na saída, testes, lock, ordem da geração do próximo encounter).
