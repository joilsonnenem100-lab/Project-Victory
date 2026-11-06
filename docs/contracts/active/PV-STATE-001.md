# PV-STATE-001 — Posse e classificação do estado em memória

Status: **IMPLEMENTADO** (branch `chief/m2-state-001`)
Emitido por: Arquitetura (sessão Chefe), 2026-11-06, conforme `roadmap-to-playable.md` (M2).
Lastro:
- Handbook V3 §17 (Permanente: Account, Character; Semi-permanente: Hunt Session, Runtime; Temporário: cálculos de execução);
- `docs/architecture/master/knowledge-base/PROJECT-VICTORY-STATE-OWNERSHIP-MODEL.md` (cada estado tem um dono);
- ADR-028;
- ADR-057 (a Hunt nunca é recuperada);
- `project-state.md` (dívida "Concorrência/estado" e "`RuntimeId` escolhido pelo cliente").

*O roadmap citava "Handbook §8"; a classificação de estados está no §17.*

## Classificação dos stores

| Store | Registro | Conteúdo | Dono (STATE-OWNERSHIP-MODEL) | Classe (Handbook §17) | Onde vive |
|---|---|---|---|---|---|
| `AccountRepository` | scoped (EF) | Account | Account Aggregate | **Permanente** | SQLite |
| `CharacterRepository` | scoped (EF) | Character, Experience, Inventory (itens e ouro) | Character Aggregate | **Permanente** | SQLite |
| `ItemDefinitionRepository` | scoped (EF) | Definições de item (com preço de venda) | Conteúdo | **Permanente** (dado de conteúdo) | SQLite |
| `RuntimeRepository` | scoped (EF) | Cópia persistida do Runtime | Runtime Layer | **Semi-permanente** | SQLite (checkpoint do Runtime: M5/M6) |
| `RuntimeExecutionStateStore` | singleton | Runtime em execução + índice por personagem | Runtime Layer | **Semi-permanente** (cópia viva; reconstruída no Recovery, `PV-RUNTIME-011`) | memória (`ConcurrentDictionary`) |
| `HuntSessionRepository` | scoped (EF) | Hunt Session | Hunt Runtime | **Semi-permanente** pelo Handbook; **não recuperada** (ADR-057) — o registro persistido é histórico, nunca retomado | SQLite |
| `HuntRepository` | singleton | Hunt em execução | Hunt Runtime | **Temporário** (ADR-057) | memória |
| `EncounterStateStore` | singleton | Encounter ativo | Hunt Runtime / Combat | **Temporário** | memória (`ConcurrentDictionary`) |
| `CombatContextRepository` | singleton | Contexto de combate | Combat System | **Temporário** | memória |
| `CombatResultStore` | **scoped** | Resultado do combate entre o ataque e a recompensa | Combat System | **Temporário de uma requisição** | memória da requisição |
| `SupplyPolicyStore` → `PersistentSupplyPolicyStore` *(Fila 6, `PV-PERSIST-010`)* | scoped (EF) | Política de poções do jogador (ativa e pendente) | Jogador (configuração) | **Permanente** | SQLite (`SupplyPolicies`) |

*Singletons **sem estado mutável**, fora da classificação: `RewardSettings`, `SystemRandomSource` (`Random.Shared`), `MonsterLootDefinitionProvider`, `LocationActionCatalog`, `WorldDefinitionStore`, `WorldDefinitionCatalog` (conferido contra o `Program.cs` na revisão).*

## Escopo implementado

1. **Thread-safety:** `HuntRepository` e `CombatContextRepository` (singletons) trocam `List<>` sem sincronização por `ConcurrentDictionary`. Id repetido em `AddAsync` passa a lançar exceção, em vez de duplicar na lista.
2. **`CombatResultStore` continua *scoped*** (decisão D1). O resultado é gravado pelo `AttackEncounterHandler` e lido pelo `FinalizeHuntAfterCombatHandler` **na mesma requisição**. Um singleton guardaria todos os resultados para sempre, sem dono que os removesse.
3. **`RuntimeId` repetido:** `IRuntimeExecutionStateStore.Contains` (método novo) e `CreateRuntimeExecutionHandler` recusam um id já em uso **antes de gravar**.
   - Runtime de outro personagem → `OwnershipViolationException` → **403** (regra do `PV-SEC-001`).
   - Do mesmo personagem → erro (antes já falhava na chave primária do banco).

## Decisões do Arquiteto

- **D1 — `CombatResultStore` scoped é a classificação correta** (estado temporário de uma requisição). A dívida "estado em serviço scoped" não é defeito, enquanto o resultado não precisar sobreviver à requisição. Se o Combat Orchestrator (`PV-COMBAT-010`) precisar do resultado entre turnos, o escopo é revisto naquele contrato.
- **D2 — Sem limpeza de memória neste contrato.** Hunts, encounters e contextos de combate terminados continuam em memória até o processo reiniciar (comportamento anterior). A remoção ao fim da Hunt pertence ao ciclo Starting→Active→Ending→Finished do `PV-HUNT-011` (M6).
- **D3 — Concorrência dentro de um mesmo agregado** (ex.: dois ataques simultâneos ao mesmo encounter) **não** é tratada aqui. Os stores agora são seguros; a serialização de comandos sobre a mesma Hunt é do Hunt Loop e do Runtime Coordinator (M6).

## Testes

- `SmokeTests/InMemoryStateStoreTests.cs`: 2.000 inclusões paralelas em cada repositório, todas legíveis; id repetido recusado.
- `Api.Tests/RuntimeIdReuseHttpTests.cs`: outra conta reusando o `RuntimeId` recebe 403; o runtime continua do dono.

Também neste branch, as ressalvas da revisão do `PV-REWARD-010`:
- testes de configuração mais precisos e o novo teste de valor não numérico;
- R-015, R-016 e R-017 registradas em `project-state.md`.

Resultado da solução: build com 0 avisos; **161 aprovados, 0 ignorados, 0 falhas**.

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

1. Singletons sem estado mutável listados acima.
2. **Verificar-e-gravar não atômico:** entre `Contains` e `Store` não há trava. Dois `create` simultâneos com o mesmo id, ou um `create` depois de reiniciar a API (memória vazia), continuam protegidos só pela chave primária do banco e respondem 500, não 403. Não há sobrescrita. Registrado em `project-state.md`; solução definitiva: gerar o id no servidor ou o Recovery (`PV-RUNTIME-011`).
3. `RuntimeId` repetido do mesmo personagem responde 500 (comportamento anterior); 409 fica para um contrato de mapeamento de erros.
4. Lastro do roadmap corrigido para Handbook §17.
5. `RuntimeIdReuseHttpTests` passou a exigir 200 do dono.
6. Agregados mutáveis dentro de singletons (ex.: `ActiveEncounter._monsters`) → checklist do M6 (D3).

## Fora do escopo

- Persistência definitiva dos semi-permanentes e Save Strategy (M5).
- Recovery (`PV-RUNTIME-011`, M6).
- Logs de depuração `Console.WriteLine` no `EncounterStateStore` (ficam como estão; a troca por `ILogger` cabe em contrato de observabilidade).
