# PV-SUPPLY-010 — Poções de vida e de mana

> **SUPERADO em parte (2026-11-06, `DEC-20261106-22`):** o item "fim de suprimento encerra a Hunt" (escopo 6 e pergunta 2) **não vale mais**. Sem poção o personagem segue sem cura até morrer, e a Hunt termina só por morte, saída manual ou queda de conexão. A implementação está no `PV-SUPPLY-011`.

Status: **IMPLEMENTADO (servidor)** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no branch `chief/m3-content-011`). Branch `chief/m3-supply-010` — valores e duas interpretações **PROVISÓRIOS**
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 7).
Lastro:
- `DEC-20261106-02`: só poção de vida e poção de mana;
- `DEC-20261106-03`: uso configurado pelo jogador em tela própria, inclusive durante a Hunt; o servidor valida e aplica a partir do próximo turno;
- `DEC-20261106-04`: sem suprimento não há cura; o fim do suprimento encerra a Hunt pelo mesmo pipeline;
- ADR-006 (a poção é a Secondary Action);
- ADR-012;
- Server Authority.

## Escopo implementado

1. **Catálogo** (`Application/Supply/SupplyCatalog`): `Health Potion` e `Mana Potion` com Ids fixos. Preço de **compra** no NPC = 50 (débito do Analyzer, `DEC-20261106-01`) e de **venda** = 10 — **PROVISÓRIOS**. `ItemDefinition.NpcBuyPrice` é novo (construtor novo, aditivo).
2. **Efeito** (`ISupplyEffects` / `ProvisionalSupplyEffects`, do `PV-COMBAT-012`): +75 HP e +50 Mana — **PROVISÓRIO**.
3. **Política do jogador** `SupplyPolicy`: "usar a poção quando a vida ou a mana ficar abaixo de X% do máximo" (1–100; `null` = não usar).
   - Endpoint **`POST api/hunt/supplies`** (`[Authorize]`):
     - o dono vem do token (`PlayerOwnershipGuard` → 403);
     - limites inválidos → **400**;
     - a resposta traz `appliesFrom: "NextTurn"`.
   - **Próximo turno:** a política fica **pendente** (`ISupplyPolicyStore`) e só passa a valer no início da próxima rodada (`AttackEncounterHandler` → `ActivatePending`), nunca no meio de um turno.
4. **No combate:**
   - `SupplyAwareCombatActionSelector` escolhe a Secondary Action do jogador nesta ordem: poção de vida (abaixo do limite e com poção) → poção de mana → habilidade da classe;
   - `SupplyConsumptionObserver` conta cada poção usada;
   - ao fim da rodada, as instâncias usadas **saem do inventário** do personagem (persistido).
5. **Sem suprimento não há cura:** não existe regeneração nem outra cura durante a Hunt. A única outra recomposição de vida é a da morte, na cidade (`knowledge-base/character.md`).
6. **Fim de suprimento** → `EndHuntHandler(OutOfSupplies)`, o **mesmo pipeline** (`PV-HUNT-010`): o encounter fica `Cancelled`, a Hunt fica `Completed`, e o jogador volta à cidade e aguarda comando.
   - **Interpretação PROVISÓRIA:** "fim de suprimento" = uma poção que a política manda usar chegou a **0** unidades.
7. **Como o jogador obtém poções — PROVISÓRIO:** a configuração do servidor `Supplies:StarterHealthPotions` / `Supplies:StarterManaPotions` dá poções na criação do personagem. O **padrão é 0**: nada muda para jogadores. Compra no NPC é Fase 7/8.

## Testes

- `SmokeTests/SupplyTests.cs`:
  - validação de limites;
  - a política pendente só vale ao ser ativada;
  - o seletor usa a poção de vida abaixo do limite **só com poção** (sem poção cai na habilidade) e usa a poção de mana;
  - o fim de suprimento só conta para poções da política;
  - **integração:** com semente fixa e 2 poções, o Knight bebe as 2, o inventário fica sem poção e a Hunt termina por **OutOfSupplies** pelo pipeline;
  - com a política ligada e 0 poções, termina na primeira rodada.
- `Api.Tests/SupplyPolicyHttpTests.cs`: 200 com `NextTurn`, 400 (0 e 150), 403 com personagem de outra conta.

Resultado da solução: build com 0 avisos; **242 aprovados, 0 ignorados, 0 falhas**.

## Perguntas ao PO

1. Cura das poções (hoje +75 HP e +50 Mana) e preços de compra e venda no NPC (hoje 50 e 10).
2. "Fim de suprimento" significa "acabou uma poção configurada" (implementado) ou outra regra (ex.: acabaram **todas**)? **Efeito hoje:** com a política de uma poção ligada e 0 unidades, a Hunt termina **já na primeira rodada**, mesmo sem dano. Como todo jogador começa com 0 poções (padrão), ligar a política sem ter poções encerra a Hunt na hora.
3. Como o jogador consegue poções antes do comércio (Fase 7/8)? Kit inicial? Drop?

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

O revisor rodou a suíte (242 aprovados). Ressalvas tratadas no branch `chief/m3-content-011`:
1. **ADR-035/036 / R-015:** o consumo de poções remove itens direto do inventário. O R-015 foi atualizado: aceito como débito, e o Inventory Service fica para antes do comércio.
2. **A "tela própria" do cliente não foi entregue**, só o endpoint. Corrigido no roadmap ("servidor implementado"); a tela é do `PV-UI-0xx` (M8).
3. A política fica **só em memória** (`SupplyPolicyStore`, singleton) e se perde ao reiniciar o servidor. Concorrência: a mesma do R-029. A persistência é do `PV-PERSIST-011` (M5).
4. O efeito "0 poções + política ligada → fim na primeira rodada" foi incluído na pergunta 2 ao PO.
5. `NpcBuyPrice` agora é **persistido e lido** (`PV-CONTENT-011`: coluna, repositório e poções na tabela `Items`). O débito do Analyzer é do `PV-HUD-ANALYZER-010` (M8).
6. Teste HTTP de 401 sem token para `POST api/hunt/supplies` acrescentado ao `AuthorizationHttpTests`. A política gravada entre rodadas e só aplicada na seguinte tem cobertura unitária (store); a de integração fica para o `PV-QA-004`.
7. A ordem dos registros na composição foi corrigida (suprimentos depois do `ThrowIfNull`).
8. O efeito das poções no `CombatResolver` da Infrastructure soma-se ao **R-028**.
