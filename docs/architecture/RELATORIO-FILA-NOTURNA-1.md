# Relatório — Fila Noturna 1 (2026-11-07, sem supervisão)

Worktree `C:/pvn`. Branches empilhadas a partir da última branch da Fila 9 (`feature/pv-gov-019-game-session-handbook`): `feature/n1-hud-analyzer-010` → `n1-admin-010` → `n1-combat-014` → `n1-content-012` → `n1-legacy-001` → `n1-qa-010` (a última traz tudo). Tudo foi enviado com push, sem `--force` e sem tocar na `main`. Detalhes no anexo A e no `FILA-NOTURNA-1-STATUS.md`.

| # | Item | Status | Testes ao fim (executados) |
|---|---|---|---|
| 1–6 | PV-GOV-014…019 | ✅ feitos na Fila 9 (mesma sessão; não refeitos) | 410 |
| 7 | PV-HUD-ANALYZER-010 (servidor) | ✅ `GET api/hunt/analyzer/{id}` e `/history`; histórico das últimas 10 Hunts | 420 |
| 8 | PV-ADMIN-010 | ✅ `GET/PUT api/admin/xp-rate`, policy `Administrator`; admin **só por Id de conta** (ressalva de segurança) | 433 |
| 9 | PV-COMBAT-014 | ✅ spell de cura das 5 classes (% da vida máxima por faixa de Level, gasta Mana); seletor automático | 455 |
| 10 | PV-CONTENT-012 | ✅ poções em 3 níveis, kit inicial, preços e loot por dados; sem recompensa diária (R-068) | 473 |
| 11 | PV-LEGACY-001 | ✅ parcial: endpoint `api/gameplay/action` retirado (commit isolado `be9e094`, manifesto); cadeia interna mantida → `PV-LEGACY-002` | 480 |
| 12 | PV-QA-010 | ✅ `docs/playtest/` (roteiro, matriz de aceite, `api.http`; sem Swagger) | 480 |

**Build:** 0 avisos e 0 erros em cada item. **Testes:** 480 aprovados, 0 falhas, 0 ignorados, executados pelo Chefe a cada item. **Revisão:** `pv-architecture-reviewer` em todos os contratos, todos APROVADO COM RESSALVAS; as ressalvas foram aplicadas na branch seguinte (anexo A).
**Migrations (3, novas e aditivas; nenhuma existente editada):** `HuntAnalyzerSummaries` (tabela), `ServerExperienceRate` (tabela), `PotionTiers` (só InsertData no `Up`). O banco do PO não foi tocado.
**Cliente:** não foi tocado. Ficam pendentes a aba Analyzer, a tela de admin, a exibição da cura e os itens da UI-1.

**Provisórios e onde trocar (principais; lista completa nos contratos):**
- Cura: 10/15/20/25 % da vida máxima nos Levels 1/20/50/100, custo de 20 de Mana e uso abaixo de 50 % da vida. Ficam em `ClassHealingSpellTable` e na config `Combat:HealingSpellBelowPercent`.
- Poções: vida 75/150/250 e Mana 50/75/100. Preços: compra 50/100/190 e 50/93/144; venda 10/20/38 e 10/18/28. Ficam em `PotionCatalog`; mudar preço exige migration nova.
- Kit inicial: 30/10/3 de vida e 20/5/2 de Mana, na seção `Supplies:StarterKit` do `appsettings.json`.
- XP rate: máximo de 100x, em `ExperienceRateLimits.Maximum`.
- Histórico do Analyzer: 10 Hunts, na config `HuntAnalyzer:HistorySize`. O tempo online é a duração da Hunt.
- Loot: Spider Fang com 10 % na Spider, em `SpiderCaveCatalog`.

**Perguntas ao PO:**
1. A cura (PO-53) é uma porcentagem da **vida máxima**? O Druid cura 5 pontos a mais em cada faixa, o que é **inferência do PO-01**. Confirma?
2. Recompensa diária (PO-16, R-068): qual a frequência e o conteúdo? Poção pode cair de monstro?
3. `PV-LEGACY-002`: autoriza apagar o `ExplorationDecisionHandler`, que é código morto fora da cadeia, e ajustar o `CoreGameplayFlowSmokeTest`? O resto da cadeia já está autorizado pelo `DEC-20261107-04`.
4. Pendentes da Fila 9: o bloqueio por Continente (PO-09); gemas para slots como exceção ao "só cosméticos"; Monster AI sem R-050.
