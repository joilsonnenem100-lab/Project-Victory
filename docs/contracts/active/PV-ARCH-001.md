# PV-ARCH-001 — Composição de DI única (R-014)

Status: **IMPLEMENTADO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no branch `chief/q2-platform-004`). Branch `chief/q2-arch-001`. Registro: `DEC-20261106-14`.
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 2", item 4). Lastro: R-014 (`project-state.md`); Clean Architecture (`CLAUDE.md`: a API só traduz HTTP); revisão do `PV-QA-001` parte 2.

## Problema

O `Program.cs` e `tests/ProjectVictory.SmokeTests/TestServiceCollectionExtensions.cs` registravam os mesmos serviços à mão. A cópia de teste já tinha ficado defasada duas vezes, e o `DependencyInjectionTests` validava só a cópia.

## Decisão do Arquiteto

- **D1 — Projeto novo `src/ProjectVictory.Composition`** (class library, net10.0), que referencia Domain, Application, Infrastructure e Persistence. É a raiz de composição compartilhada.
  - Infrastructure e Persistence não se referenciam, então nenhuma das duas camadas existentes enxerga todos os serviços.
  - Criar a dependência Infrastructure → Persistence (ou o contrário) acoplaria camadas que hoje são independentes.
- **D2 — `AddProjectVictoryCore(this IServiceCollection, RewardSettings)`** registra todos os serviços do jogo que a API registrava, com os **mesmos tempos de vida**. Ficam no host:
  - o `ProjectVictoryDbContext` (SQLite × InMemory);
  - o `ICurrentPlayerContext` (token × fixo);
  - JWT, CORS, controllers e o `ApiExceptionHandler`;
  - a leitura da configuração do XP rate, passada como parâmetro.

## Escopo implementado

1. `ProjectVictoryServiceCollectionExtensions.AddProjectVictoryCore` com os registros que saíram do `Program.cs`, incluindo o catálogo de mundo e o `PlayerOwnershipGuard`.
2. `Program.cs` chama o método. Nenhum registro mudou: conferido por script, o conjunto de pares (tempo de vida, serviço) do `Program.cs` antigo é igual ao do novo.
3. `TestServiceCollectionExtensions` chama o **mesmo** método e depois acrescenta:
   - dublês de teste (`TestRewardProcessor`, `FixedRandomSource`; o último registro vence);
   - a lista explícita de handlers usados **só** pelos testes (existem em `src/`, mas a API não os registra).
4. `DependencyInjectionTests` passa a validar a composição real da API.
5. `.sln`, `ProjectVictory.API.csproj` e `ProjectVictory.SmokeTests.csproj` referenciam o projeto novo.

## Prova

Build com 0 avisos. Todos os testes verdes: **167 aprovados, 0 ignorados, 0 falhas**. São os 166 anteriores (todos ainda passam) mais 1 controle novo pedido na revisão do `PV-QA-003`. O `Api.Tests` sobe a API real em Development (`ValidateOnBuild`) com a composição nova.

Neste branch também entraram as ressalvas da revisão do `PV-QA-003` (dois controles e notas no R-018).

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

O revisor conferiu por script os 54 pares (tempo de vida, serviço): idênticos. Também rodou build e testes (167 aprovados). Ressalvas aplicadas no branch `chief/q2-platform-004`:
1. `Program.cs`: `using`s sem uso removidos e linha em branco duplicada removida. O BOM da linha 1 (introduzido por engano neste contrato; o original não tinha) foi removido.
2. Separador no texto do R-018.
3. Projeto `Composition` registrado no `decision-log.md` (`DEC-20261106-14`) e na tabela de estado do `project-state.md`.
4. Comentário no `DependencyInjectionTests`: as implementações de produção sobrescritas (`RewardProcessor`, `SystemRandomSource`) são validadas pelo `ValidateOnBuild` e pelo `Api.Tests`.
5. A lista manual de handlers só de teste continua (ligada ao R-016) e está registrada.
6. Status atualizado.
