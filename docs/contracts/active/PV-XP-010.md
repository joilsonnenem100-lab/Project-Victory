# PV-XP-010 — Experience Distribution como responsabilidade própria

- **Status:** Implementado (Fila 6, branch `chief/q6-xp-010`)
- **Origem:** ordem "MODO FILA 6", item 2; M4 do `roadmap-to-playable.md`.
- **Lastro:**
  - **ADR-007:** a Experience Distribution só distribui; o cálculo é do Reward Calculation e o bônus é do Bonus Pipeline; ela recebe apenas o **valor final**.
  - **ADR-024:** todo ganho permanente passa pelo Reward Pipeline, o único que altera Experience.
  - ADR-023/025/027/028: Experience é a única verdade, e o Level é derivado.

## Implementação

- `src/ProjectVictory.Application/Reward/ExperienceDistribution.cs`:
  - `ExperienceShare(CharacterId, Amount)`;
  - `IExperienceDistributor.Distribute(finalExperience, participants)`. **Não altera Experience**: só reparte;
  - `EvenSplitExperienceDistributor` (*** PROVISÓRIO ***, a regra de grupo é do PO): divisão igual; o resto da divisão vai, 1 a 1, aos primeiros participantes na ordem recebida (determinístico, e a soma das partes é o total). Valida participantes não vazios e distintos e Experience ≥ 0.
- **Reward Pipeline (`RewardResolutionHandler`):**
  1. a Experience final é calculada como antes (XP do monstro × XP rate);
  2. ela passa pela `IExperienceDistributor` com os participantes. Hoje há 1 jogador por Hunt, então o único participante é o personagem do combate;
  3. cada parte é aplicada pelo `ApplyResolvedRewardToCharacterHandler` (o mesmo de antes).

  Sem Experience (derrota, sem definição ou 0), a chamada é a mesma de antes e a distribuição não é chamada.
- **`ApplyRewardHandler`** (usado pelos SmokeTests): também passa pela distribuição, com 1 participante.
- **Aditivo:** os construtores novos recebem a `IExperienceDistributor`. Os antigos continuam e usam o `EvenSplitExperienceDistributor`. DI: `IExperienceDistributor` → `EvenSplitExperienceDistributor` (singleton).

## Testes

- Novo `Application.Tests/ExperienceDistributionTests`:
  - 1 participante recebe tudo (10, 0 e 3 bilhões);
  - 3 participantes com 100 de XP recebem 34/33/33, na ordem, com a soma preservada;
  - entradas inválidas são rejeitadas.
- Novo `SmokeTests/ExperienceDistributionPipelineTests` (**regressão**):
  - vitória sobre um Rat pelo Reward Pipeline real dá **+10 XP**, igual a antes, e passa pela distribuição com 1 participante;
  - na derrota, nenhuma Experience e nenhuma chamada à distribuição.
- Os E2E existentes, que já conferem a XP das vitórias na Spider Cave, **não mudaram** e continuam verdes.

Resultado: build com 0 avisos; **325 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 117, Runtime 17, Smoke 71, Api 89).

## Também nesta branch: ressalvas da revisão do `PV-MONSTER-010`

1. Uma regra de monstro agora só pode ser `UseSkill`: poção, `ClassAbility` e `BasicAttack` são rejeitados. O consumo de poção só conta ações **do jogador**. Assim, um dado de monstro não pode gastar as poções do jogador.
2. Teste de DI: o seletor resolvido pela DI usa a `IMonsterAI` registrada (`Selector_ResolvedByDI_UsesTheRegisteredMonsterAI`).
3. Comentário nos construtores antigos dos seletores: são para compatibilidade e testes; a produção usa a DI.
4. A validação das regras foi movida para o `Domain.Tests/MonsterBehaviorTests`.
5. e 6. O roadmap e o contrato derivado da ordem ficam registrados no relatório da Fila 6.
7. O R-043 continua como bloqueio de entrada do contrato de skills de monstro.

## PROVISÓRIO

- A regra de divisão da Experience em grupo (`EvenSplitExperienceDistributor`) é do PO; no Tibia, o *shared experience* tem regras próprias. Para trocar, implemente outra `IExperienceDistributor` e registre-a na DI.
- Quem são os participantes de uma Hunt em grupo: hoje é sempre o personagem do combate. A Hunt em grupo é uma fase futura.
