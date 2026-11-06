# PV-PERSIST-010 — Estado permanente e semi-permanente em EF Core

- **Status:** Implementado (Fila 6, branch `chief/q6-persist-010`)
- **Origem:** ordem "MODO FILA 6", item 4; M5 do `roadmap-to-playable.md`.
- **Lastro:** Handbook V3 §17 (classes de estado); `STATE-OWNERSHIP-MODEL`; classificação do `PV-STATE-001`; ADR-057 (Hunt não é recuperada).

## Revisão da classificação (PV-STATE-001 + o que surgiu depois)

| Store | Classe | Onde vivia | Ação |
|---|---|---|---|
| `AccountRepository`, `CharacterRepository` (Character, Experience, Inventory: itens e ouro), `ItemDefinitionRepository` | Permanente | SQLite (EF) | Já persistidos — mantidos |
| `RuntimeRepository` (cópia do Runtime) | Semi-permanente | SQLite (EF) | Já persistido — mantido |
| `HuntSessionRepository` | Semi-permanente (histórico; não recuperada, ADR-057) | SQLite (EF) | Mantido |
| `RuntimeExecutionStateStore` | Semi-permanente (cópia **viva**; reconstruída pelo Recovery, `PV-RUNTIME-011`) | memória | Mantido em memória, como na classificação. O checkpoint persistido é o `RuntimeRepository` |
| `HuntRepository`, `EncounterStateStore`, `CombatContextRepository`, `CombatResultStore` | Temporário | memória | Mantidos em memória |
| **`SupplyPolicyStore`** (política de poções, `PV-SUPPLY-010`, **posterior** ao PV-STATE-001) | **Permanente** (configuração do jogador) | **memória (singleton)** — perdida ao reiniciar | **Persistido em EF Core** (este contrato) |

O único store que devia ser permanente e ainda vivia em memória era a política de poções. O resto já estava classificado e no lugar certo.

## Implementação

- `src/ProjectVictory.Persistence/Supply/PersistentSupplyPolicyStore.cs`:
  - entidade `PersistentSupplyPolicyState` (tabela **`SupplyPolicies`**, chave `CharacterId`): política **ativa** e **pendente**;
  - `PersistentSupplyPolicyStore : ISupplyPolicyStore`, com a mesma semântica do `DEC-20261106-03`: a pendente passa a ativa só em `ActivatePending`, no início do próximo turno.
  - **Grava** só quando o jogador muda a configuração ou quando a pendente é ativada (uma vez por mudança). **Nunca a cada turno** (`PV-PERSIST-011`).
- `ProjectVictoryDbContext`: `DbSet<PersistentSupplyPolicyState> SupplyPolicies`.
- DI: `ISupplyPolicyStore` → `PersistentSupplyPolicyStore` (**scoped**, usa o DbContext). O `SupplyPolicyStore` em memória continua existindo para os testes de unidade (aditivo).

## Migration (NOVA e aditiva)

`20261106220713_SupplyPolicies` (`src/ProjectVictory.Persistence/Database/Migrations/`):
- `Up`: só `CreateTable("SupplyPolicies")`;
- `Down`: `DropTable`;
- snapshot: só a entidade nova.

**Nenhuma migration existente foi editada.** Foi gerada com `dotnet ef migrations add` com `Jwt__Key` aleatória só no ambiente do comando (nada gravado) e com a connection string apontando para um arquivo temporário. O comando não conecta ao banco, e **o banco real do PO não foi tocado**.

## Testes

- `SmokeTests/SupplyPolicyPersistenceTests`:
  - a pendente não vale até a ativação;
  - sobrevive a um novo `DbContext`;
  - ativar sem pendente mantém a ativa;
  - a composição usa o store persistente.
- `Api.Tests/RestartPersistenceHttpTests` (**reinício**): um segundo host da API (DI, singletons e memória novos) sobe sobre o **mesmo banco SQLite de teste**. Ali voltam:
  - a conta (login);
  - o personagem (Level e Experience);
  - o Runtime persistido;
  - a política de poções.

  Inventory (itens e ouro) já era coberto pelos testes de persistência do M2.

Resultado: build com 0 avisos; **342 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 119, Runtime 17, Smoke 85, Api 90).

## Também nesta branch: ressalvas da revisão do `PV-SPAWN-010`

Ver a seção de revisão do contrato `PV-SPAWN-010`: ids de instância e monstro iguais, R-044 completado, R-046, `TimeProvider` explícito, `SpawnDefinitionId` documentado, testes das guardas do `ApplyRewardHandler` e ajustes cosméticos.

## Pendências

- Recovery do Runtime vivo a partir do checkpoint (`PV-RUNTIME-011`, M6) e "última Hunt escolhida" (M6, ADR-057 v1.2).
