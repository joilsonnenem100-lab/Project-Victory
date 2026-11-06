# PV-COMBAT-011 — Combate por turnos determinístico

Status: **IMPLEMENTADO** (branch `chief/m3-combat-011`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 4): "Combate por turnos determinístico, exatamente 1 participante ativo por turno (ADR-052). Aleatoriedade somente por gerador injetável com semente, para testes determinísticos."
Lastro: ADR-052 (execução determinística; um participante ativo por turno); ADR-051.

## Escopo implementado

1. **Ordem de turnos** (`SequentialTurnScheduler`, criado no `PV-COMBAT-010` e coberto aqui): jogador, depois cada monstro vivo na ordem do encounter, e de novo o jogador. Exatamente um ativo por turno; mortos são pulados; **sem aleatoriedade** na ordem.
   - **PROVISÓRIO:** iniciativa e velocidade não estão documentadas, então o jogador sempre começa.
2. **Gerador com semente** `Domain/Primitives/SeededRandomSource` (implementa o `IRandomSource` que já existia): a mesma semente produz a mesma sequência. Uma instância por combate (`CombatSession.Random`).
3. **Semente por encounter:**
   - `ActiveEncounter.Seed` (construtor novo; o antigo continua e usa 0);
   - `EncounterGenerator.Generate(huntId, seed)` (sobrecarga);
   - o `ExecuteHuntHandler` pede a semente ao `ICombatSeedProvider`.
4. **`CombatSettings` / `CombatSeedProvider`:** a semente é aleatória por padrão. A chave de servidor `Combat:FixedSeed` fixa todas as sementes, **só para testes e depuração**: em produção a chave fica ausente, e o cliente nunca a controla. Ela é passada ao `AddProjectVictoryCore` (parâmetro opcional novo) e lida no `Program.cs`.
5. `ActiveEncounter.AttachSession` guarda a `CombatSession` do encounter (usada no `PV-COMBAT-012`).

## Testes (`Application.Tests/TurnDeterminismTests.cs`)

- a mesma semente produz o mesmo combate inteiro (resolver que sorteia dano pelo gerador da sessão);
- sementes diferentes produzem combates diferentes;
- `SeededRandomSource` é reproduzível;
- o scheduler mantém um ativo por turno, pula os mortos e volta ao jogador;
- a semente fixa só vale quando configurada;
- o encounter guarda a sua semente.

Resultado da solução: build com 0 avisos; **202 aprovados, 0 ignorados, 0 falhas**.

## Observação

A sequência de `System.Random(seed)` é estável dentro da mesma versão do .NET. Uma atualização de runtime pode mudar combates gravados com sementes antigas; os testes comparam execuções na mesma versão.
