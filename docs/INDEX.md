# Project-Victory — INDEX (leia primeiro)

Uma página. Atualizado em 2026-11-06 (Fila 8, `PV-GOV-012`). Detalhe só quando o item de trabalho o citar.

## Estado atual
- **Jogável pela API:** conta → login → personagem → entrar → Hunt contínua (combate por turnos, IA de monstro, Spawn) → XP/loot reais → Level derivado → queda/logout/reinício sem perda.
- **Marcos:** M0–M5 concluídos; M6 (Hunt Loop, queda, Recovery) e M7 (Event Bus) entregues em **REVIEW**; `PV-TRAIN-010` e M8 (UI/Primeiro Teste Jogável) pendentes.
- **Baseline:** build 0 avisos; contagem oficial de testes em `project-state.md` (381 no início da Fila 8).
- **Nenhum módulo novo LOCKED** desde a Fase 6/Monsters; o que falta para cada LOCK fica no `roadmap.md`.

## Regras vivas (resumo; texto completo no `CLAUDE.md`)
Server Authority (identidade do token) · Aggregate Ownership · Clean Architecture · Regra Aditiva · Contrato antes de código · Experience é a verdade, Level derivado · Referência Tibia (`DEC-20261106-15`) · Nada de achismo (cite a fonte) · Economia de contexto e uma fila por sessão (`DEC-20261106-45`).

## Últimas decisões (`developer-guide/architecture/decision-log.md`)
- `DEC-20261106-46` — Segunda Hunt com outra em andamento → **409**, sem encerrar a anterior (R-052).
- `DEC-20261106-47` — Reinício do servidor = queda: retorno à última Hunt após 10 s online, nova Hunt Session.
- `DEC-20261106-48` — Vida 0 no checkpoint → volta com vida e Mana cheias na cidade (**PROVISÓRIO**, R-055).
- `DEC-20261106-49` — Logout = saída manual (Hunt `Completed`), sem retorno automático.
- Anteriores relevantes: ADR-057 (Hunt termina ao desconectar; sem Hunt offline), `DEC-20261106-20/21/22` (queda 1 min, retorno 10 s, treino com página aberta).

## Onde está cada coisa
| O quê | Onde |
|---|---|
| Estado oficial, revisões pendentes (R-xxx), testes | `developer-guide/architecture/project-state.md` |
| Roadmap de execução (M0–M8) / ordem oficial dos módulos | `developer-guide/architecture/roadmap-to-playable.md` / `roadmap.md` |
| Decisões datadas (DEC-) | `developer-guide/architecture/decision-log.md` |
| ADRs (próximo livre: ADR-059) | `developer-guide/adr/` |
| Contratos | `docs/contracts/active/PV-<ÁREA>-NNN.md` |
| Documentos de domínio (critério de LOCK) | `developer-guide/domain/` |
| Relatórios de filas encerradas | `docs/architecture/historico/` |
| Arquivos apagados/movidos | `docs/architecture/CLEANUP-MANIFEST-*.md` |
| Código | `src/` (Domain, Application, Infrastructure, Persistence, Composition, API, Client) · testes em `tests/` |

## Próxima fila
Fila 8 (em execução): `PV-GOV-012`, `PV-HUNT-013`, `PV-RUNTIME-012`, `PV-RUNTIME-013`, `PV-EVENT-012`, `PV-QA-007`, `PV-GOV-013`. Relatório: `docs/architecture/RELATORIO-FILA-8.md`.
Depois (a decidir pelo PO): `PV-TRAIN-010`, cliente UI-1 (`nextEncounter`, `leave`, parar heartbeat no logout), skills de monstro (R-043/R-050), 2ª área de Hunt.
