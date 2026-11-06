# PV-MONSTER-010 — Monster AI orientada a dados

- **Status:** Implementado (Fila 6, branch `chief/q6-monster-010`)
- **Origem:** ordem "MODO FILA 6", item 1; M4 do `roadmap-to-playable.md` (ordem oficial: Monster AI → Experience Distribution → Spawn).
- **Lastro:**
  - **ADR-005:** IA totalmente orientada por configuração; **uma** implementação; comportamento definido pelas habilidades da espécie.
  - **ADR-006:** Basic Attack obrigatório, executado pela engine; a Monster AI só escolhe a Secondary Action.
  - **ADR-051:** nenhuma lógica de monstro no orquestrador.
  - **ADR-052:** determinismo com a semente do encounter.
- **Fora de escopo (ordem):** combate elemental e skills de monstro avançadas. Nenhuma mecânica nova de jogo.

## Implementação

- **Dados (Domain):** `Combat/Rules/MonsterBehavior.cs`.
  - `MonsterActionRule(Kind, ChancePercent, Skill?)`: "com X% de chance, usar esta Secondary Action". A validação exige chance de 1 a 100, não aceita `BasicAttack` como regra (ele é sempre executado) e exige `UseSkill` com a skill.
  - `MonsterBehavior(Rules)`, com `MonsterBehavior.BasicAttackOnly`.
- **Definição do monstro:** `MonsterContent` (catálogo da Spider Cave) ganha `Behavior` (parâmetro opcional, aditivo). Os 5 monstros declaram **`BasicAttackOnly`**, que é o comportamento de antes, agora em dados.
- **Application:** `Combat/TurnBased/MonsterAI.cs`.
  - `IMonsterBehaviorProvider` (fonte dos dados) e `IMonsterAI` (a Monster AI).
  - `DataDrivenMonsterAI`, a **única** implementação. Não conhece espécies: lê as regras do monstro (`SourceId` = MonsterId), testa uma a uma na ordem com `session.Random` (semente do encounter) e a primeira sorteada vence. **Sem regras, devolve `null` sem sortear nada**, por isso os combates atuais não mudam.
- **Infrastructure:** `Combat/MonsterBehaviorProvider` lê o catálogo. Monstro fora do catálogo fica só com o ataque básico, o mesmo critério do `MonsterCombatProfileProvider`.
- **Seletores:** `DefaultCombatActionSelector` e `SupplyAwareCombatActionSelector` delegam o lado do monstro à `IMonsterAI` (construtores novos e aditivos; os antigos continuam, com a IA sobre "só ataque básico"). O orquestrador não mudou: continua pedindo a Secondary Action ao seletor.
- **DI:** `IMonsterBehaviorProvider` → `MonsterBehaviorProvider` e `IMonsterAI` → `DataDrivenMonsterAI` (singletons).

## Testes

- Novo `Application.Tests/MonsterAITests`:
  - sem regras, não consome aleatoriedade;
  - a regra vem do dado, com a mesma semente dando as mesmas escolhas, e 50% usa às vezes e às vezes não;
  - a IA não escolhe pelo jogador;
  - pelo orquestrador, o monstro usa a habilidade de dados depois do ataque básico;
  - a validação das regras.
- Novo `SmokeTests/MonsterBehaviorDataTests`: os 5 monstros declaram o comportamento em dados (só ataque básico), e a composição registra a IA única.
- **Nenhum teste existente mudou**, incluindo os ponta a ponta da Spider Cave: o resultado dos combates é o mesmo.

Resultado: build com 0 avisos; **317 aprovados, 0 ignorados, 0 falhas** (Application 27, Domain 116, Runtime 17, Smoke 68, Api 89).

## PROVISÓRIO / pendências

- Habilidades de monstro (dados novos no `SpiderCaveCatalog`, campo `Behavior`): nenhuma hoje. É decisão de balanceamento do PO.
- **R-043:** uma habilidade de monstro hoje usaria `SkillDefinition`, que tem `Class` (classe de jogador). Uma definição neutra de habilidade de monstro fica para o contrato de skills de monstro.

## Revisão (`pv-architecture-reviewer`): APROVADO COM RESSALVAS — aplicadas na branch `chief/q6-xp-010`

1. Regra de monstro só `UseSkill`; consumo de poção só para ações do jogador.
2. Teste de que a DI usa a `IMonsterAI` registrada.
3. Comentários nos construtores antigos.
4. Validação testada no `Domain.Tests`.
5/6. Registrados no relatório da Fila 6 (roadmap marcado no modo fila; contrato derivado da ordem do PO).
7. R-043 mantido.
