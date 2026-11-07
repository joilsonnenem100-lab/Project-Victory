# PV-HUD-ANALYZER-010 — Hunt Analyzer no servidor (tempo real, resumo ao fim e histórico)

Origem: Fila Noturna 1 (2026-11-07), executada sem o PO (leituras PROVISÓRIAS marcadas abaixo). Lastro: `DEC-20261106-01` (Analyzer: XP obtido, XP/hora, débito, lucro, lucro líquido; tempo real durante a Hunt e resumo ao final; **calculado no servidor**), `DEC-20261106-08` (lucro = loot × preço de venda ao NPC), PO-47 em `docs/product/PO-DECISOES-BASE.md` (histórico das últimas Hunts), `roadmap-to-playable.md` (M8, "Dependências do Hunt Analyzer": débito = poções gastas × preço de compra no NPC; XP/hora só com tempo online, ADR-057), ADR-002 (cliente só exibe), ADR-012 (pipeline único de encerramento), ADR-058 (`PublishNotificationAsync`: assinante isolado), PV-SEC-001 (posse pelo token → 403). **Só o servidor**; o cliente (aba "Analyzer") fica pendente.

## Escopo
- **D1 — Cálculo (Domain):** `ProjectVictory.Domain.Hunt.HuntAnalyzerCalculator` (puro). Entradas: XP da Hunt, segundos da Hunt, linhas de suprimento (quantidade × preço de compra), linhas de loot (quantidade × preço de venda) e ouro. Saída `HuntAnalyzerReport`.
- **D2 — Apuração da Hunt (Application, memória):** `IHuntAnalyzerTally` (singleton, por `HuntId`): poções usadas por item e loot por item + ouro. Alimentada **só por leitura** de fatos de outros módulos: o `AttackEncounterHandler` informa as poções usadas na rodada (contagem do Supply); o Reward informa o loot entregue por um observador novo (`IRewardDeliveryObserver`, definido no módulo Reward, que não conhece o Analyzer). O Analyzer não altera Combat, Reward, Supply nem Character.
- **D3 — Resumo e histórico:** assinante `HuntAnalyzerOnHuntEnded : IAsyncEventHandler<HuntEndedEvent>` (publicado pelo pipeline único, ADR-012, como notificação isolada, ADR-058) calcula o resumo, grava em `HuntAnalyzerSummaries` e mantém só as **últimas N** Hunts do personagem; descarta a apuração em memória.
- **D4 — Endpoints aditivos** (controller novo `HuntAnalyzerController`, `[Authorize]`; posse por `PlayerOwnershipGuard.EnsureCharacterAsync` → **403** para personagem de outra conta ou inexistente, padrão PV-SEC-001; identidade só do token):
  - `GET api/hunt/analyzer/{characterId}` → `HuntAnalyzerLiveResult { HasActiveHunt, HuntId?, Analyzer?, LastHunt? }`. Com Hunt em andamento: `Analyzer` em tempo real; sem Hunt: `LastHunt` = resumo da última Hunt encerrada (ou null).
  - `GET api/hunt/analyzer/{characterId}/history` → lista `HuntAnalyzerSummaryDto` (mais recente primeiro, até N).
- **DTOs:** `HuntAnalyzerDto { ExperienceGained, ElapsedSeconds, ExperiencePerHour, Debit, LootValue, Gold, Profit, NetProfit, SuppliesUsed, ItemsLooted }`; `HuntAnalyzerSummaryDto { HuntId, EndReason, EndedAt, Analyzer }`.

## Regra de cálculo
- XP obtido = `Hunt.ExperienceGained` (soma do Reward Pipeline na Hunt). XP/hora = `floor(XP × 3600 / segundos)` se segundos ≥ 1, senão 0 (mesma fórmula do `EndHuntResult`).
- Débito = Σ poções usadas × `NpcBuyPrice`. Lucro = Σ itens do loot × `NpcSellPrice` + ouro. Lucro líquido = lucro − débito (pode ser negativo). Preços lidos de `IItemDefinitionRepository` no momento do cálculo; item sem definição vale 0.

## Leituras PROVISÓRIAS (onde trocar)
1. **Tamanho do histórico = 10** — `HuntAnalyzerSettings.DefaultHistorySize` (`Application/HuntAnalyzer/HuntAnalyzer.cs`); configurável por `HuntAnalyzer:HistorySize` (Program.cs).
2. **Tempo online = duração da Hunt** (`Hunt.ElapsedAt`): ADR-057 diz que a Hunt só existe online; a queda curta (< 1 min) que não encerra a Hunt é contada. Trocar em `GetHuntAnalyzerHandler` / `HuntAnalyzerOnHuntEnded` (entrada `elapsedSeconds`).
3. **Preço do momento do cálculo** (não congelado na hora do loot) — `HuntAnalyzerPricing` (`Application/HuntAnalyzer`).
4. **Apuração em memória**: reinício do servidor perde a apuração da Hunt em curso (a Hunt já termina na queda, ADR-057/`DEC-20261106-47`); o resumo dessa Hunt fica só com XP/tempo. Trocar em `InMemoryHuntAnalyzerTally`.
5. **403 também para personagem inexistente** (não revela ids) — padrão do `PlayerOwnershipGuard`.

## Migration
Nova e aditiva: `20261107070451_HuntAnalyzerSummaries` (`CreateTable` + índice em `CharacterId, EndedAtUnixMilliseconds`; `Down` = `DropTable`; o snapshot só ganhou linhas). Gerada com `dotnet ef migrations add`, chave JWT aleatória só no ambiente do comando; nenhuma migration existente editada. Necessária: o histórico precisa sobreviver ao reinício.

## Fora do escopo
Cliente (aba "Analyzer"); preço congelado por item; consumo de suprimentos além das poções; histórico paginado.

## Testes
Domain.Tests `HuntAnalyzerCalculatorTests` (fórmulas, XP/hora com 0 s, lucro líquido negativo). SmokeTests `HuntAnalyzerSmokeTests` (apuração + resumo gravado e corte em N pelo assinante). Api.Tests `HuntAnalyzerHttpTests` (401 sem token; 403 personagem de outra conta; Analyzer durante a Hunt com XP depois de um encounter vencido; histórico depois de sair da Hunt). Resultado: build 0 avisos; 420 testes verdes (410 + 4 unidade + 2 smoke + 3 E2E + 1 caso novo da convenção de autorização dos controllers).
