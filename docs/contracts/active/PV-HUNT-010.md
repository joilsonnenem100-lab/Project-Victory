# PV-HUNT-010 — Derrota e pipeline único de encerramento da Hunt

Status: **IMPLEMENTADO** (branch `chief/m3-hunt-010`)
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
