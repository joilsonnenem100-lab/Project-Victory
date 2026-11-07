# Project-Victory — INDEX (leia primeiro)

Uma página. Atualizado em 2026-11-07 (Fila 9, Fila Noturna 1 e Fila 10). Detalhe só quando o item de trabalho o citar.

## Estado atual
- **Jogável pela API:** conta → login → personagem → entrar → Hunt contínua (combate por turnos, IA de monstro, Spawn) → XP/loot reais → Level derivado → queda/logout/reinício sem perda.
- **Marcos:** M0–M5 concluídos; M6/M7 entregues — **Recovery e Event System LOCKED** (Fila 8); **Monster AI LOCKED** (Fila 10, `DEC-20261107-06`); Game Session e Hunt Loop em REVIEW; `PV-TRAIN-010` e M8 (UI/Primeiro Teste Jogável) pendentes.
- **Baseline:** build 0 avisos; **528 testes** verdes no fim da Fila 10 (contagem oficial em `project-state.md`). Migrations: 25 (a última, `CharacterAccountRules`, aditiva).
- O que falta para cada LOCK fica no `roadmap.md` (seção "Capacidades de Runtime e Eventos") e em `docs/architecture/historico/REVISAO-FECHAMENTO-FILA-8.md`.

## Regras vivas (resumo; texto completo no `CLAUDE.md`)
**Game Design:** `docs/product/PO-DECISOES-BASE.md` é a fonte da verdade de Game Design; só o PO altera; toda alteração atualiza este arquivo, o `decision-log.md`, os documentos afetados (por nota de superação, sem apagar histórico) e o `INDEX.md` (`DEC-20261107-01`; comando `/po-decisao`).
Server Authority (identidade do token) · Aggregate Ownership · Clean Architecture · Regra Aditiva · Contrato antes de código · Experience é a verdade, Level derivado · Referência Tibia (`DEC-20261106-15`) · Nada de achismo (cite a fonte) · Economia de contexto e uma fila por sessão (`DEC-20261106-45`).

## Fila 10 (2026-11-07)
PO-56 a PO-61 registrados (`PO-DECISOES-BASE.md` v1.2); **Monster AI LOCKED**. Servidor: tier/Continente informativos + `GET api/hunt/catalog` (R-064), cura igual para todas as vocações (PO-59), recompensa diária por conta no `enter` (R-068), regras de personagem da conta — 5 slots (3+2 bloqueados), um por classe, nome Tibia, exclusão em 7 dias (R-066), `ExplorationDecisionHandler` e órfãos do legado apagados, `enter` sem personagem → 404 (R-069). **528 testes**, 2 migrations aditivas novas (`DailyRewardClaims`, `CharacterAccountRules`; total 25). Relatório: `docs/architecture/RELATORIO-FILA-10.md`.

## Fila Noturna 1 (2026-11-07)
Servidor: Hunt Analyzer (`api/hunt/analyzer`), admin do XP rate (`api/admin/xp-rate`, admin só por Id em `Admin:Accounts`), spell de cura das 5 classes (PO-53), poções em 3 níveis e kit inicial (PO-15/16), `api/gameplay/action` retirado. **480 testes**, 3 migrations aditivas novas (`HuntAnalyzerSummaries`, `ServerExperienceRate`, `PotionTiers`). Playtest: `docs/playtest/`. Relatório: `docs/architecture/RELATORIO-FILA-NOTURNA-1.md`.

## Últimas decisões (`developer-guide/architecture/decision-log.md`)
- `DEC-20261107-05` — PO-56 a PO-61 (`PO-DECISOES-BASE.md` v1.2): Continente livre; gemas = cosméticos + 2 slots + passes; Monster AI atual oficial; cura igual para todas as vocações (sem +5 do Druid); recompensa diária por configuração; remoção do `ExplorationDecisionHandler`.
- `DEC-20261107-06` — **Monster AI LOCKED** (revisão formal, régua do PO-58); R-050/R-043 suspensos.
- `DEC-20261107-02` — Varredura de consistência (revisão formal): notas de superação; conflitos com código R-064 (tier ainda bloqueia), R-065 (sem spell de cura), R-066 (sem limite de personagens).
- `DEC-20261107-03` — `ADR-059`: Hunt Loop + Event Bus fazem a cadeia (Handbook §10 emendado); Game Session continua REVIEW.
- `DEC-20261107-04` — Remoção autorizada do `api/gameplay/action` (R-053), começando por teste funcional.
- `DEC-20261107-01` — Adoção do `PO-DECISOES-BASE.md` (PO-01 a PO-55); PO-53 cura por spell em todas as vocações ("sem suprimento não há cura" só para poções); PO-54 dinheiro compra tempo; PO-55 `RequiredTitleTier` informativo.
- `DEC-20261106-46` — Segunda Hunt com outra em andamento → **409**, sem encerrar a anterior (R-052).
- `DEC-20261106-47` — Reinício do servidor = queda: retorno à última Hunt após 10 s online, nova Hunt Session.
- `DEC-20261106-48` — Vida 0 no checkpoint → volta com vida e Mana cheias na cidade (**PROVISÓRIO**, R-055).
- `DEC-20261106-49` — Logout = saída manual (Hunt `Completed`), sem retorno automático.
- `DEC-20261106-50/51` — `ADR-058` (bus síncrono = aguardado; notificação isolada); revisão de LOCK da Fila 8.
- Anteriores relevantes: ADR-057 (Hunt termina ao desconectar; sem Hunt offline), `DEC-20261106-20/21/22` (queda 1 min, retorno 10 s, treino com página aberta).

## Onde está cada coisa
| O quê | Onde |
|---|---|
| **Decisões de Game Design do PO (fonte da verdade)** | `docs/product/PO-DECISOES-BASE.md` |
| Estado oficial, revisões pendentes (R-xxx), testes | `developer-guide/architecture/project-state.md` |
| Roadmap de execução (M0–M8) / ordem oficial dos módulos | `developer-guide/architecture/roadmap-to-playable.md` / `roadmap.md` |
| Decisões datadas (DEC-) | `developer-guide/architecture/decision-log.md` |
| ADRs (próximo livre: ADR-060) | `developer-guide/adr/` |
| Contratos | `docs/contracts/active/PV-<ÁREA>-NNN.md` |
| Documentos de domínio (critério de LOCK) | `developer-guide/domain/` |
| Relatórios de filas encerradas | `docs/architecture/historico/` |
| Arquivos apagados/movidos | `docs/architecture/CLEANUP-MANIFEST-*.md` |
| Código | `src/` (Domain, Application, Infrastructure, Persistence, Composition, API, Client) · testes em `tests/` |

## Próxima fila
Fila 10 **concluída** (branches empilhadas `feature/f10-*`; a última, `feature/f10-gov-021`, traz tudo). Candidatos (decide o PO): seleção de personagem no `enter` (R-072), cliente (catálogo de Hunts, slots/criação/exclusão, recompensa diária, Analyzer, cura), R-073 (exclusão × Hunt ativa, pergunta ao PO), `PV-TRAIN-010`.
