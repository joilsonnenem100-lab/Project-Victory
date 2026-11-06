# PV-COMBAT-013 — 4 skills por classe em dados, seletor automático, dano extensível

- **Status:** Implementado (Fila 4, branch `chief/q4-combat-013`)
- **Origem:** ordem do PO "MODO FILA 4 - VERSÃO FINAL", item 5; `DEC-20261106-23` (skills) e `DEC-20261106-16` (combate simples).
- **ADRs:** ADR-006 (ataque básico + ação secundária), ADR-051 (Combat Orchestrator), ADR-052 (turnos determinísticos), ADR-045 (orquestrador não implementa regra de outro domínio).
- **Regra aditiva:** o enum `CombatActionKind` ganha `UseSkill`; `ClassAbility` continua existindo (sem uso pelo seletor padrão). DTO e log do turno ganham o campo opcional `SkillName`; `DamageType` existe em `SkillDefinition` e em `CombatActionOutcome` (ainda não vai para o log nem para o DTO — entra quando houver mais de um tipo).

## Escopo

1. **Skills como dados** — `src/ProjectVictory.Domain/Combat/Rules/ClassSkills.cs`:
   - `SkillDefinition(Id, Name, Class, Targeting, ManaCost, DamageMultiplier, UnlockLevel, Scaling, DamageType = Physical)`;
   - `ClassSkillTable.All`: 4 skills por classe (2 em área, 2 em alvo único), desbloqueio por Level (1, 5, 15, 20);
   - `ClassSkillTable.BasicAttackMultiplier(class)`: força do ataque básico por classe.
2. **Seletor automático** — `SkillSelectionPolicy(MinimumEnemiesForArea = 2)`: com 2+ inimigos vivos usa a melhor skill em área que o mana paga; com 1 inimigo, a melhor de alvo único; sem mana ou sem skill do tipo certo, nenhuma (só o ataque básico). Ligado ao `DefaultCombatActionSelector`; o `SupplyAwareCombatActionSelector` mantém a prioridade da poção (`DEC-20261106-22`, próximo turno) e, sem poção, delega a escolha.
3. **Orquestrador** — `ICombatActionSelector.SelectSecondaryChoice` (método padrão de interface, aditivo) devolve `CombatTurnChoice(Kind, Skill)`. Skill em área atinge todos os inimigos vivos e gasta o mana **uma vez**. O log registra `SkillName`. *Revisão (B1/B2):* a regra da área está no resolver (`ICombatResolver.ResolveSkill`, aditivo); o orquestrador só cobra o custo único devolvido e aplica os resultados.
4. **Resolver** — `CombatResolver` trata `UseSkill`: confere o mana **antes** de consumir número aleatório (determinismo), usa o multiplicador e o `DamageType` da skill.
5. **Dano extensível** — `DamageType { Physical }`, carregado em `SkillDefinition` e em `CombatActionOutcome`. Novos tipos (Fire, Ice, Holy...) entram como valores novos do enum e dados; resistências ficam para contrato próprio.
6. **Fórmula** — `IDamageFormula.RollDamage(attacker, defender, multiplier, random)` e `MaximumDamage(attacker, multiplier)`; o ataque básico usa `CombatProfile.BasicAttackMultiplier` (padrão 1).

## Tabela (*** PROVISÓRIA ***)

| Classe | Básico | Área (L1 / L15) | Alvo único (L5 / L20) | Escala |
|---|---|---|---|---|
| Knight | 1.3 | Whirl Strike 0.8×/10 · Ground Shaker 1.0×/20 | Brutal Blow 1.4×/15 · Fierce Thrust 1.7×/25 | Weapon |
| Paladin | 1.6 | Arrow Rain 1.2×/15 · Piercing Volley 1.4×/25 | Aimed Shot 1.3×/15 · Radiant Shot 1.5×/25 | Distance |
| Sorcerer | 1.0 | Flame Wave 1.6×/20 · Fire Storm 2.0×/35 | Spark Bolt 1.9×/15 · Arcane Lance 2.4×/30 | MagicLevel |
| Druid | 1.0 | Frost Wave 1.2×/20 · Stone Rain 1.5×/35 | Frost Shard 1.4×/15 · Thorn Lance 1.7×/30 | MagicLevel |
| Monk | 1.1 | Sweeping Kick 1.1×/15 · Whirlwind Palm 1.3×/25 | Power Punch 1.4×/15 · Chi Strike 1.7×/25 | FistFighting |

(multiplicador × / custo de mana). Regras do PO refletidas nos dados e testadas: Knight com área de dano baixo e alvo único médio, básico forte; Paladin com o básico mais forte; Sorcerer com o maior dano médio. **Nomes, números, níveis de desbloqueio e a política** são PROVISÓRIOS (nomes genéricos, estilo Tibia, `DEC-20261106-15`). Onde trocar: `ClassSkillTable` e `SkillSelectionPolicy` (`ClassSkills.cs`).

Todo dano de skill é `Physical` hoje (o PO não definiu elementos). `SkillScaling` está registrado em dados mas **ainda não altera a conta**: a fórmula provisória usa o perfil da classe; ligar o scaling a Magic Level/Distance/Fist é parte do contrato de skills de personagem (Fase de Skills).

## Testes

- Novo `Application.Tests/ClassSkillTests.cs`: 2+2 skills por classe; regras do PO nos dados; desbloqueio por Level; política (área com 2+, alvo único com 1, nenhuma sem mana); área atinge todos e gasta mana uma vez; alvo único com 1 inimigo; determinismo da fórmula real com a mesma semente; multiplicador escala o dano.
- `Application.Tests.csproj` passa a referenciar Infrastructure (para o teste de determinismo com o `CombatResolver` real).
- **Asserção alterada:** `SmokeTests/SupplyTests` — o seletor sem poção não devolve mais `ClassAbility`; devolve `null` (sem skill) ou `UseSkill` ("Brutal Blow" para Knight Level 5 contra 1 inimigo).

## Ressalvas de revisões anteriores aplicadas nesta branch

- **PV-PROG-012:** #2 `AscensionRules` com propriedades só-leitura (a validação não é contornável por `with`); #3 `ExperienceCurve` divide por 3 antes de multiplicar por 50 — Level máximo suportado 569.256 → **821.009** (teste ajustado); #5 comentário do `ProgressionProcessor`. #1 vira **R-037**; #4 registrado no contrato `PV-PROG-012`.
- **PV-SUPPLY-011:** #2 a localização inicial do mundo semeado passa a se chamar `HomeCity.Name` ("Victoria"); #3 `NoSupplies_DoesNotEndTheHunt` e `PolicyWithoutPotions_DoesNotEndTheHunt` agora exigem `huntEndReason` nulo, combate não finalizado e estado `Active`; #4 novo teste `EndHunt_DeathNoticeOnlyOnDeath_AlwaysReturnsToVictoria` (Death com aviso; Victory, Manual e Disconnect sem aviso; sempre "Victoria"); #5 nota no contrato `PV-SUPPLY-011`.

Resultado da solução: build com 0 avisos; **295 aprovados, 0 ignorados, 0 falhas** (Application 24, Domain 108, Runtime 17, Smoke 62, Api 84).

## Revisão (`pv-architecture-reviewer`) — REPROVADO, corrigido na branch `chief/q4-combat-013b`

- **B1 (corrigido):** com o resolver real, a skill em área parava no primeiro alvo quando a mana ficava entre o custo e o dobro (o resolver conferia a mana de novo a cada alvo, já descontada). Agora a mana é conferida uma vez, antes de qualquer sorteio, todos os alvos são resolvidos e só então o custo é cobrado. Teste novo `SmokeTests/ClassSkillCombatTests.AreaSkill_RealResolver_ManaEqualToCost_HitsAllTargets` (mana = custo, 3 alvos, mana final 0) e `…_NotEnoughMana_NoSkillNoRandomConsumed`.
- **B2 (corrigido):** a regra da área (quem é atingido, custo único) saiu do orquestrador (ADR-051, linha 36; ADR-045) e foi para `CombatResolver.ResolveSkill`. Operação nova e aditiva em `ICombatResolver` (método padrão que não executa, para resolvers que não conhecem skills em área), com os records `SkillResolution` e `SkillHit`.
- **Ressalva 1 (registrada no relatório da Fila 4):** o seletor padrão passou de `ClassAbility` para `UseSkill`, e o ataque básico ganhou multiplicador por classe — o balanceamento da Spider Cave muda (mais dano do jogador). Sustentado pelo `DEC-20261106-23`.
- **Ressalva 2 (corrigida):** texto sobre `DamageType` acima.
- **Ressalva 3 (aplicada):** os testes com o `CombatResolver` real foram para o `SmokeTests` (`ClassSkillCombatTests`); o `Application.Tests` deixou de referenciar Infrastructure.
- **Ressalva 4 (registrada):** `SkillScaling` (Magic Level, Distance, Fist) ainda não entra na conta → **R-039**.
- **Ressalva 5 (aplicada):** comentário em `ICombatActionSelector.SelectSecondaryChoice`.
- **Ressalva 6 (aplicada):** comentário de `ApplyArea` reescrito.

Resultado depois da correção: build com 0 avisos; **297 aprovados, 0 ignorados, 0 falhas** (Application 23, Domain 108, Runtime 17, Smoke 65, Api 84).

## Revisão final (`pv-architecture-reviewer`): APROVADO COM RESSALVAS

B1, B2 e as ressalvas 2 a 6 conferidos. Ressalvas novas:
1. **Aplicada:** teste direto `ClassSkillCombatTests.ResolveSkill_NotEnoughMana_NotExecuted_NoRandomConsumed` (guard de mana do resolver, 0 sorteios); o teste antigo virou `…_NotEnoughMana_SelectorChoosesNoSkill` (o seletor já filtra por mana).
2. **Registrada:** o método padrão `ICombatResolver.ResolveSkill` devolve `NotExecuted`. Um resolver que não o implemente deixa de executar skills em área (antes passavam por `ResolveAction`). Hoje só afeta os fakes de teste (`FakeResolver`, `RandomDamageResolver`, `ZeroDamageResolver`); é seguro, e para usar skills em área o resolver precisa implementar o método.
3. R-038 nas perguntas ao PO (`RELATORIO-FILA-4.md`).
4. Sem ação: o BOM do `Application.Tests.csproj` saiu no diff.

Resultado: build com 0 avisos; **298 aprovados, 0 ignorados, 0 falhas** (Smoke 66).
