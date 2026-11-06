# PV-QA-003 — Testes HTTP de posse e de token

Status: **CONCLUÍDO** (branch `chief/q2-qa-003`; só `tests/` e documentação)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 2", item 3). Lastro: `PV-SEC-001` (403 para recurso de outra conta), `PV-PLATFORM-002` (validação do JWT), `TESTING-STRATEGY`; lacunas listadas no `PV-QA-001` parte 2.

## Testes (`tests/ProjectVictory.Api.Tests/OwnershipAndTokenHttpTests.cs`, API real)

| Teste | Resultado |
|---|---|
| Outra conta muda atividade e local do runtime do dono | **403** nos dois; o dono continua com 200 |
| Outra conta encerra a hunt session do dono | **403** |
| Outra conta ataca o encounter do dono | **403**; o ataque do dono continua 200 |
| Token expirado há 10 min (acima da tolerância de 5 min) | **401** |
| Token válido **sem** a claim da conta (`NameIdentifier`) | Nunca devolve dados; hoje responde **500**, não 401 → achado **R-018** |

## Achado R-018 (não corrigido: exige mudança em `src/`, fora do escopo)

O `[Authorize]` aceita o token. O `HttpCurrentPlayerContext` lança `InvalidOperationException("Player identity unavailable.")`, que o `ApiExceptionHandler` transforma em 500. O teste fixa o comportamento atual (falha sem dados, 500) e documenta a correção esperada (401) para um contrato futuro na API. Ao corrigir, a asserção do teste deve ser trocada.

## Resultado

Solução: build com 0 avisos; **166 aprovados, 0 ignorados, 0 falhas** (+5 em `Api.Tests`). Nenhum arquivo em `src/` foi alterado.

Neste branch também entraram as ressalvas da revisão do `PV-GOV-008`.
