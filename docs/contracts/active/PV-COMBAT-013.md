# PV-COMBAT-013 — 4 skills por classe em dados, seletor automático, dano extensível

- **Status:** Implementado (Fila 4, branch `chief/q4-combat-013`)
- **Origem:** ordem do PO "MODO FILA 4 - VERSÃO FINAL", item 5; `DEC-20261106-23` (skills) e `DEC-20261106-16` (combate simples).
- **ADRs:** ADR-006 (ataque básico + ação secundária), ADR-051 (Combat Orchestrator), ADR-052 (turnos determinísticos), ADR-045 (orquestrador não implementa regra de outro domínio).
- **Regra aditiva:** o enum `CombatActionKind` ganha `UseSkill`; `ClassAbility` continua existindo (sem uso pelo seletor padrão). DTOs e logs ganham campos opcionais (`SkillName`, `DamageType`).

## Escopo

1. **Skills como dados** — `src/ProjectVictory.Domain/Combat/Rules/ClassSkills.cs`:
   - `SkillDefinition(Id, Name, Class, Targeting, ManaCost, DamageMultiplier, UnlockLevel, Scaling, DamageType = Physical)`;
   - `ClassSkillTable.All`: 4 skills por classe (2 em área, 2 em alvo único), desbloqueio por Level (1, 5, 15, 20);
   - `ClassSkillTable.BasicAttackMultiplier(class)`: força do ataque básico por classe.
2. **Seletor automático** — `SkillSelectionPolicy(MinimumEnemiesForArea = 2)`: com 2+ inimigos vivos usa a melhor skill em área que o mana paga; com 1 inimigo, a melhor de alvo único; sem mana ou sem skill do tipo certo, nenhuma (só o ataque básico). Ligado ao `DefaultCombatActionSelector`; o `SupplyAwareCombatActionSelector` mantém a prioridade da poção (`DEC-20261106-22`, próximo turno) e, sem poção, delega a escolha.
3. **Orquestrador** — `ICombatActionSelector.SelectSecondaryChoice` (método padrão de interface, aditivo) devolve `CombatTurnChoice(Kind, Skill)`. Skill em área atinge todos os inimigos vivos e gasta o mana **uma vez**. O log registra `SkillName`.
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
