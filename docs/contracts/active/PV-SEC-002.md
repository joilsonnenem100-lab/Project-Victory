# PV-SEC-002 — Token sem a claim da conta responde 401 (R-018)

Status: **IMPLEMENTADO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no branch `chief/m3-char-010`). Branch `chief/m3-sec-002`
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 1; autorizado pelo Arquiteto-Chefe).
Lastro:
- R-018 (`project-state.md`), achado do `PV-QA-003`;
- Handbook V3 §23 (autenticação);
- `CLAUDE.md` (Server Authority: identidade vem do token, `ICurrentPlayerContext`).

## Problema

Um token JWT válido (assinatura, Issuer, Audience e validade corretos) **sem** a claim `NameIdentifier` passava pelo `[Authorize]`. Nos endpoints que liam a identidade, o `HttpCurrentPlayerContext` lançava `InvalidOperationException` e a resposta era **500**, com log de erro. Com uma claim que não era um GUID, o `Guid.Parse` também gerava 500. Em endpoints que não liam a identidade (ex.: `playable/status`), a resposta era 200.

## Escopo implementado (defesa em duas camadas)

1. **`API/Authentication/AccountClaim`:** regra única para a claim da conta. Ela precisa ser um `NameIdentifier` com um GUID válido e diferente de vazio.
2. **JwtBearer `OnTokenValidated`** (`Program.cs`): token sem identidade de conta utilizável → `context.Fail` → **401** em **todo** endpoint `[Authorize]`.
3. **`HttpCurrentPlayerContext`:** em vez de `InvalidOperationException`/`FormatException`, lança `PlayerIdentityUnavailableException`. O `ApiExceptionHandler` mapeia essa exceção para **401**, com log de **aviso** em vez de erro (resolve o agravante de log apontado no R-018).

Nenhum endpoint, DTO ou handler da Application mudou. Tokens emitidos pelo `JwtTokenGenerator` sempre trazem a claim, então os jogadores não são afetados.

## Testes

- `Api.Tests/OwnershipAndTokenHttpTests.TokenWithoutUsableAccountClaim_Returns401`: 3 casos (sem claim, claim que não é GUID, GUID vazio) × 2 endpoints (`/api/character`, `/api/playable/status`), todos **401**. Substitui o teste do `PV-QA-003` que fixava o 500 de propósito, como o próprio teste e o R-018 pediam.
- `Api.Tests/HttpCurrentPlayerContextTests`: claim válida devolve o `AccountId`; ausente, não-GUID ou vazia lançam `PlayerIdentityUnavailableException`.

Resultado da solução: build com 0 avisos; **179 aprovados, 0 ignorados, 0 falhas**.

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

O revisor rodou build e testes (179 aprovados). Ressalvas:
1. A rota exceção → 401 com log de aviso no `ApiExceptionHandler` não tem teste de ponta a ponta, porque o JwtBearer barra antes. Fica coberta pelo teste unitário do `HttpCurrentPlayerContext`. Um teste do handler com logger falso fica para contrato futuro.
2. **Aplicada:** `PublicLogin_StillWorks_EvenWithTokenWithoutAccountClaim`.
3. O 401 da defesa em profundidade não traz o header `WWW-Authenticate`. Cosmético; aceito.
4. **Aplicada:** o teste HTTP virou 6 linhas `MemberData` (claim × URL). Uma falha agora identifica o endpoint.
5. Empilhamento confirmado: `chief/m3-char-010` nasce de `chief/m3-sec-002`.
