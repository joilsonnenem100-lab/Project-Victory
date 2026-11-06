# PV-HUNT-010 — Derrota e pipeline único de encerramento da Hunt

Status: **IMPLEMENTADO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas registradas no branch `chief/m3-supply-010`). Branch `chief/m3-hunt-010`
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 6).
Lastro:
- ADR-012 (ciclo único da Hunt; todo encerramento pelo mesmo pipeline; termina na Cidade Principal);
- `DEC-20261106-04` (morte ou fim de suprimento encerram; volta à cidade e aguarda comando; nenhuma perda de loot ou XP);
- `knowledge-base/character.md` (LOCKED: "Morte → Cidade Principal → HP cheio → Mana cheia"; "Não existe perda").

## Escopo implementado

1. **`EndHuntHandler`**, o **pipeline único**, com os motivos `HuntEndReason`: Victory, Death, OutOfSupplies, Manual, Disconnect.
   1. Fecha a Hunt: Victory, OutOfSupplies e Manual → `Completed`; Death → `Failed`; Disconnect → `Cancelled`.
   2. Leva o jogador à **cidade** e o deixa **aguardando comando** (`RuntimeActivity.Idle`).
   3. Na **morte**, HP e Mana voltam cheios (máximos derivados, `PV-CHAR-010`).
   4. **Nada é retirado**: XP, ouro e loot entregues pelo Reward Pipeline ficam.
2. O **ataque** (`AttackEncounterHandler`) encerra pelo pipeline em **vitória** (todos os monstros mortos) e em **morte** (vida 0). A resposta ganha `huntEndReason` e `returnedToCity` (**aditivos**).
3. **Cidade — PROVISÓRIO:** não existe "Cidade Principal" no mundo semeado (cidades são da Fase 7). A única localização ativa, "Starting Location" (`WorldDefinitionSeed`), faz esse papel (`EndHuntHandler.MainCity`).

## Limites registrados

- **R-027:** a **Hunt Session** persistida (`api/hunt-session/start|stop`) e a **Hunt** em memória do combate continuam separadas. O `stop` manual da Hunt Session ainda **não** passa pelo pipeline, e o pipeline não fecha a Hunt Session. O motivo `Manual` já existe; ligar os dois é do `PV-HUNT-011` (Hunt Loop, M6), junto com `Disconnect` (`PV-HUNT-012`).
- Vida e Mana do runtime ficam só em memória: a persistência é do `PV-PERSIST-011` (M5).

## Testes (`SmokeTests/HuntEndingPipelineTests.cs`)

- **Morte pelo ataque real:** o jogador começa com 1 de vida e as sementes são fixas (busca determinística em 1–30). A Hunt fica `Failed`; o jogador está em Idle, na cidade, com HP e Mana iguais aos máximos.
- **Vitória pelo mesmo pipeline:** a Hunt fica `Completed`, o jogador está em Idle na cidade, e a recompensa fica (2 Rats = 20 XP).
- **Morte não retira XP já ganha** (500 XP antes = 500 depois).

Resultado da solução: build com 0 avisos; **227 aprovados, 0 ignorados, 0 falhas**.

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

O revisor rodou a suíte (227 aprovados). Ressalvas:
1. A atividade Idle é gravada sem passar pelas regras de transição do Domain → **R-030**.
2. Cinco handlers mortos mudam o estado da Hunt fora do pipeline → **R-031**. O pipeline é o **único caminho alcançável**.
3. O teste de morte busca a semente em 1–30. É determinístico, mas uma mudança na fórmula PROVISÓRIA pode trocar a semente usada. Aceito; a preparação de teste mexe na vida do runtime e usa `GainExperience` direto só para montar o cenário.
4. O teste HTTP de `huntEndReason`/`returnedToCity` fica no `PV-QA-004`.
5. `Kind` do `CombatTurnDto`: a API já serializava enums como texto (`JsonStringEnumConverter`), então o JSON não mudou.
6. Na morte, `playerHealth` na resposta é a vida **do fim da rodada** (0); o runtime já está com HP cheio na cidade.
7. O `EndHuntHandler` não verifica o dono: hoje só o ataque o chama, **depois** do `PlayerOwnershipGuard`. Novos chamadores (`Manual`/`Disconnect`) precisarão do guard.
8. GUIDs da cidade copiados do `WorldDefinitionSeed`: menor; trocar quando existir cidade (Fase 7).
