# PV-CONTENT-012 — Conteúdo mínimo da Spider Cave: poções em três níveis, kit inicial, preços e loot (PO-13/15/16)

Origem: Fila Noturna 1 (2026-11-07), executada sem o PO (leituras PROVISÓRIAS marcadas abaixo). Item do roadmap: `PV-CONTENT-010` "Conteúdo mínimo — monstros, itens e loot suficientes para ~30 min de jogo" (`roadmap-to-playable.md`), recortado aqui para a Spider Cave. Lastro: `PO-DECISOES-BASE.md` **PO-13** (uma região no Primeiro Teste Jogável: Spider Cave), **PO-15** (poções em três níveis: pequena, média, grande), **PO-16** (poções vêm de kit inicial e recompensa diária), PO-20 (venda de loot ao NPC por preço fixo); `DEC-20261106-02` (suprimentos = poção de vida e de Mana; nota de superação: três níveis), `DEC-20261106-08` (todo item tem preço de venda ao NPC, valores PROVISÓRIOS do Arquiteto; preço é dado da definição do item; comércio fica na Fase 7/8), `DEC-20261106-09` (Spider Cave e os 5 monstros; atributos provisórios), `DEC-20261106-15` (referência Tibia), `PV-SUPPLY-010/011`, `PV-CONTENT-011`, `PV-PERSIST-011` (consumo pendente até o checkpoint), `PV-HUD-ANALYZER-010` (débito pelo preço de compra). **Só o servidor e dados**; nada de NPC, comércio, loja ou cliente.

## Escopo
- **D1 — Dados das poções (`Application/Supply/PotionCatalog.cs`, fonte única):** `PotionDefinition(Key, Item, Kind {Health, Mana}, Size {Small, Medium, Large}, Amount)`; 6 níveis. **Regra Aditiva:** Health Potion (`…a1`) e Mana Potion (`…a2`) continuam com os mesmos Ids, nomes, preços e efeitos (75/50) e passam a ser o nível **pequeno**. Novos: Strong/Great Health Potion (`…a3`, `…a4`), Strong/Great Mana Potion (`…a5`, `…a6`). `SupplyCatalog.IsPotion` reconhece os 6. `ProvisionalSupplyEffects` lê os 75/50 do catálogo (mesmos valores).
- **D2 — Escolha do nível (`PotionCatalog.Choose`) — regra PROVISÓRIA:** quando a política do jogador manda usar poção de vida/Mana (limite em % — inalterado, `DEC-20261106-03`), o seletor usa, entre os níveis que o personagem **tem**, a **menor poção que resolve** (valor ≥ o que falta para o máximo); se nenhuma resolve, a **maior disponível**. Motivo: não desperdiçar poção cara quando falta pouco e curar o máximo possível quando falta muito. A política continua com dois limites (vida e Mana); não há limite por nível.
- **D3 — Ação de combate (aditivo):** `CombatTurnChoice.PotionItemId` e `CombatTurnAction.PotionItemId` (parâmetros opcionais novos). `CombatResolver` cura/restaura pelo nível (`ISupplyEffects.HealthRestoreFor/ManaRestoreFor`, membros com corpo padrão: sem nível ou nível pequeno = valores antigos). O orquestrador só repassa o Id (ADR-051).
- **D4 — Contagem por nível:** `CombatSupplyContext` conta disponível/usado por Id do item (`Available`, `Used`); os totais antigos (`HealthPotions`, `ManaPotions`, `…Used`) e `Load(policy, h, m)` continuam (sem nível = pequena). `IPendingSupplyConsumption` ganha `AddItems/PeekItems/SubtractItems`; os métodos antigos continuam (Add/Subtract sem nível = pequena; Peek/Take = totais por tipo). `CheckpointService` remove do inventário por nível. `IHuntAnalyzerTally.RecordSupplyItems` registra o débito por nível (preço de compra de cada item).
- **D5 — Kit inicial (PO-16) por configuração:** `Supplies:StarterKit:<Key>` (Key do catálogo, maiúsculas indiferentes) lido no `Program.cs` por `SupplySettings.ParseStarterKit`; chave desconhecida, valor não inteiro ou negativo → o startup falha. As chaves antigas `Supplies:StarterHealthPotions/StarterManaPotions` continuam e **somam** ao kit (nível pequeno). `CreateCharacterHandler` entrega `SupplySettings.StarterItems()`.
- **D6 — Preços NPC (dado, não comércio):** compra/venda nas definições dos itens novos (catálogo + tabela `Items`). Loot: preços mantidos (já PROVISÓRIOS no `PV-CONTENT-011`).
- **D7 — Loot da Spider Cave:** a Spider (antes só ouro) passa a dar **Spider Fang 10%**; agora todo monstro da área dá item. Demais tabelas inalteradas.

## Balanceamento PROVISÓRIO (onde trocar)
| Valor | Atual | Onde trocar |
|---|---|---|
| Nomes dos níveis (estilo Tibia) | Health / Strong Health / Great Health Potion; Mana / Strong Mana / Great Mana Potion | `PotionCatalog` (+ migration nova para o nome na tabela `Items`) |
| Cura (vida) pequena/média/grande | 75 / 150 / 250 (Tibia ≈ 150/300/500, ÷2; 75 já validado) | `PotionCatalog.All` (`SmallHealthAmount`) |
| Mana pequena/média/grande | 50 / 75 / 100 (Tibia ≈ 100/150/200, ÷2; 50 já validado) | `PotionCatalog.All` (`SmallManaAmount`) |
| Compra NPC vida | 50 / 100 / 190 (Tibia) | `PotionCatalog` + migration nova (`Items.NpcBuyPrice`) |
| Compra NPC Mana | 50 (mantido; Tibia 56) / 93 / 144 (Tibia) | idem |
| Venda NPC vida / Mana | 10 / 20 / 38 · 10 / 18 / 28 (≈ 20% da compra; no Tibia o NPC não compra poção) | idem (`NpcSellPrice`) |
| Kit inicial | Health 30, Strong Health 10, Great Health 3, Mana 20, Strong Mana 5, Great Mana 2 | `appsettings.json` → `Supplies:StarterKit` (ou variável `Supplies__StarterKit__<Key>`) |
| Regra de escolha do nível | menor que resolve; senão a maior | `PotionCatalog.Choose` |
| Spider: Spider Fang | 10% | `SpiderCaveCatalog.Monsters` |
| Preços de venda do loot | Rat Tail 2, Cheese 3, Spider Fang 8, Spider Silk 25 (inalterados) | `SpiderCaveCatalog` + migration nova |

Sem restrição de Level/vocação por nível de poção (no Tibia as maiores exigem Level/vocação) — fora do escopo, PROVISÓRIO.

**~30 min de jogo (estimativa, não medida):** com a spell de cura como cura principal (PO-53, `PV-COMBAT-014`) e as poções como reserva, o kit (43 de vida, 27 de Mana) cobre a sessão do Primeiro Teste Jogável; a medida real fica para o teste com o PO (M8).

## Testes antigos ajustados (justificativa)
O kit inicial agora vem ligado no `appsettings.json` (comportamento real do jogo). Três testes da API contam itens/poções exatas de um personagem **sem kit** (cenário escrito antes do PO-16): `SpiderCaveEndToEndTests` (classe `Server`: `Victory_…` conta 2–3 itens de loot; `Potion_…` conta exatamente 3 poções iniciais) e `RewardFlowHttpTests.DefeatingRat_GivesExperienceAndDrop` ("exatamente 1 item"). Eles passam a subir o servidor com o kit zerado (`ApiFactory.WithoutStarterKit()`, variáveis `Supplies__StarterKit__<Key>=0`) — o mesmo cenário de antes; **nenhuma asserção foi mudada**. Os demais 453 testes antigos não mudaram (a política de poção padrão é "não usar", então o kit não altera combate).

## Migration
**Nova e aditiva:** `20261107073604_PotionTiers` — só `InsertData` das 4 poções novas na tabela `Items` (`Down` = `DeleteData`; o snapshot só ganha linhas). Indispensável: o Hunt Analyzer e `GET api/item` leem preço/nome da tabela `Items`. Gerada com `dotnet ef migrations add` (JWT aleatório só no ambiente do comando, banco temporário). Nenhuma migration existente editada.

## Fora do escopo / pendências
- **Recompensa diária de poções (PO-16): NÃO implementada** — pendência registrada no `project-state.md`.
- NPC, comércio, loja, recompra (Fases 7/8); poção como drop de monstro (PO-16 define kit + diária como fontes — pergunta ao PO); cliente (mostrar o nível usado no log/HUD; política por nível).

## Testes
- `SmokeTests/PotionTiersTests` (15): dados (3 níveis por tipo, valores e preços crescentes, compra > venda > 0, Ids/Keys únicos; poções antigas = nível pequeno inalterado; tabela `Items` com os níveis novos em SQLite real; todo monstro da Spider Cave dá item); escolha do nível (teoria com 5 casos; só entre os níveis que o personagem tem); seletor + resolver + observador por nível; `Load` antigo = pequena; pendente por nível e totais antigos; kit (parse por Key, soma das chaves antigas, rejeições); integração: personagem novo recebe o kit e a Hunt consome por nível (inventário após checkpoint e débito no Analyzer).
- `Api.Tests/StarterKitHttpTests` (2): o kit do `appsettings.json` é válido e tem todos os níveis; personagem novo pela API nasce com o kit.
- Resultado: build 0 avisos; **473 testes** verdes (456 + 15 + 2).
