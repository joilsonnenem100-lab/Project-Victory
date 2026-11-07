# Matriz de aceite — Primeiro Teste Jogável pela API

Item `PV-QA-010` (Fila Noturna 1, 2026-11-07). Passos manuais em `docs/playtest/ROTEIRO-PLAYTEST.md` (número do passo entre parênteses); requisições em `docs/playtest/api.http`. Testes em `tests/ProjectVictory.Api.Tests` (API real) salvo indicação `SmokeTests`.

| # | Critério | Como verificar | Teste automatizado que já cobre | Resultado esperado |
|---|---|---|---|---|
| 1 | Conta e login públicos; token emitido | Registro e login (1–2) | `AuthorizationHttpTests` (`Account_IsPublic`), `OwnershipAndTokenHttpTests` | 200; login devolve `accountId` e `token`; senha errada 401 |
| 2 | Rotas protegidas exigem token válido | Chamar `GET api/character` sem token / com token adulterado | `AuthorizationHttpTests`, `OwnershipAndTokenHttpTests`, `ControllerAuthorizationConventionTests` | 401 |
| 3 | Recurso de outra conta é recusado (Server Authority) | Usar `characterId`/`encounterId`/`huntId` de outra conta | `AuthorizationHttpTests`, `OwnershipAndTokenHttpTests`, `RuntimeIdReuseHttpTests`, `SupplyPolicyHttpTests`, `HuntLeaveHttpTests`, `HuntAnalyzerHttpTests` | 403 |
| 4 | Personagem nasce Level 1, Novice, com kit inicial | Criar personagem; `GET api/character`; `GET api/inventory/{id}` (3) | `LevelTitleHttpTests`, `StarterKitHttpTests` | Level 1, XP 0, `title: "Novice"`, inventário com as 6 poções do kit |
| 5 | Entrar no mundo devolve o runtime | `POST api/game/enter` (4) | `SpiderCaveEndToEndTests`, `LevelTitleHttpTests` | 200 com `runtimeId` |
| 6 | Heartbeat mantém o jogador online | `POST api/game/heartbeat` (5) | `HeartbeatHttpTests` | 200, `status: "Online"` |
| 7 | Hunt na Spider Cave inicia com encounter | `POST api/hunt/execute` com `huntArea: "Spider Cave"` (6) | `SpiderCaveEndToEndTests`, `HuntLeaveHttpTests` (`ChosenHuntArea_UnknownIs400_KnownIsAccepted`) | 200 com `huntId` e `encounter`; área desconhecida 400 |
| 8 | Só uma Hunt por vez | Repetir o passo 6 com a Hunt rodando | `HuntLeaveHttpTests` (`SecondHunt_WhileOneRuns_Is409_AndTheFirstContinues`), `Fila8EndToEndTests` | 409; a primeira continua |
| 9 | Combate por turnos; monstros também atacam | Atacar (7) e ler `turns[]` | `TurnBasedCombatHttpTests`, `SpiderCaveEndToEndTests` | Turnos do jogador e dos monstros com dano |
| 10 | Vitória não encerra a Hunt (loop contínuo) | Vencer um encounter (7) | `ContinuousHuntEndToEndTests`, `SpiderCaveEndToEndTests` (`Victory_…`) | `combatState: "Victory"`, `huntEndReason: null`, `nextEncounterId` preenchido |
| 11 | XP e loot pelo Reward Pipeline; Level derivado | `GET api/character` e `GET api/inventory/{id}` após vitórias | `RewardFlowHttpTests`, `SpiderCaveEndToEndTests` | XP = soma dos monstros mortos; itens no inventário |
| 12 | Morte volta à cidade com vida cheia, nada perdido | Lutar até morrer (7) | `SpiderCaveEndToEndTests` (`Defeat_…`) | `huntEndReason: "Death"`, `returnCity: "Victoria"`, `deathNotice`; próxima Hunt com vida cheia |
| 13 | Política de poções vale do próximo turno; nível escolhido pelo servidor | `POST api/hunt/supplies` (8) e atacar | `SupplyPolicyHttpTests`, `SpiderCaveEndToEndTests` (`Potion_…`), `SmokeTests/PotionTiersTests` | 200 `appliesFrom: "NextTurn"`; turnos `UseHealthPotion` com `healing > 0`; inválido 400 |
| 14 | Sem poção a Hunt continua | Política ligada sem poções | `SpiderCaveEndToEndTests` (`NoSupplies_DoesNotEndTheHunt`) | Hunt segue até vitória do encounter ou morte |
| 15 | Cura por spell automática, prioridade sobre a poção | Atacar até a vida cair abaixo de 50 % (9) | `HealingSpellConfigurationTests`, `SmokeTests/HealingSpellCombatTests` | Turno `UseHealingSpell` com `healing > 0`, gasta Mana; sem Mana não lança |
| 16 | Analyzer em tempo real | `GET api/hunt/analyzer/{id}` com Hunt rodando (10) | `HuntAnalyzerHttpTests` (`DuringTheHunt_IsLive_…`) | `hasActiveHunt: true`, `analyzer` com XP, XP/h, débito, lucro |
| 17 | Saída manual encerra a Hunt sem perda | `POST api/hunt/leave` (11) | `HuntLeaveHttpTests`, `SpiderCaveEndToEndTests`, `ContinuousHuntEndToEndTests` | `reason: "Manual"`, `huntState: "Completed"`, `returnedToCity: true`; repetir 409 |
| 18 | Histórico do Analyzer guarda o resumo | `GET .../history` (12) | `HuntAnalyzerHttpTests` (`…AfterLeaving_HistoryHasTheSummary`) | Lista com a Hunt encerrada, mais recente primeiro |
| 19 | Logout = saída manual, sem retorno automático | `POST api/game/logout` e heartbeat (13) | `HuntLeaveHttpTests` (`Logout_…`, `HeartbeatAfterLogout_ReportsLoggedOut_UntilEnter`), `Fila8EndToEndTests` (`Logout_IsAManualExit_…`) | `returnCity: "Victoria"`; heartbeat `LoggedOut` até novo enter |
| 20 | Queda e retorno à última Hunt | Parar heartbeat > 60 s; voltar | `ContinuousHuntEndToEndTests` (`ShortDrop_…`), `Fila8EndToEndTests` | Hunt encerrada por `Disconnect`; após 10 s online, nova Hunt na última área |
| 21 | Reinício do servidor sem perda | Parar e subir a API | `RestartPersistenceHttpTests`, `RuntimeRecoveryHttpTests`, `Fila8EndToEndTests` | Conta, personagem, runtime e política de poções de volta; a Hunt não é retomada (retorno como queda) |
| 22 | Admin do XP rate restrito e efetivo | Seção 3 do roteiro | `AdminExperienceRateHttpTests` | Admin 200 (GET/PUT); comum 403; sem token 401; inválido 400; nova recompensa usa o rate novo |
| 23 | Caminho legado retirado | `POST api/gameplay/action` | `LegacyGameplayActionHttpTests` | 404 |

**Sem teste automatizado próprio (verificação só manual):** tempo de sessão de ~30 min com o kit (estimativa do `PV-CONTENT-012`); legibilidade dos valores do Analyzer para o jogador.
