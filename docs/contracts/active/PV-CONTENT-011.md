# PV-CONTENT-011 — Spider Cave por dados

Status: **IMPLEMENTADO** (branch `chief/m3-content-011`) — balanceamento **PROVISÓRIO**
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 8).
Lastro:
- `DEC-20261106-09` (Spider Cave com Rat, Cave Rat, Spider, Poison Spider e Giant Spider, confirmados pelo PO; atributos provisórios);
- `DEC-20261106-08` (todo item com preço de venda ao NPC provisório);
- ADR-003 (definição única de monstro);
- ADR-037/038 (Loot Tables);
- pendências do M2 (`PV-REWARD-010`, revisão: Cave Rat sem definição, Rat Tail fora da tabela `Items`, chance sem validação).

## Escopo implementado

1. **Catálogo único** `Infrastructure/Content/SpiderCaveCatalog`: os 5 monstros, com Level, vida, perfil de combate, XP, ouro e drops, mais a composição dos encounters.
   - `MonsterLootDefinitionProvider`, `MonsterCombatProfileProvider` e o gerador de encounters **leem daqui**: uma só fonte de dados.

   | Monstro | Level | Vida | XP | Ouro | Drops |
   |---|---|---|---|---|---|
   | Rat | 1 | 20 | 10 | 1–5 | Rat Tail 100% (valores do M2 mantidos) |
   | Cave Rat | 2 | 50 | 15 | 2–8 | Cheese 50% |
   | Spider | 2 | 30 | 12 | 1–6 | — |
   | Poison Spider | 4 | 60 | 22 | 2–10 | Spider Fang 30% |
   | Giant Spider | 8 | 220 | 120 | 10–40 | Spider Silk 30%, Spider Fang 50% |

   **Tudo PROVISÓRIO**, inclusive os itens novos (Cheese, Spider Fang, Spider Silk) e os perfis de combate.
2. **Encounters:** a área (`IHuntAreaCatalog` → `SpiderCaveHuntArea`) tem 5 composições:
   - [Rat, Cave Rat, Rat];
   - [Spider, Spider];
   - [Cave Rat, Poison Spider];
   - [Poison Spider, Spider, Rat];
   - [Giant Spider].

   A composição é **sorteada pela semente do encounter**, com um gerador próprio para não consumir números do combate. O construtor antigo do `EncounterGenerator` (sem catálogo) mantém a composição fixa antiga (Regra Aditiva).
3. **Pendências do M2:**
   - o **Cave Rat** agora dá XP, ouro e loot;
   - **Rat Tail** (e os outros itens de loot, mais as 2 poções do `PV-SUPPLY-010`) estão na **tabela `Items`**;
   - a **chance de drop é validada** entre 0 e 1 (`LootDropDefinition`).
4. **Preço de venda ao NPC** em todo item que cai: Rat Tail 2, Cheese 3, Spider Fang 8, Spider Silk 25 (**PROVISÓRIO**). Também há o preço de **compra** das poções: 50.
5. **Persistência:**
   - migration **nova** `SpiderCaveItems`: coluna `Items.NpcBuyPrice` (padrão 0) mais `InsertData` dos 6 itens, com Ids fixos;
   - nenhuma migration existente foi editada;
   - `has-pending-model-changes` = sem mudanças;
   - o `ItemDefinitionRepository` lê o preço de compra;
   - o seed do Training Sword passa a procurar o item **pelo nome**, porque a tabela já vem populada.
   - **Validado numa cópia** do banco local do PO: dados antigos preservados, 6 itens inseridos. O MD5 do original não mudou.

## Mudança de comportamento (registrada)

O encounter da Hunt deixou de ser sempre [Rat, Cave Rat, Rat] e passou a ser sorteado entre os 5 da Spider Cave (`DEC-20261106-09`). Dois testes que dependiam da composição fixa foram adaptados:
- o teste de vitória passou a esperar a soma do XP **dos monstros do encounter**, lida do catálogo;
- o teste HTTP do Rat passou a escolher, de forma determinística, uma semente que sorteia o encounter com Rat.

## Testes (`SmokeTests/SpiderCaveContentTests.cs`)

- os 5 monstros confirmados, cada um com combate e loot do catálogo (nenhum usa o perfil de reserva);
- o Cave Rat dá XP e loot;
- todo item que cai tem preço de venda;
- a chance fora de 0–1 é rejeitada;
- a composição é determinística pela semente, e as 5 aparecem;
- **SQLite real:** a tabela `Items` bate com os catálogos (nome, descrição, preços), o seed do Training Sword continua funcionando e o repositório lê o preço de compra.

Resultado da solução: build com 0 avisos; **251 aprovados, 0 ignorados, 0 falhas**.

## Perguntas ao PO

1. Atributos dos 5 monstros (vida, Level, ataque e defesa), XP, ouro e chances.
2. Os itens de drop novos (nomes, quais monstros os dão) e os preços de venda.
3. A composição dos encounters da Spider Cave (quais grupos aparecem e com que frequência).
