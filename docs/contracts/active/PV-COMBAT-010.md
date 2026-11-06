# PV-COMBAT-010 — Combat Orchestrator

Status: **IMPLEMENTADO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no branch `chief/m3-combat-012`; ver `PV-COMBAT-012.md`) (branch `chief/m3-combat-010`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 3).
Lastro:
- ADR-051 (o Combat Orchestrator coordena; nunca implementa dano, Skills, IA, Loot ou Experience);
- ADR-052 (turnos, exatamente um participante ativo);
- ADR-006 (Basic Attack obrigatório + Secondary Action opcional; a Monster AI só escolhe a Secondary);
- `docs/architecture/master/` (Handbook V3, Combat).

## Escopo implementado

1. **Domain (`Domain/Combat/TurnBased`) — só estado, sem regra:**
   - `Combatant`: jogador ou monstro; vida, mana, `CombatProfile`; `ApplyDamage`, `Heal`, `RestoreMana`, `TrySpendMana`;
   - `CombatSession`: participantes, participante ativo, número do turno, `IRandomSource` injetado, resultado Ongoing/Victory/Defeat, alvo padrão;
   - `CombatTurnAction`, `CombatActionKind` (BasicAttack, ClassAbility, UseHealthPotion, UseManaPotion), `CombatActionOutcome`, `CombatTurnLogEntry`.
2. **Application (`Application/Combat/TurnBased`):**
   - `CombatOrchestrator.ExecuteRound`: executa turnos até a vez do jogador voltar ou o combate acabar. Em cada turno, o Basic Attack é sempre executado. A Secondary Action é **pedida** ao `ICombatActionSelector`, as regras ao `ICombatResolver.ResolveAction` e o próximo ativo ao `ITurnScheduler`. O orquestrador **só aplica** os resultados (dano, cura, mana) e registra o log. Tem um limite de segurança de 1000 turnos por rodada.
   - Interfaces `ITurnScheduler`, `ICombatActionSelector` e `ICombatActionObserver` (gancho para consumir suprimentos, `PV-SUPPLY-010`).
   - `ICombatResolver` ganhou `ResolveAction` (**aditivo**); o `Resolve` antigo continua.
   - `SequentialTurnScheduler` (base do `PV-COMBAT-011`) e `NoSecondaryActionSelector` (**PROVISÓRIO**: Monster AI é M4; a escolha do jogador vem do `PV-SUPPLY-010`).
3. **Infrastructure:** `CombatResolver.ResolveAction` mantém o dano fixo do stub anterior (5) até o `PV-COMBAT-012`.
4. **Composição:** scheduler, seletor e orquestrador registrados no `AddProjectVictoryCore`.

O fluxo HTTP ainda **não** usa o orquestrador. A integração no ataque do encounter entra no `PV-COMBAT-011`/`012`, junto com a semente e as regras de dano.

## Testes (`Application.Tests/CombatOrchestratorTests.cs`, resolver falso)

- ordem jogador → cada monstro vivo, um ativo por turno, parada na vez do jogador;
- o dano vem só do resolver;
- vitória quando todos os monstros morrem, com os mortos pulados;
- derrota interrompe na hora;
- a Secondary Action é escolhida fora do orquestrador e aplicada depois do Basic Attack.

Resultado da solução: build com 0 avisos; **196 aprovados, 0 ignorados, 0 falhas**.

Também neste branch, as ressalvas da revisão do `PV-CHAR-010` (ver aquele contrato).
