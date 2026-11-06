# PV-GOV-008 — CLAUDE.md e project-state.md no estado real

Status: **CONCLUÍDO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no branch `chief/q2-qa-003`). Branch `chief/q2-gov-008`; só `.md` — build/teste não aplicáveis ao diff (a suíte foi executada no branch anterior e no seguinte, sem mudança)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 2", item 2). Lastro: ADR-016 do `decision-log.md` (documentação é do Arquiteto); `CLAUDE.md` (Definition of Done: documentação atualizada).

## Mudanças no `CLAUDE.md`

- Comandos: `SmokeTests` deixa de aparecer como item separado "com Skip" (está no `.sln` e o Skip saiu no `PV-REWARD-010`); passo de **user-secrets** (`Jwt:Key`) antes do `dotnet run`, com o motivo (a API não sobe sem chave) e o guia.
- Lista dos **5 projetos de teste** e baseline atual (161 aprovados, 0 ignorados; fonte oficial: `project-state.md`).
- Regra de confirmar que não restou processo `dotnet` ao fim de cada contrato.
- **ADRs:** próximo livre ADR-058; 044/048 NOT USED; 026 ACCEPTED; 055 PROPOSED; formato `DEC-AAAAMMDD-NN`.
- `CURRENT_CONTEXT`: a lista de documentos com referência antiga foi corrigida (eram 4 documentos, não 3) e registra que o `PV-GOV-007` já trocou as referências.
- Nova seção **Execução autônoma em fila (MODO NOITE / MODO FILA)**: branches empilhadas, passos por contrato, o que fazer se travar, relatórios em `docs/architecture/`, worktrees.

## Mudanças no `project-state.md`

Data de atualização, resumo (M2 completo; combate ainda stub), "Sprint Atual" (Fila 2) e "Próximo Objetivo" (M3), com o texto anterior preservado como histórico.
