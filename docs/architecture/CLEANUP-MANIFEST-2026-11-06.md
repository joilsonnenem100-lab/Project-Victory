# Cleanup Manifest — 2026-11-06 (Fila 5)

Autorização: PO, `DEC-20261106-27` (remover atributos e stats que não existem no Tibia) e `DEC-20261106-28` (exceção formal à Regra Aditiva), além do `DEC-20261106-31` (R-038). Inventário completo: `docs/architecture/PV-GOV-010-INVENTARIO-ATRIBUTOS.md`.

Regras: cada remoção listada aqui; nenhum arquivo inteiro apagado sem estar listado; nenhuma migration existente editada.

## PV-GOV-010 — documentação (texto tachado, não apagado)

| Documento | O que saiu | Por quê |
|---|---|---|
| `developer-guide/domain/entities.md` | `AttackSpeed`, `Capacity` (lista de value objects) | `DEC-20261106-27`; sem Capacidade (`DEC-20261106-17/26`) |
| `developer-guide/domain/value-objects.md` | seções `AttackSpeed` e `Capacity` | idem |
| `docs/character e conceitos/PROJECT-VICTORY-ITEM-SYSTEM-DESIGN.md` | exemplos de afixo com `Strength` ("of Strength", "Strong III / Strength", "Strength I/II/III") | `DEC-20261106-27` |

Emendas (sem remoção): `ADR-026` v1.2, `knowledge-base/character.md`, `schema/character.md`, `knowledge-base/progression.md`, `developer-guide/domain/attributes.md`, `PV_ATTRIBUTE_SYSTEM_FOUNDATION.md`.

## PV-PROG-013 — Ascension da Fila 4 substituída pelos títulos por Level (`DEC-20261106-30`)

Base: o `DEC-20261106-30` **supera** a regra de Ascension do `DEC-20261106-19` (Fila 4). Substituições, cada uma pelo equivalente novo:

| Removido / substituído | Substituto |
|---|---|
| `src/ProjectVictory.Domain/Progression/AscensionRules.cs` (arquivo inteiro, `git rm`) | `src/ProjectVictory.Domain/Progression/LevelTitles.cs` (`LevelTitle`, `LevelTitleTable`) |
| `Character.Ascension` e `Character.AscensionUnder(rules)` | `Character.Title` e `Character.TitleUnder(table)` |
| `HuntAreaDefinition.RequiredAscension` (padrão 1); `GetAvailable(ascension)` | `HuntAreaDefinition.RequiredTitleTier` (padrão 0); `GetAvailable(tier)` |
| `SpiderCaveCatalog.RequiredAscension = 1` | `SpiderCaveCatalog.RequiredTitleTier = 0` |
| Configuração `Ascension:LevelStep` / `Ascension:Maximum` (`appsettings.json`) | Seção `LevelTitles` (lista de `Tier`, `Label`, `MinimumLevel`) |
| Parâmetro `AscensionRules? ascensionRules` de `AddProjectVictoryCore` | `ILevelTitleProvider? levelTitles` |
| Argumento `Ascension` em `AttributeCurveTable.ForClass` dentro do `Character` (sem efeito na curva) | Valor padrão |
| Testes da Ascension da Fila 4 (`AscensionTests`: 4 testes; `DerivedLevelTests`: 2 asserções; `AscensionContentTests`: 2 testes) | Testes de títulos nos limites 499/500 … 2499/2500 e Level muito alto; tier nas Hunt areas |

Nenhum DTO publicado perdeu campo: a Ascension nunca foi exposta pela API. `GetCharactersResult` e `EnterGameResult` **ganharam** `title` e `titleTier` (aditivo). Schema do banco inalterado (nenhuma migration).

## PV-CHAR-012 — atributos que não existem no Tibia (`DEC-20261106-27/28`)

| Removido | Onde | Verificação |
|---|---|---|
| Classe `Attributes` (`Strength`, `Dexterity`, `Intelligence`, `Vitality`) — **arquivo inteiro**, `git rm` | `src/ProjectVictory.Domain/Entities/Attributes.cs` | grep: usada só em `Character.cs` |
| Propriedade `Character.Attributes` e as 2 atribuições `Attributes = new Attributes()` nos construtores | `src/ProjectVictory.Domain/Entities/Character.cs` | sempre vazia (valores 0); nunca lida, persistida, exposta em DTO ou exibida |

Nada a remover em Application, Persistence (a tabela `Characters` nunca guardou esses campos → **nenhuma migration**), API (nenhum DTO os expunha), cliente (não exibia) nem testes (nenhum os usava). Teste de guarda novo: `Domain.Tests/TibiaAttributeTests` impede que voltem.

## PV-CLEAN-004 — Commands/Results órfãos (R-038) — branch `chief/q5-clean-004`, commit `844954c`

Autorização: PO, `DEC-20261106-31`. Antes da remoção, grep de cada nome em `src` e `tests` (`*.cs`, `*.csproj`, `*.ts`, `*.tsx`, `*.json`): **zero referências** fora do próprio arquivo. Build com 0 avisos; 311 testes aprovados depois. Commit isolado; reverter: `git revert 844954c`.

- `src/ProjectVictory.Application/Hunt/CancelHuntCommand.cs`
- `src/ProjectVictory.Application/Hunt/CancelHuntResult.cs`
- `src/ProjectVictory.Application/Hunt/ExecuteExistingHuntCommand.cs`
- `src/ProjectVictory.Application/Hunt/ExecuteNextHuntCommand.cs`
- `src/ProjectVictory.Application/Hunt/ExecuteNextHuntResult.cs`
- `src/ProjectVictory.Application/Hunt/ProcessHuntExecutionCommand.cs`
- `src/ProjectVictory.Application/Hunt/ProcessHuntExecutionResult.cs`
- `src/ProjectVictory.Application/Hunt/ResolveHuntResultCommand.cs`
- `src/ProjectVictory.Application/Hunt/ResolveHuntResultResult.cs`
