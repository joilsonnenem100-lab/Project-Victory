# Revisão arquitetural de fechamento — Fila 7 (M6 sem treino e M7)

- **Data:** 2026-11-06 (Fila 7, item 7 — `PV-GOV-011`)
- **Responsável:** Arquiteto-Chefe (Claude), com validação do `pv-architecture-reviewer`.
- **Critério** (único, desde a revisão de fechamento do M4, `DEC-20261106-38`): um módulo só fica **LOCKED** quando o seu **documento de domínio**, que é o contrato arquitetural do módulo, está cumprido e não há pendência que mude o seu modelo. Se faltar algo, o módulo fica em **REVIEW** com a lista do que falta.

## Resultado

| Módulo | Antes | Agora | Falta para o LOCK |
|---|---|---|---|
| Monster AI (Fase 6) | REVIEW | **REVIEW** | R-050 (cooldowns, condições, pesos e contexto do `monster-ai.md`) e R-043 (habilidade de monstro neutra) — nada disso entrou na Fila 7 |
| Experience Distribution (Fase 6) | REVIEW | **REVIEW** | R-045 (participantes elegíveis, distribuição atômica) — depende da Hunt em grupo |
| Spawn System (Fase 6) | REVIEW | **REVIEW** | R-044 ✅ e R-046 ✅ fechados na Fila 7. Faltam R-057: Respawn Policy, Spawn Delay e Spawn Definition versionada |
| Game Session + Runtime Coordinator (M6, novo) | — | **REVIEW** | R-051 (área da última Hunt), R-054 (heartbeat depois do logout), R-055 (EnterGame × Recovery) |
| Hunt Loop + encerramento por queda (M6, novo) | — | **REVIEW** | R-059 (`hunt.md`: estados e Hunt Result), R-052 (Hunt duplicada, decisão do PO), R-053 (caminho legado `gameplay/action`), R-027 (Hunt Session persistida fora do pipeline), R-030, R-060 (escopo por queda) |
| Recovery do Runtime (M6, novo) | — | **REVIEW** | R-055 e R-056 (escalabilidade e ordem) |
| Event System — Typed Event Bus e Domain Events (M7, novo) | — | **REVIEW** | R-058 (`events.md`: Timestamp; bus síncrono × handlers assíncronos) |

**Nenhum módulo foi travado nesta fila.** A Fase 6 continua IN PROGRESS.

Os módulos novos do M6/M7 não estavam nas fases do `roadmap.md`. A revisão os registra numa seção própria ("Capacidades de Runtime e Eventos"), sem criar fase nova, que é decisão de roadmap e ficaria para revisão própria.

## Evidências (Fila 7)

- **Contratos:** `PV-RUNTIME-010`, `PV-HUNT-011`, `PV-HUNT-012`, `PV-RUNTIME-011`, `PV-EVENT-010-011`, `PV-QA-006` (`docs/contracts/active/`). Cada um foi revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS), e as ressalvas foram aplicadas na branch seguinte ou registradas.
- **Regras de jogo cumpridas e provadas pela API real** (`PV-QA-006`):
  - Hunt contínua;
  - queda de exatamente 60 s não encerra;
  - queda de 61 s encerra;
  - retorno depois de **mais** de 10 s online com uma nova Hunt na última Hunt;
  - reinício sem Hunt.
- **Fechados na fila:** R-036, R-044, R-046, R-048 e R-029 (este, para `api/hunt/*`).
- **Build e testes:** 0 avisos, **380 aprovados** (executado, depois das ressalvas finais).

## Alterações feitas por esta revisão

- `developer-guide/architecture/roadmap.md`: a linha do Spawn System na Fase 6 foi atualizada (R-044/R-046 fechados; o que falta passa a ser o R-057), e foi acrescentada a seção "Capacidades de Runtime e Eventos (M6/M7)", com os módulos em REVIEW.
- `developer-guide/architecture/project-state.md`: R-057, R-058 e R-059.
