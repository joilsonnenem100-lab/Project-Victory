# Relatório — MODO FILA 8 (econômico), 2026-11-06

Branches empilhadas a partir da `main` (`39f0a75`), worktree `C:/pv8`, push de cada uma; sem `push --force`, sem tocar na `main`, nada apagado (só `git mv`), sem segredos, nenhuma migration existente editada, banco do PO não tocado (testes em bancos temporários). **Merge da última branch (`chief/q8-gov-013`) traz tudo.** Anexo: `REVISAO-FECHAMENTO-FILA-8.md`.

| # | Item / branch `chief/q8-*` | Status | Revisor |
|---|---|---|---|
| 1 | PV-GOV-012 — `docs/INDEX.md`, leitura pelo INDEX, economia de contexto, relatórios em `historico/` | ✅ | Aprovado c/ ressalvas (aplicadas) |
| 2 | PV-HUNT-013 — 2ª Hunt → 409 sem encerrar a 1ª (R-052); logout = saída manual | ✅ | Aprovado c/ ressalvas (aplicadas) |
| 3 | PV-RUNTIME-012 — reinício = queda; vida 0 → cheia; EnterGame × Recovery (R-055); R-056 | ✅ | Aprovado c/ ressalvas (aplicadas/R-063) |
| 4 | PV-RUNTIME-013 — área escolhida de verdade (R-051, R-041); heartbeat pós-logout (R-054) | ✅ | Aprovado c/ ressalvas (aplicadas) |
| 5 | PV-EVENT-012 — ADR-058 (R-058); escopo de DI por queda (R-060) | ✅ | Aprovado c/ ressalvas (aplicadas) |
| 6 | PV-QA-007 — E2E pela API real das 4 decisões | ✅ | Aprovado c/ ressalvas (aplicadas/registradas) |
| 7 | PV-GOV-013 — LOCK: **Event System e Recovery LOCKED**; Game Session em REVIEW | ✅ | Aprovado c/ ressalvas (aplicadas) |

**Build:** 0 avisos, 0 erros em todas as branches. **Testes:** 381 → **410 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 120, Runtime 17, Smoke 134, Api 108), executados por mim ao fim de cada item.
**Migrations:** 1 nova e aditiva — `20261107024024_RuntimeLoggedOut` (`AddColumn<bool> LoggedOut`, padrão `false`; `Down` remove).
**Decisões registradas:** `DEC-20261106-45…51`; **ADR-058** (novo, ACCEPTED). R fechados: R-041, R-052, R-054, R-055, R-056, R-058, R-060 (R-051 parcial). Novos: R-062 (servidor único), R-063 (dívidas do Recovery).
**Nada travou nem foi pulado.** R-053 (caminho legado `gameplay/action`) **não foi limpo**: só tem teste de autorização, não funcional (ordem: limpar só se coberto).

**PROVISÓRIO:** vida 0 no checkpoint → vida e Mana cheias (`DEC-48`, `RuntimeReconstruction`); tempos 60 s / 10 s (`appsettings` → `GameSession`); encounters ainda da área padrão (só existe a Spider Cave).

**Perguntas ao PO**
1. Confirmar o `DEC-48` (vida 0 → cheia), hoje PROVISÓRIO.
2. Game Session: o "Runtime Coordinator" do Handbook §10 deve encadear Hunt→Combat→Reward (refatorar) ou basta emendar o Handbook (o Hunt Loop + Event Bus fazem a cadeia)? Decide o LOCK do módulo.
3. Autorizar contrato para aposentar o caminho legado `gameplay/action` (R-053), começando por um teste funcional.
4. Alinhar as instruções dos subagentes (`.claude/agents/`) à regra de economia de contexto (hoje mandam ler o `project-state.md` inteiro) — não alterei por ser configuração.
5. Cliente (UI-1, fora desta fila): botão de sair da Hunt (sem ele, o cliente atual recebe 409 até morte/logout/queda), seletor de área (`huntArea`), status `LoggedOut` no heartbeat.
6. Pendentes de filas anteriores: R-043/R-050, R-045, R-040, R-035, balanceamento, Treino Free × Premium.
