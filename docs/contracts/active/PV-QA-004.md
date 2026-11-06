# PV-QA-004 — Ponta a ponta na Spider Cave (API real)

Status: **IMPLEMENTADO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no mesmo branch). Branch `chief/m3-qa-004`; só `tests/` e documentação
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 9).
Lastro: contratos do M3 (`PV-COMBAT-010/011/012`, `PV-HUNT-010`, `PV-SUPPLY-010`, `PV-CONTENT-011`); `TESTING-STRATEGY`.

## Testes (`tests/ProjectVictory.Api.Tests/SpiderCaveEndToEndTests.cs`)

Todos pela **API real** (`WebApplicationFactory`, Development, SQLite temporário, JWT aleatório).
- A semente de servidor (`Combat:FixedSeed`) é fixa e escolhida de forma determinística para sortear o encounter desejado da Spider Cave.
- As poções iniciais vêm da configuração do servidor (`Supplies:Starter…`).
- Nenhuma configuração vem do cliente.

| Cenário | O que prova |
|---|---|
| **Vitória** (Knight × [Rat, Cave Rat, Rat]) | O combate é por turnos (os monstros também ferem). Cada monstro morto rende XP pelo **Reward Pipeline**, e a soma por rodada é igual à soma das definições (35) e ao total do personagem. O loot vai para o inventário (2–3 itens: Rat Tail 100%, Cheese 50%). `huntEndReason = Victory` e o jogador volta à cidade |
| **Derrota** (Sorcerer × [Giant Spider]) | `playerDefeated`, `huntEndReason = Death`, volta à cidade pelo **mesmo pipeline**. A XP não muda (nada perdido). A **próxima Hunt começa com HP cheio** (125 no primeiro turno) |
| **Poção a partir do próximo turno** | Sem política, a rodada 1 não usa poção. O jogador liga a política **durante a Hunt** (`POST api/hunt/supplies` → 200) e as rodadas seguintes usam poções (cada uma cura), no máximo as 3 que existem. Com a semente fixa, termina por **OutOfSupplies** depois de **exatamente 3** poções (desfecho concreto afirmado) |
| **Fim de suprimento** (política de mana sem poção de mana) | `huntEndReason = OutOfSupplies` na primeira rodada, combate `Cancelled` e volta à cidade |
| **Determinismo da Hunt inteira** | A mesma semente produz a mesma sequência de turnos, danos e motivo de fim para dois personagens diferentes |

Fora destes cenários, também cobertos em outros testes: 401 sem token (`AuthorizationHttpTests`), 403 de outra conta (`OwnershipAndTokenHttpTests`, `SupplyPolicyHttpTests`), ouro e preço em SQLite (`SmokeTests`).

## Limites

- O **ouro** não aparece em nenhuma resposta HTTP (exibição no M8). Ele é verificado nos testes de Smoke.
- A resposta do inventário lista ids de instância, então o tipo de cada item é conferido nos testes de Smoke.

Resultado da solução: build com 0 avisos; **256 aprovados, 0 ignorados, 0 falhas**. Nenhum arquivo em `src/` foi alterado.

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

O revisor rodou a suíte (256 aprovados) e confirmou que o determinismo é real. Ressalvas aplicadas no mesmo branch:
1. O teste de poção não tem mais asserção condicional: afirma `OutOfSupplies` e 3 poções.
2. Cada teste confere pela resposta de `/api/hunt/execute` que o encounter sorteado é o esperado (primeiro monstro).
3. O jogador é identificado pelo nome do personagem criado, não pela inicial.
4. O comentário do arquivo foi alinhado com os limites: o ouro não sai no HTTP; o loot é conferido pela contagem.
5. Esta revisão valeu para `623caeb`. Os commits seguintes do branch são estas ressalvas e as do `PV-CONTENT-011` (só `tests/` e documentação); a suíte completa foi rodada de novo antes do relatório final.
