# PV-SPAWN-010 — Spawn System stateless, dirigido pela Hunt, com ciclo de vida

- **Status:** Implementado (Fila 6, branch `chief/q6-spawn-010`)
- **Origem:** ordem "MODO FILA 6", item 3; M4 do `roadmap-to-playable.md`.
- **Lastro:**
  - **ADR-009:** stateless; nenhum pack persistido; packs criados sob demanda; instâncias descartadas.
  - **ADR-010:** cada Hunt define a sua configuração de spawn e o Spawn System só a interpreta.
  - **ADR-047:** máquina de estados explícita; instância efêmera, nunca persistida, sem regra de domínio.
  - `developer-guide/domain/spawn.md`: Spawn Context imutável, Spawn Result como DTO, estados Created/Spawning/Active/Despawning/Despawned.
- **Fora de escopo:** o Hunt Loop contínuo (M6, R-036) e qualquer configuração nova de spawn (Spawn Delay, Elite Chance, Pack Size variável). Nenhuma mecânica nova.

## Implementação

- **Domain:** `Spawn/SpawnInstance.cs`.
  - `SpawnState` e `SpawnInstance`, com `SpawnInstanceId`, `SpawnDefinitionId` (a definição da entidade, hoje o MonsterId), `CurrentState`, `SpawnedAt` e `DespawnedAt`.
  - Transições só na ordem `Created → Spawning → Active → Despawning → Despawned`; fora da ordem dá `InvalidOperationException`.
- **Application:** `Spawn/SpawnSystem.cs`.
  - `SpawnContext(HuntId, HuntArea, Seed)` é imutável. A **configuração de spawn é a da Hunt area** (packs do Monster Pool).
  - `SpawnResult(Instances, Monsters)` é um DTO temporário.
  - `ISpawnSystem` e `StatelessSpawnSystem` (singleton e sem estado):
    - `SpawnPack` escolhe o pack com um gerador **próprio** da semente do encounter. É o mesmo sorteio que ficava no `EncounterGenerator`, então a composição é idêntica para a mesma semente e nenhum número do combate é consumido. Cria as instâncias e as leva até `Active`.
    - `Despawn` leva as instâncias de `Active` a `Despawned`.
- **`EncounterGenerator`:** com catálogo, os monstros vêm do `ISpawnSystem`. O sorteio interno (`FromArea`) foi substituído pela chamada ao Spawn System; o resultado é o mesmo. O construtor novo recebe o `ISpawnSystem`. Os antigos continuam: sem catálogo, a composição fixa antiga; só com catálogo, usam o `StatelessSpawnSystem`.
- **DI:** `ISpawnSystem` → `StatelessSpawnSystem` (singleton); o `EncounterGenerator` recebe o catálogo e o Spawn System.
- **Instâncias efêmeras:** o encounter guarda só o estado dos monstros (`EncounterMonsterState`). As Spawn Instances não são guardadas nem persistidas.

## Comportamento externo

O mesmo de antes: mesma semente dá a mesma composição, a mesma vida e a mesma semente de combate. **Nenhum teste existente mudou** (E2E da Spider Cave, `SpiderCaveContentTests`, determinismo).

## Testes

- `Domain.Tests/SpawnInstanceTests`: ciclo completo com horários; transições fora da ordem rejeitadas; ids obrigatórios.
- `SmokeTests/SpawnSystemTests`:
  - mesma semente dá o mesmo pack do gerador antigo (5 sementes), com instâncias por monstro;
  - pack `Active` e depois `Despawned`, sem estado no sistema;
  - contexto inválido rejeitado;
  - a composição real usa o Spawn System;
  - a poção "usada" por monstro não sai do inventário (ressalva do PV-XP-010).

Resultado: build com 0 avisos; **336 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 119, Runtime 17, Smoke 80, Api 89).

## Pendências

- **R-044:** o `ActiveEncounter` terminado continua no `EncounterStateStore` depois do fim da Hunt. É ele que responde "Combat already finished" a um ataque repetido, e descartá-lo mudaria a resposta externa. Fica para o Hunt Loop contínuo (`PV-HUNT-011`, M6), que criará e descartará encounters (chamando `Despawn`).
- Configurações de spawn do ADR-010 ainda não usadas (Spawn Delay, Elite Chance, Pack Size): entram com conteúdo, por decisão do PO.

## Também nesta branch: ressalvas da revisão do `PV-XP-010`

1. `project-state.md`: o status da Fase 6 diz que Monster AI, Experience Distribution e Spawn foram implementados por contrato, e que o LOCK só vem pela revisão do M4.
2. `ApplyRewardHandler`: uma parte de outro personagem dá erro (o handler serve a 1 participante).
3. `ApplyRewardHandler`: partes de valor 0 são ignoradas.
4. Teste do filtro de poção do monstro no observer.
5. `developer-guide/domain/experience-distribution.md`: seção "Implementação atual (PV-XP-010)"; o documento continua DRAFT.
6. **R-045:** atomicidade da Experience com vários participantes.

## Revisão (`pv-architecture-reviewer`): APROVADO COM RESSALVAS — aplicadas na branch `chief/q6-persist-010`

1. O `EncounterMonsterState` usa o **mesmo id** da Spawn Instance (teste `SpawnInstanceId_IsTheEncounterMonsterId`).
2. e 4. O R-044 foi completado: o encounter nunca é removido (crescimento sem limite, anterior a este contrato), e as instâncias ficam em `Active` até o `PV-HUNT-011`.
3. **R-046:** os Engine Events de spawn ficam para o M6 / contrato de eventos (`spawn.md` é DRAFT).
5. O `TimeProvider.System` foi registrado explicitamente (`TryAddSingleton`); o `StatelessSpawnSystem` é criado por factory. Há teste com relógio fixo (`SpawnedAt`/`DespawnedAt`).
6. **`SpawnDefinitionId`:** hoje é o id da **definição do monstro** (MonsterId do catálogo). A "Spawn Definition versionada" do `spawn.md` fica para o conteúdo/M6.
7. Testes das guardas do `ApplyRewardHandler`: parte de outro personagem dá erro; parte 0 não altera nada.
8. `EncounterGenerator` com `using` e sem instância dupla do Spawn System.
