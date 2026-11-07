# PV-FIX-001 — `enter` sem personagem devolvia 500 (Fila 10, item 7)

Status: IMPLEMENTADO (2026-11-07) · Fecha **R-069** (registrado na revisão do `PV-QA-010`, ressalva c; resolução proposta no `project-state.md`: "mapear para 400/404 com exceção tipada").

## O que muda
- `EnterGameHandler`: conta sem personagem (nunca criado ou exclusão finalizada, `PV-CHAR-013`) lança `NoCharacterForAccountException`, que continua sendo `InvalidOperationException` (aditivo para quem já capturava).
- `ApiExceptionHandler`: essa exceção vira **404 Not Found** (`ProblemDetails`). Antes era um 500 genérico. Escolha: 404, porque o recurso pedido (o personagem da conta) não existe e o pedido em si é válido.

## Testes
`Api.Tests/EnterWithoutCharacterHttpTests`: sem personagem → 404; depois de criar → 200; sem token → 401.

## Resultado
Build sem avisos; **528 testes verdes**. Sem migration.
