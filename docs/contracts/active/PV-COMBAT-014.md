# PV-COMBAT-014 — Spell de cura por vocação e seletor automático (PO-53)

Origem: Fila Noturna 1 (2026-11-07), executada sem o PO (leituras PROVISÓRIAS marcadas abaixo). Lastro: PO-53 e PO-01 em `docs/product/PO-DECISOES-BASE.md` (todas as vocações têm 1 spell de cura que melhora com o Level; cura = **porcentagem fixa da vida**; cada uso gasta Mana; Druid com "cura própria"), `DEC-20261107-01` (adoção do PO-DECISOES-BASE; emenda do `DEC-20261106-03/04/22`: "sem suprimento não há cura" vale **só para poções**), R-065 em `project-state.md`, ADR-051 (o Combat Orchestrator não implementa regra de outro domínio: dano, cura e custos ficam no `CombatResolver`/dados), ADR-052 (um participante por turno), ADR-006 (turno = Basic Attack + Secondary Action opcional), `DEC-20261106-15` (referência Tibia; nomes genéricos), `PV-SUPPLY-010/011` (política de poções), `PV-COMBAT-013` (skills em dados; determinismo do resolver). **Só o servidor**; nada no cliente.

## Escopo
- **D1 — Dados (Domain, `Combat/Rules/ClassHealingSpells.cs`):** `HealingBand(FromLevel, PercentOfMaximumHealth)`, `HealingSpellDefinition(Id, Name, Class, ManaCost, Bands)` com `PercentForLevel(level)` (faixa mais alta com `FromLevel ≤ level`) e `RawHealingFor(level, maxHealth)` = ⌊maxHealth × % / 100⌋, mínimo 1 quando a % é positiva. `ClassHealingSpellTable`: **1 spell por vocação** para as 5 classes do código (Knight, Paladin, Sorcerer, Druid, Monk). `HealingSpellPolicy(BelowPercent)`: o seletor cura quando a vida fica abaixo de X % da máxima; `null`/0 = desligado.
- **D2 — Ação nova (aditiva):** `CombatActionKind.UseHealingSpell` (valor acrescentado no fim do enum). `Combatant.HealingSpell` (parâmetro opcional novo; monstros sem spell). `CombatantFactory.CreatePlayer` entrega a spell da classe.
- **D3 — Regra no resolver (ADR-051):** `CombatResolver.ResolveAction(UseHealingSpell)`: sem spell, morto ou com Mana < custo → `NotExecuted`; senão cura = `min(RawHealingFor(Level, MaxHealth), MaxHealth − CurrentHealth)` (limitada ao máximo de vida) e `ManaCost` = custo da spell. **Não consome número aleatório**: o combate continua determinístico com a semente do encontro.
- **D4 — Orquestrador:** só o alvo — a spell de cura mira o próprio ator, como as poções (uma condição a mais na linha que já escolhia o alvo das poções). Nenhuma regra de cura no orquestrador nem em handler.
- **D5 — Seletor automático (`SupplyAwareCombatActionSelector`):** recebe `HealingSpellPolicy` (parâmetro opcional novo; sem ela, a cura por spell fica **desligada**, para os testes de unidade antigos que constroem o seletor à mão). A composição (DI) registra `HealingSpellPolicy.Default` (`TryAddSingleton`) e o `Program.cs` lê `Combat:HealingSpellBelowPercent` (0 a 100; fora disso o startup falha).

## Regra de prioridade (Secondary Action do jogador)
1. **Spell de cura** — vida abaixo do limite da `HealingSpellPolicy` **e** Mana ≥ custo da spell.
2. **Poção de vida** (reserva) — política de poção manda (vida abaixo do limite da poção) e há poção. Cobre o caso "sem Mana" e o caso "vida entre o limite da spell e o da poção" (comportamento antigo preservado).
3. **Poção de Mana** — política de poção manda e há poção; repõe a Mana que a spell gasta.
4. **Skill da classe** (`SkillSelectionPolicy`, `PV-COMBAT-013`).
Monstros: inalterado (Monster AI).

## Leituras PROVISÓRIAS (onde trocar)
1. **Hipótese de trabalho: "% fixa da vida" = % da vida MÁXIMA** (PO-DECISOES-BASE, "Ponto a confirmar com o PO"). Trocar em `HealingSpellDefinition.RawHealingFor` (`Domain/Combat/Rules/ClassHealingSpells.cs`).
2. **Faixas de Level e porcentagens** — todas as vocações: Level 1 → 10 %, 20 → 15 %, 50 → 20 %, 100 → 25 %; **Druid** (PO-01, "cura própria"): +5 pontos em cada faixa (15/20/25/30 %) — **inferência do PO-01** (o PO-01 diz só "cura própria"; o bônus de +5 pontos não está escrito em lugar nenhum; levado às perguntas ao PO abaixo). Trocar em `ClassHealingSpellTable.StandardBands` / `DruidBands`.
> *Nota de superação (2026-11-07, `DEC-20261107-05`, PO-59 em `docs/product/PO-DECISOES-BASE.md`): sem bônus do Druid; a mesma tabela para todas as vocações (`PV-COMBAT-015`).*
3. **Custo de Mana = 20** para todas as vocações. Trocar em `ClassHealingSpellTable.DefaultManaCost` (ou por linha da tabela).
4. **Nomes** (genéricos, estilo Tibia): Wound Mending (Knight), Divine Mending (Paladin), Arcane Mending (Sorcerer), Nature Mending (Druid), Inner Mending (Monk). Trocar em `ClassHealingSpellTable.All`.
5. **Limite do seletor = 50 % da vida máxima.** Trocar por configuração `Combat:HealingSpellBelowPercent` (0 desliga) ou em `HealingSpellPolicy.DefaultBelowPercent`.
6. **Prioridade spell → poção de vida → poção de Mana → skill** (acima). Trocar em `SupplyAwareCombatActionSelector.SelectSecondaryChoice`.
7. **Spell disponível desde o Level 1** (sem Level de desbloqueio; PO-53 diz "todas as vocações têm"). Para desbloquear por Level, acrescentar `FromLevel` maior na primeira faixa.
8. A cura por spell **não** passa pela configuração de poções do jogador (não há opção "não usar a spell" por personagem); o limite é do servidor. Uma configuração por personagem fica para o contrato de UI de suprimentos.

## Testes antigos
**Nenhum teste antigo foi ajustado.** Com a cura ligada na composição (padrão 50 %), todos os 436 testes anteriores continuaram verdes sem mudança: os cenários antigos que montam o seletor à mão ficam sem cura (política ausente = desligada, escolha registrada no D5), e os cenários pela composição real (smoke/E2E) não mudaram de desfecho conferido. Escolha: manter a cura **ativa** na composição (comportamento real do jogo) em vez de desligá-la nos cenários antigos por configuração, por ser o mais honesto — se um desfecho antigo tivesse mudado, o teste seria ajustado e listado aqui.

## Migration
Nenhuma. A spell vem de dados em código e o limite, de configuração.

## Fora do escopo
Cliente (mostrar a cura no log/HUD; opção do jogador para a spell); spells de cura em aliado; escalonamento por Magic Level; cooldown/exaustão (Tibia) — não há turno de cooldown no combate atual.

## Testes
- `Domain.Tests/ClassHealingSpellTableTests` (12): 1 spell por vocação com custo de Mana; tabela por faixa de Level (1, 19, 20, 49, 50, 99, 100, 500); Druid +5; cura bruta (arredondamento e mínimo 1); política (limite, desligada, 0).
- `SmokeTests/HealingSpellCombatTests` (7), com o `CombatResolver` real: cura = % da vida máxima pela faixa e custa Mana sem consumir aleatoriedade; cura limitada ao máximo; sem Mana (ou sem spell) não cura; seletor (spell primeiro com Mana; poção de vida como reserva; poção de vida entre os limites; poção de Mana; política desligada = comportamento antigo; sem Mana não escolhe a spell); orquestrador (a spell cura o próprio ator, gasta Mana, e duas execuções com a mesma semente geram o mesmo log); composição registra o limite padrão.
- Resultado: build 0 avisos; **455 testes** verdes (436 + 12 + 7).

## Ressalvas da revisão (A–D) — Fila Noturna 1, commit próprio
- **A — cura ligada na composição real:** confirmada, sem correção de código. `SmokeTests/HealingSpellCombatTests`: `Composition_ResolvedSelector_ChoosesHealingSpell_WhenBelowLimitWithMana` (o `ICombatActionSelector` resolvido pelo DI é o `SupplyAwareCombatActionSelector` e escolhe `UseHealingSpell` com vida 45 % e Mana; não escolhe acima do limite nem sem Mana) e `Composition_ResolvedOrchestrator_HealsHurtPlayer` (o `CombatOrchestrator` do DI cura o jogador ferido). Pela API real: `Api.Tests/HealingSpellConfigurationTests.DefaultConfiguration_IsFiftyPercent_AndTheResolvedSelectorUsesIt`.
- **B — configuração inválida derruba o startup:** `Api.Tests/HealingSpellConfigurationTests.OutOfRange_FailsStartup` (-1 e 101; a falha tem de vir do `InvalidOperationException` que cita `Combat:HealingSpellBelowPercent`) e `ServerConfiguration_ChangesTheLimit` (30), no padrão do `ExperienceRateConfigurationTests`.
- **C — vida cheia:** o `CombatResolver` devolvia `Executed` com cura 0 e gastava Mana; agora devolve `NotExecuted` sem gastar Mana (`Resolver_AtFullHealth_DoesNotCastAndSpendsNoMana`). O seletor já não escolhia a spell com vida cheia (só abaixo do limite), então nenhum desfecho de combate pela composição muda.
- **D — provisório do Druid:** marcado como **inferência do PO-01** (leitura 2 acima).

## Perguntas ao PO
1. ✅ *Respondida (2026-11-07, PO-59, `DEC-20261107-05`): nenhum bônus — cura igual para todas as vocações; removido no `PV-COMBAT-015`.* **Bônus de cura do Druid (inferência do PO-01):** o PO-01 diz que o Druid tem "cura própria"; a implementação dá **+5 pontos percentuais** em cada faixa (15/20/25/30 % contra 10/15/20/25 % das outras vocações). Confirma esse bônus, outro valor, ou nenhum bônus (a "cura própria" seria só a spell que todas as vocações já têm pelo PO-53)?
2. "% fixa da vida" = % da vida **máxima** (leitura 1) — confirmar.
