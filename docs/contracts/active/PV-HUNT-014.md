# PV-HUNT-014 — Tier e Continente informativos + catálogo de Hunts (Fila 10, item 2)

Status: IMPLEMENTADO (2026-11-07) · Lastro: `PO-DECISOES-BASE.md` PO-09, PO-10, PO-55, **PO-56** (`DEC-20261107-05`); fecha **R-064**. Escopo: só servidor.

## O que muda
1. `ExecuteHuntHandler.ResolveArea`: sai a recusa por tier (`TitleUnder(...).Tier < RequiredTitleTier` → 400). Área **inexistente** continua 400 (`InvalidHuntAreaException`, Server Authority). `RequiredTitleTier` fica como dado informativo (aditivo, nada removido do record).
2. Retorno automático (`GameSession.cs`): o `catch (InvalidHuntAreaException)` **fica**, só para área retirada do catálogo (comentário atualizado). *Leitura registrada:* a ordem pedia remover "o tratamento no retorno automático"; o tratamento por **tier** deixou de existir com o item 1, e retirar o `catch` faria o heartbeat falhar quando uma área sai do catálogo — regressão não autorizada.
3. Continente: não há bloqueio por Continente no código (não existe o conceito); PO-56 fica só em documento.
4. **Novo (aditivo):** `GET api/hunt/catalog` (`[Authorize]`, somente leitura) → `regions[] { name, hunts[] { name, recommendedTitleTier, recommendedTitle, recommendedLevel, monsters[] { monsterId, name } } }`. Handler `GetHuntCatalogHandler` (Application); título/Level pela tabela de títulos da configuração (`ILevelTitleProvider`).
5. `HuntAreaDefinition` ganha `Region` opcional (aditivo). **PROVISÓRIO:** região da Spider Cave = "Spider Cave" (`SpiderCaveCatalog.RegionName`) até existir conteúdo de World/regiões.

## Testes (justificativa)
- `LogoutMarkAndHuntAreaTests.UnknownOrAboveTierArea_IsRefused…` → renomeado `UnknownArea_IsRefused_NoAreaMeansTheDefault`: o caso "acima do tier → 400" **contradiz o PO-55/PO-56** e saiu; o caso "inexistente → recusada" fica.
- Novo `AboveTierArea_IsAllowed_TierIsInformative_AndTheReturnUsesIt` (Novice entra em área de tier 5; retorno após queda usa a área).
- Novo `HuntCatalogHttpTests` (401 sem token; Spider Cave com recomendação e monstros sem repetição).
- `AscensionContentTests` (`GetAvailable`) mantido: o filtro continua existindo como leitura informativa.

## Resultado
Build 0 avisos; testes 483 verdes (Runtime 17, Domain 136, Application 34, Smoke 170, Api 126). Sem migration.
