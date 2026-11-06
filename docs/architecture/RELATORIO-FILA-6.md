# Relatório — MODO FILA 6 (M4 e M5), 2026-11-06

Ordem do PO e do Arquiteto-Chefe: "MODO FILA 6". Branches empilhadas a partir da `main` (`3bf8581`, que contém a Fila 5) e trabalho no worktree `C:/pv6`, com push de cada branch. Não houve `push --force` nem alteração na `main`. Nenhum arquivo foi apagado fora da lista, nenhum segredo foi escrito e nenhuma migration existente foi editada. **Uma** migration nova foi criada, só aditiva.

## Branches (o merge da última traz todas)

| # | Branch | Commit final | Conteúdo |
|---|---|---|---|
| 1 | `chief/q6-monster-010` | `ed1a514` | PV-MONSTER-010 |
| 2 | `chief/q6-xp-010` | `6aa7496` | PV-XP-010 + ressalvas do MONSTER-010 |
| 3 | `chief/q6-spawn-010` | `a12be50` | PV-SPAWN-010 + ressalvas do XP-010 |
| 4 | `chief/q6-persist-010` | `80378d4` | PV-PERSIST-010 + ressalvas do SPAWN-010 |
| 5 | `chief/q6-persist-011` | `b3d1428` | PV-PERSIST-011 + ressalvas do PERSIST-010 |
| 6 | `chief/q6-m4-close` | (última; contém este relatório) | Revisão de fechamento do M4 + ressalvas do PERSIST-011 e da própria revisão |

## Status

| Item | Status | Revisão |
|---|---|---|
| 1. PV-MONSTER-010 — Monster AI orientada a dados | ✅ | APROVADO COM RESSALVAS (aplicadas) |
| 2. PV-XP-010 — Experience Distribution | ✅ | APROVADO COM RESSALVAS (aplicadas) |
| 3. PV-SPAWN-010 — Spawn System stateless | ✅ | APROVADO COM RESSALVAS (aplicadas) |
| 4. PV-PERSIST-010 — estado permanente em EF Core | ✅ | APROVADO COM RESSALVAS (aplicadas; o revisor rodou a suíte) |
| 5. PV-PERSIST-011 — Save Strategy / Checkpoints | ✅ | APROVADO COM RESSALVAS; **1 obrigatória (Server Authority), corrigida** |
| 6. Revisão de fechamento do M4 | ✅ | APROVADO COM RESSALVAS; o critério dos LOCKs foi corrigido (ver abaixo) |

Nada travou nem foi pulado.

## Build e testes (executados)

| Momento | Build | Testes |
|---|---|---|
| Início (main, Fila 5) | 0 avisos | 311 |
| PV-MONSTER-010 | 0 avisos | 317 |
| PV-XP-010 | 0 avisos | 325 |
| PV-SPAWN-010 | 0 avisos | 336 |
| PV-PERSIST-010 | 0 avisos | 342 (o revisor também executou: 342) |
| PV-PERSIST-011 | 0 avisos | 344 (o revisor do M4 também executou: 344) |
| **Final** | **0 avisos, 0 erros** | **348 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 119, Runtime 17, Smoke 91, Api 90) |

**Nenhum teste existente foi alterado** na fila. Os E2E da Spider Cave, os de poção e os de determinismo passaram sem mudança.

## Migrations criadas

| Migration | Tipo | Conteúdo |
|---|---|---|
| `20261106220713_SupplyPolicies` | **nova, aditiva** | `Up`: `CreateTable("SupplyPolicies")` (política de poções do jogador, ativa e pendente; chave `CharacterId`). `Down`: `DropTable`. O snapshot mudou só nessa entidade |

Foi gerada com `dotnet ef migrations add`, usando `Jwt__Key` aleatória só no ambiente do comando (nada gravado) e a connection string apontando para um arquivo temporário. **O banco real do PO não foi tocado.** O teste de reinício usa o SQLite temporário dos testes.

## O que mudou

- **Monster AI:** uma única IA orientada a dados (ADR-005/006). O comportamento atual dos 5 monstros da Spider Cave foi migrado para dados ("só ataque básico"). Sem regras, nada é sorteado, então os resultados não mudam.
- **Experience Distribution:** reparte só a Experience final (ADR-007), e cada parte passa pelo Reward Pipeline (ADR-024). Com 1 participante, o XP é o mesmo de antes (teste de regressão).
- **Spawn System:** stateless, dirigido pela Hunt area, com a máquina de estados `Created → Spawning → Active → Despawning → Despawned` (ADR-009/010/047). Mesmo sorteio para a mesma semente.
- **Persistência:** a política de poções, configuração do jogador, saiu da memória e foi para o EF Core. O resto já seguia a classificação do PV-STATE-001. Teste de **reinício da API**: com o primeiro host parado e o segundo subindo sobre o mesmo banco de teste, voltam a conta, o personagem, o Runtime e a política (ativa).
- **Save Strategy (Handbook §19):** checkpoints no início da Hunt, na recompensa e na conclusão; o de logout está pronto, mas sem chamador (M6). Uma **rodada de combate não grava nada**, nem com poção: o consumo fica pendente e vai para o banco no próximo checkpoint. Isso está provado por teste com contadores de escrita.

## Achados dos revisores (todos aplicados ou registrados)

- **MONSTER-010:**
  - uma regra de monstro com poção gastaria a poção do jogador → regra de monstro só `UseSkill`, e o consumo de poção conta só ações do jogador;
  - teste de DI do seletor;
  - comentários;
  - validação movida para o Domain.Tests.
- **XP-010:**
  - guarda de `CharacterId` e partes 0 no `ApplyRewardHandler`;
  - teste do filtro de poção;
  - doc de domínio;
  - R-045.
- **SPAWN-010:**
  - id da instância igual ao id do monstro;
  - R-044 completado (encounters nunca removidos; instâncias abandonadas em `Active`);
  - R-046 (Engine Events);
  - `TimeProvider` explícito;
  - testes das guardas.
- **PERSIST-010:**
  - reinício com o primeiro host parado;
  - política ativa conferida após o reinício;
  - corrida no primeiro `SetPending`;
  - R-047 (FK e variante assíncrona).
- **PERSIST-011:**
  - **(obrigatória)** o checkpoint de início usava o `RuntimeId` do corpo da requisição → passou a usar o do servidor, com teste;
  - o débito pendente só sai depois de gravado;
  - log limitado;
  - método da interface sem corpo padrão;
  - testes de vitória e de morte;
  - R-049 (invariante do inventário).
- **Fechamento do M4:** o critério estava **inconsistente**. Eu tinha travado Monster AI e Experience Distribution e deixado só o Spawn em REVIEW por causa do `spawn.md`, mas os documentos de domínio dos outros dois também não estão cumpridos. Agora vale um **critério único**: os três ficam em **REVIEW**, com o que falta para cada LOCK. Também foram corrigidos a contagem de testes, o tachado no `roadmap.md` e a linha da Fase 6 no `project-state.md`.

## Fechamento do M4 (`docs/architecture/REVISAO-FECHAMENTO-M4.md`)

| Módulo | Estado | Falta para o LOCK |
|---|---|---|
| Monsters | LOCKED | — |
| Monster AI | **REVIEW** | R-050 (cooldowns, condições, pesos e contexto do `monster-ai.md`) e R-043 (habilidade de monstro neutra) |
| Experience Distribution | **REVIEW** | R-045 (participantes elegíveis e distribuição atômica) |
| Spawn System | **REVIEW** | R-044 (descarte e despawn no fim da Hunt) e R-046 (Engine Events), com o M6 |

**Nenhum módulo foi travado nesta fila.** A Fase 6 continua IN PROGRESS.

## PROVISÓRIO e onde trocar

| Item | Onde |
|---|---|
| Habilidades de monstro (hoje nenhuma) | `SpiderCaveCatalog`, campo `Behavior` (`MonsterBehavior`/`MonsterActionRule`) |
| Regra de divisão de XP em grupo (divisão igual, resto aos primeiros) | `EvenSplitExperienceDistributor`, ou outra `IExperienceDistributor` registrada na DI |
| Quem é participante de uma Hunt em grupo | `RewardResolutionHandler` (hoje é o personagem do combate) |
| Configurações de spawn do ADR-010 (Spawn Delay, Elite Chance, Pack Size) | `HuntAreaDefinition` / `SpawnContext`, quando houver conteúdo |
| Risco aceito da Save Strategy (queda do servidor perde o débito de poções desde o último checkpoint) | `DEC-20261106-37` |

## Pendências registradas na fila

R-043, R-044, R-045, R-046, R-047, R-048 (checkpoint de logout e queda no M6), R-049 (invariante do inventário) e R-050. Todas estão em `project-state.md`, seção Revisões Pendentes.

## Perguntas ao PO

1. **Habilidades de monstro:** que monstros da Spider Cave devem ter habilidade (ex.: veneno da Poison Spider)? Com que chance e efeito? Elas entram como dados (R-043/R-050).
2. **XP em grupo:** usar o *shared experience* do Tibia (com bônus por vocações diferentes) ou a divisão igual provisória? Quem conta como participante?
3. **Save Strategy:** aceita o risco de, numa queda do servidor, as poções usadas desde o último checkpoint não serem descontadas?
4. **Pendentes de filas anteriores:**
   - Critical, Dodge e Healing Power;
   - `BasicCombatAttributes` sem uso;
   - R-040 (Magic Level inicial 0);
   - nome da skill "Arcane Lance";
   - R-035 (Monk);
   - fórmula de dano;
   - poções (cura, preço e como obter);
   - balanceamento da Spider Cave.

## Notas de processo

- No modo fila, o `roadmap-to-playable.md` é marcado ✅ ao fim de cada contrato, antes da revisão. As revisões vieram depois e as ressalvas entraram na branch seguinte (ressalva 5 do MONSTER-010).
- Os contratos da fila foram **derivados da ordem do PO** e escritos junto com o código, como a ordem autoriza (ressalva 6 do MONSTER-010; CLAUDE.md: "contrato antes de código").
- Ao fim de cada contrato rodei `dotnet build-server shutdown` e conferi os processos `dotnet` (detalhe no resumo final).
