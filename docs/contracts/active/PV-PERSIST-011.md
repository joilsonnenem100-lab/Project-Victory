# PV-PERSIST-011 — Save Strategy / Checkpoints

- **Status:** Implementado (Fila 6, branch `chief/q6-persist-011`)
- **Origem:** ordem "MODO FILA 6", item 5; M5 do `roadmap-to-playable.md`.
- **Lastro:** Handbook V3 §19, "Checkpoint Strategy": salvar no **início**, na **conclusão**, na **recompensa** e no **logout**; evitar salvar cada ação pequena. Também Handbook §17 (classes de estado) e ADR-024 (o Reward Pipeline grava a Experience).

## Antes × depois

| Momento | Antes | Depois |
|---|---|---|
| Entrar no jogo | grava a cópia do Runtime | igual |
| **Início da Hunt** | nada | **checkpoint `HuntStart`** (cópia do Runtime) |
| Rodada de combate sem morte de monstro | **gravava o personagem** se o jogador usasse poção | **não grava nada**: o consumo de poções fica **pendente** em memória (`IPendingSupplyConsumption`, estado temporário) |
| Morte de monstro | Reward Pipeline grava XP/loot | **checkpoint `Reward`** antes do pipeline (grava as poções pendentes e a cópia do Runtime), e depois o Reward Pipeline, como antes |
| **Fim da Hunt** (vitória/morte/manual/queda) | nada no banco (vida/mana só em memória) | **checkpoint `HuntConclusion`** no `EndHuntHandler` (pipeline único, ADR-012): grava as poções pendentes e a cópia do Runtime, com vida/mana já restauradas na morte |
| Logout | não existe | `CheckpointReason.Logout` pronto; **sem chamador** até existir logout ou queda (R-048, M6) |

A política de poções (`PV-PERSIST-010`) grava só quando o jogador muda a configuração e quando a pendente é ativada, uma vez por mudança. Não grava a cada rodada.

## Implementação

`src/ProjectVictory.Application/Checkpoint/CheckpointService.cs`:
- `CheckpointReason`: `HuntStart`, `Reward`, `HuntConclusion`, `Logout`.
- `IPendingSupplyConsumption` / `PendingSupplyConsumption` (singleton thread-safe): poções usadas e ainda não gravadas. Ao carregar os suprimentos de uma rodada, o pendente é **descontado** do inventário, para nunca usar duas vezes a mesma poção.
- `ICheckpointService` / `CheckpointService` (scoped): grava o consumo pendente no Inventory do personagem (uma escrita) e a cópia do Runtime pelo `IRuntimeRepository.SaveCheckpointAsync`, novo e aditivo (método padrão sem efeito para dublês; o `RuntimeRepository` do EF atualiza vida/mana).
- `ICheckpointLog` / `CheckpointLog`: registro em memória dos checkpoints, para observabilidade e testes.

Ligação:
- `ExecuteHuntHandler` faz o checkpoint `HuntStart`;
- `AttackEncounterHandler` registra o consumo pendente em vez de gravar, e faz o checkpoint `Reward` quando algum monstro morre;
- `EndHuntHandler` faz o checkpoint `HuntConclusion`.

Esses três handlers ganharam parâmetros no **fim** do construtor. Eles só são criados pela DI; nenhum código os instancia à mão. Um construtor antigo mantido seria **incorreto**, porque adiaria o consumo sem checkpoint. Nenhum endpoint, DTO ou resposta mudou.

**Banco:** nenhuma migration nova. A tabela `RuntimeStates` já tinha `Health`/`Mana`.

## Testes

- Novo `SmokeTests/SaveStrategyTests`:
  - **rodadas sem morte de monstro e sem fim de Hunt não gravam nada**, nem no personagem nem na cópia do Runtime, **inclusive rodadas em que o jogador usou poção**. Isso é contado por decorators de escrita em volta dos repositórios reais, numa luta longa (Giant Spider) com 5 poções;
  - o início registra `HuntStart` e o fim registra `HuntConclusion`;
  - ao final, o inventário tem exatamente `5 − poções usadas`, ou seja, nada se perdeu;
  - o consumo pendente acumula e é retirado uma vez só.
- **Todos os testes existentes passam sem mudança**, incluindo os de poções (SmokeTests e E2E), que conferem o inventário ao fim da Hunt.

Resultado: build com 0 avisos; **344 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 119, Runtime 17, Smoke 87, Api 90).

## Risco aceito

Se o servidor cair no meio de uma Hunt, as poções usadas desde o último checkpoint não saem do inventário (o jogador "ganha" essas poções). É o custo do §19 ("evitar salvar cada ação pequena"), e a Hunt também não é recuperada depois de reinício (ADR-057).

## Também nesta branch: ressalvas da revisão do `PV-PERSIST-010`

1. O teste de reinício **para o primeiro host** antes de subir o segundo.
2. O comentário do teste foi corrigido (o ouro não é conferido ali; Inventory e ouro já são cobertos no M2).
3. A política fica **ativa** antes do reinício e é conferida com `GetActive` depois, no SQLite real.
4. Corrida no primeiro `SetPending`: o `DbUpdateException` de inserção duplicada é tratado relendo a linha e gravando como atualização.
5. e 6. **R-047:** FK futura para `Characters` e variante assíncrona da interface. O acoplamento à unidade de trabalho está documentado no código.
7. A linha antiga do `project-state.md` sobre Runtime/Shared fica para a revisão de fechamento do M4.

## Pendências

- **R-048:** o checkpoint de logout e de queda tem de ser chamado no M6 (`PV-RUNTIME-010`/`PV-HUNT-012`).
- **R-045:** a atomicidade da Experience com vários participantes continua registrada.

## Revisão (`pv-architecture-reviewer`): APROVADO COM RESSALVAS — aplicadas na branch `chief/q6-m4-close`

1. **(Obrigatória, Server Authority)** O checkpoint de início usava o `RuntimeId` do **corpo da requisição**. Agora usa `hunt.RuntimeId`, resolvido pelo servidor. Teste: `StartCheckpoint_IgnoresTheRuntimeIdFromTheRequest`.
2. O consumo pendente só é retirado **depois** de gravado (`Peek` → grava → `Subtract`). Se a gravação falhar, o débito continua pendente.
3. O `CheckpointLog` guarda só os últimos 32 registros por personagem.
4. `IRuntimeRepository.SaveCheckpointAsync` ficou **sem corpo padrão**: toda implementação grava de fato.
5. Testes explícitos de **vitória** (checkpoints `Reward` e `HuntConclusion`, XP preservada) e de **morte** (só `HuntStart` e `HuntConclusion`; a cópia do Runtime fica com a vida restaurada). A escrita da ativação da política está comentada como fora da contagem.
6. Corrida no `SetPending`: se a linha não existir, relança a exceção **original**.
7. Textos do R-045 (alvo: Hunt em grupo) e do R-048 (origem) corrigidos.
8. **R-049:** invariante — retirar itens do inventário (venda, depot, troca) tem de considerar o consumo pendente.

Resultado: build com 0 avisos; **348 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 119, Runtime 17, Smoke 91, Api 90).
