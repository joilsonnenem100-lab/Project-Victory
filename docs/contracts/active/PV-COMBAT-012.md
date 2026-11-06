# PV-COMBAT-012 — Regras de dano e ação (combate real)

Status: **IMPLEMENTADO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no branch `chief/m3-hunt-010`). Branch `chief/m3-combat-012` — fórmula e atributos **PROVISÓRIOS**
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 5).
Lastro:
- ADR-006 (Basic Attack + Secondary Action);
- ADR-051 (regras fora do orquestrador);
- ADR-052;
- `knowledge-base/ascension.md` (conteúdo de Combat: dano por Level, skill e arma; defesa por Level, equipamento e shield; fórmulas oficiais **pendentes**);
- ADR-024 (recompensa só pelo Reward Pipeline).

## Escopo implementado

1. **`IDamageFormula`** (Domain/Combat/Rules) com a implementação **`ProvisionalTibiaLikeDamageFormula` — PROVISÓRIA**. Constantes em `DamageFormulaSettings.Provisional`:
   - dano máximo = floor(0,085 × skill × ataque da arma + Level/5), × multiplicador na ClassAbility;
   - dano bruto sorteado em [0, máximo];
   - armadura reduz um sorteio em [floor(armor/2), armor−1];
   - shield/defesa reduz um sorteio em [0, (defesa + Level/10)/2];
   - o dano final nunca é negativo.

   **Para trocar pela fórmula final do Tibia:** registrar outra implementação de `IDamageFormula` no `AddProjectVictoryCore`.
2. **Atributos por classe** `ClassCombatProfileTable` — **PROVISÓRIO**:
   - skill da prioridade de dano: Knight → Sword, Sorcerer → Magic Level (`ascension.md`); Paladin → Distance, Druid → Magic Level e Monk → Fist, pela mesma lógica;
   - ataque da arma, armadura e defesa de um "equipamento inicial" que ainda não existe;
   - custo e multiplicador da habilidade inicial.

   Primeira calibração: armadura 8 e defesa 10 deixavam o Knight **imune** aos Rats (o teste pela API mostrou o dano dos monstros sempre zero). Os valores foram reduzidos.
3. **Atributos dos monstros:** `IMonsterCombatProfileProvider` + `MonsterCombatProfileProvider` — **PROVISÓRIO** (Rat, Cave Rat e um perfil mínimo de reserva). O `PV-CONTENT-011` move os dados para o catálogo da Spider Cave.
4. **`CombatResolver` real** (substitui o stub atrás de `ICombatResolver`):
   - `ResolveAction`: Basic Attack e ClassAbility pela fórmula. A habilidade custa mana; sem mana **não executa** e não consome aleatoriedade.
   - Poções: cura e mana via `ISupplyEffects` (`ProvisionalSupplyEffects`: +75 HP e +50 Mana, **PROVISÓRIO**).
   - O `Resolve` antigo (atacante sempre vence; fluxo `gameplay/action`) foi **mantido** pela Regra Aditiva e registrado como **R-026**.
5. **Secondary Action padrão** `DefaultCombatActionSelector` — **PROVISÓRIO**: o jogador usa a habilidade da classe quando tem mana; os monstros não têm (Monster AI é M4).
6. **Integração no ataque real** (`AttackEncounterHandler`): cada `POST api/hunt/attack` executa uma **rodada** do orquestrador (o turno do jogador e o dos monstros, até a vez do jogador voltar).
   - Na primeira rodada é criada a `CombatSession`, com a semente do encounter (`PV-COMBAT-011`), a Vida e Mana máximas derivadas (`PV-CHAR-010`) e as atuais do runtime.
   - Cada monstro morto gera um `CombatResult` de vitória entregue ao `RewardResolutionHandler` (Reward Pipeline).
   - **Vitória = todos os monstros mortos** (antes, o primeiro monstro morto encerrava). Derrota = vida do jogador 0. O encerramento completo da Hunt na derrota é o `PV-HUNT-010`.
   - A Vida e Mana do jogador voltam para o runtime.
   - A resposta ganha `turns` (log da rodada; **aditivo**) e corrige o **R-023**: `playerMaximumHealth` agora é a Vida máxima, não a Mana.
7. **Semente fixa só em Development** (ressalva 2 do `PV-COMBAT-011`): fora de Development, `Combat:FixedSeed` é ignorado, com aviso no log.

## Ressalvas das revisões anteriores aplicadas aqui

- **`PV-COMBAT-010`:**
  - 2: regras de alvo marcadas como PROVISÓRIAS;
  - 3: `CombatRoundReport.TurnLimitReached`, com teste;
  - 4: testes de `Executed=false`, de mana insuficiente e dos observadores;
  - 1 e 6: divergências com `developer-guide/domain/combat-orchestrator.md` registradas como **R-025** ("Combat Engine" do ADR-006 = orquestrador + resolver).
- **`PV-COMBAT-011`:**
  - 1: a semente agora chega ao combate real, com teste pela API;
  - 2: semente fixa barrada fora de Development;
  - 3: loot e recompensa usam outro gerador (sem semente) → **R-024**;
  - 4: a semente 0 dos construtores antigos fica documentada como legada.

## Testes

- `Domain.Tests/DamageFormulaTests.cs`: dano máximo por Level, skill e arma; multiplicador; armadura e defesa; nunca negativo; reprodutibilidade; perfis das 5 classes.
- `SmokeTests/CombatResolverTests.cs`: resolver real, habilidade sem mana, poções, `Resolve` antigo, limite de turnos, observadores, perfis de monstro.
- `Api.Tests/TurnBasedCombatHttpTests.cs`: pela API real, os monstros causam dano ao jogador; com a **mesma semente de servidor**, dois personagens lutam **o mesmo combate, turno a turno**.
- Testes anteriores intactos, inclusive `RewardFlowHttpTests` (Rat morto → 10 XP + 1 item).

Resultado da solução: build com 0 avisos; **222 aprovados, 0 ignorados, 0 falhas**.

## Perguntas ao PO

1. Fórmula oficial de dano e defesa (`ascension.md` diz "pendente"). Até lá vale a PROVISÓRIA.
2. Atributos de combate por classe (arma, armadura e defesa iniciais, custo e força da habilidade).
3. Quem começa o combate (hoje é sempre o jogador) e a política da ação secundária do jogador (hoje: usa a habilidade sempre que tem mana).
4. **Mecânica do encounter** (revisão, ressalva 2): vitória = **todos** os monstros do encounter mortos (antes, o primeiro morto encerrava), e a recompensa é **por monstro morto**, entregue na hora. Confirmar.

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

O revisor rodou a suíte (222 aprovados). Ressalvas tratadas no branch `chief/m3-hunt-010`:
1. Pipeline de encerramento: resolvido pelo `PV-HUNT-010` (`EndHuntHandler`). A dependência morta `FinalizeHuntAfterCombatHandler` foi removida do ataque; o handler continua registrado (Regra Aditiva).
2. Vitória com todos mortos e recompensa por monstro são mecânica → **pergunta 4 ao PO** (acima).
3. O perfil de reserva de monstro sem dado vira pendência explícita do `PV-CONTENT-011`.
4. Regras de ação na Infrastructure → **R-028**.
5. Teste do bloqueio de `Combat:FixedSeed` fora de Development (`CombatSeedConfigurationTests`); derrota e XP de vários monstros testados no `PV-HUNT-010`. O aviso usa `Console.Error`, porque é emitido antes de o logger existir.
6. A resposta HTTP usa o DTO `CombatTurnDto` em vez do tipo do Domain.
7. Derrota pendente: resolvida pelo `PV-HUNT-010` (a Hunt fica `Failed`; HP e Mana cheios na cidade).
8. `CombatantFactory.cs` separado em três arquivos (`IMonsterCombatProfileProvider.cs`, `DefaultCombatActionSelector.cs`).
9. Concorrência no mesmo encounter → **R-029**.
