# Revisão arquitetural de fechamento do M4 (Fase 6 — Gameplay Foundation)

- **Data:** 2026-11-06 (Fila 6, item 6)
- **Responsável:** Arquiteto-Chefe (Claude), com validação do `pv-architecture-reviewer`.
- **Regra aplicada:** `roadmap.md`, "Estados dos Módulos": **Locked** = aprovado e parte da arquitetura oficial; depois disso, só muda por revisão arquitetural documentada. Este documento **é** essa revisão para a Fase 6.
- **Escopo do M4** (`roadmap-to-playable.md`): Monster AI → Experience Distribution → Spawn System. O módulo Monsters já era LOCKED.

## Resultado

| Módulo | Estado anterior | Estado novo | Motivo |
|---|---|---|---|
| Monsters | LOCKED | LOCKED | inalterado |
| **Monster AI** | PLANNED | **LOCKED** | ADR-005/006 implementados e cobertos por testes; nenhuma pendência arquitetural aberta |
| **Experience Distribution** | PLANNED | **LOCKED** | ADR-007/024 implementados e cobertos por testes; a regra de grupo é **dado/política PROVISÓRIA do PO** (troca por outra `IExperienceDistributor`, sem mudar a arquitetura) |
| **Spawn System** | PLANNED | **REVIEW** | implementado (ADR-009/010/047), mas com duas pendências que **são parte do contrato arquitetural** do `spawn.md`: descarte/despawn no fim da Hunt (R-044) e Engine Events (R-046). LOCK no M6, quando o Hunt Loop (`PV-HUNT-011`) fechar o ciclo |
| **Fase 6** | IN PROGRESS | **IN PROGRESS** | falta o LOCK do Spawn System |

## Evidências

### Monster AI — LOCKED

- **Contrato:** `docs/contracts/active/PV-MONSTER-010.md`. Revisão: APROVADO COM RESSALVAS, todas aplicadas no `PV-XP-010`.
- **ADR-005:**
  - uma única implementação, `DataDrivenMonsterAI` (`src/ProjectVictory.Application/Combat/TurnBased/MonsterAI.cs`);
  - o comportamento vem só dos dados (`MonsterBehavior`, `MonsterActionRule` em `src/ProjectVictory.Domain/Combat/Rules/MonsterBehavior.cs`; campo `Behavior` em `SpiderCaveCatalog`);
  - não há lógica por espécie.
- **ADR-006:** a IA só escolhe a Secondary Action; `BasicAttack`, poções e `ClassAbility` são recusados como regra de monstro.
- **ADR-051:** o orquestrador não mudou.
- **ADR-052:** o sorteio usa a semente do encounter; sem regras, nada é sorteado e o resultado dos combates é idêntico.
- **Testes:** `Application.Tests/MonsterAITests` (4), `Domain.Tests/MonsterBehaviorTests` (1), `SmokeTests/MonsterBehaviorDataTests` (3). Os E2E da Spider Cave não mudaram.
- **Pendência não arquitetural:** R-043 (definição neutra de habilidade de monstro, no contrato de skills de monstro, que é conteúdo).

### Experience Distribution — LOCKED

- **Contrato:** `docs/contracts/active/PV-XP-010.md`. Revisão: APROVADO COM RESSALVAS, todas aplicadas no `PV-SPAWN-010`.
- **ADR-007:** `IExperienceDistributor` (`src/ProjectVictory.Application/Reward/ExperienceDistribution.cs`) recebe só a Experience **final** (depois do XP rate) e só reparte; não calcula nem aplica bônus.
- **ADR-024:** cada parte é aplicada pelo Reward Pipeline (`RewardResolutionHandler` → `ApplyResolvedRewardToCharacterHandler`); nenhuma via nova altera Experience.
- **Regressão:** 1 participante recebe o mesmo XP de antes (`SmokeTests/ExperienceDistributionPipelineTests`, Rat = +10).
- **Testes:** `Application.Tests/ExperienceDistributionTests` (5), `SmokeTests/ExperienceDistributionPipelineTests` (2), `SmokeTests/ApplyRewardGuardTests` (2).
- **PROVISÓRIO (PO):** regra de divisão em grupo e definição de participante (`DEC-20261106-34`). **R-045** (atomicidade com vários participantes) entra com a Hunt em grupo e não muda o contrato do módulo.

### Spawn System — REVIEW

- **Contrato:** `docs/contracts/active/PV-SPAWN-010.md`. Revisão: APROVADO COM RESSALVAS, aplicadas no `PV-PERSIST-010`.
- **Implementado:**
  - stateless (`StatelessSpawnSystem`);
  - configuração vinda da Hunt area (ADR-010);
  - máquina de estados explícita, com instâncias efêmeras (`src/ProjectVictory.Domain/Spawn/SpawnInstance.cs`, ADR-047);
  - mesmo sorteio para a mesma semente.
- **Testes:** `Domain.Tests/SpawnInstanceTests` (2), `SmokeTests/SpawnSystemTests` (10 casos).
- **Por que não LOCKED:**
  - **R-044:** o encounter terminado nunca é removido e as instâncias ficam em `Active`; o ADR-009 manda descartar no fim da Hunt.
  - **R-046:** o `spawn.md` exige Engine Events.

  As duas pendências dependem do Hunt Loop contínuo (M6, `PV-HUNT-011`). Travar agora obrigaria a reabrir por revisão logo em seguida.

## Estado do código no fechamento

- Build: `dotnet build ProjectVictory.sln` com **0 avisos, 0 erros**.
- Testes: `dotnet test ProjectVictory.sln` com **344 aprovados, 0 ignorados, 0 falhas** (executados no fim da Fila 6).

## Alterações feitas por esta revisão

- `developer-guide/architecture/roadmap.md`, Fase 6: Monster AI e Experience Distribution passam a **LOCKED**; Spawn System, a **REVIEW**. A fase continua **IN PROGRESS**. Também foram atualizados "Sprint Atual" e "Próximo módulo".
- `developer-guide/architecture/project-state.md`: linha da Fase 6.

## Nota

Ressalva 7 da revisão do `PV-PERSIST-010` conferida: a linha "Projetos `ProjectVictory.Runtime` e `ProjectVictory.Shared` — Vazios" do `project-state.md` continua **correta** (os dois só têm `.csproj`).
