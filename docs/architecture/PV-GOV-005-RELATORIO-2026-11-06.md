# PV-GOV-005 — Relatório somente leitura (2026-11-06)

Contrato: ordem do Product Owner (modo noite, item 6). **Nenhuma mudança foi aplicada** nos arquivos analisados; este documento só registra achados e propostas. Cada proposta precisa de decisão (Arquiteto ou PO, conforme indicado) e de contrato próprio para ser aplicada.

Método: `git ls-files`, `git grep`, `git log --all` (inclusive arquivos apagados), `find` no disco. Branch: `chief/m1-gov-005`.

---

## (a) Lacunas de ADR: 026, 044, 048 e 055

### Fatos verificados

| ADR | Arquivo hoje | Histórico git | Quem cita |
|---|---|---|---|
| 026 | não existe | **nunca existiu** (0 commits com `*ADR-026*`) | só documentos de governança: `roadmap-to-playable.md` (M0), `project-state.md` (R-008), `PV-ANALYSIS-2026-11-05.md` §4.6 |
| 044 | não existe | **nunca existiu** | idem |
| 048 | não existe | **nunca existiu** | idem |
| 055 | `developer-guide/adr/ADR-055-event-driven-quests.md`, **0 bytes** | vazio em todos os commits (`40d5ea7`, `0aa823f`, `9f54994`) | `CLEANUP-MANIFEST-2026-11-05.md` ("vazios a preencher, não apagar"), `PV-ANALYSIS` |

Nenhum código (`src/`, `tests/`) cita ADR-026, 044, 048 ou 055. O `architecture-index.md` lista só ADR-001…004.

Vizinhos (todos ACCEPTED): 024 Centralized Reward Pipeline · 025 Experience Table Interpretation Model · 027 Derived Level Model · 028 Permanent vs Derived State · 042 Companion Service · 043 Pet Instance Model · 045 Hunt Orchestrator · 046 Hunt Context and Result · 047 Spawn Lifecycle · 049 Typed Event Bus · 050 Domain Events · 053 NPC Interaction · 054 NPC Spawn Integration · 056 Quest Completion Policy · 057 Hunt Session × Runtime State.

### Propostas

**ADR-026 — proposta: "Ascension Model"** (hipótese com evidência indireta)
- Evidência: "Ascension" é usada como conceito já existente por ADR-023, 025, 027, 028, 029 e 031 ("Cada Ascension referencia sua própria Experience Table" — ADR-025:25; Level derivado de "Experience + Ascension" — ADR-027:26), mas **nenhum ADR define o que é uma Ascension**. O número 026 fica exatamente entre a Experience Table (025) e o Level derivado (027).
- Texto proposto (rascunho, Status PROPOSED):
  > **Contexto:** ADR-023/025/027/028 tratam Ascension como parte da progressão, sem defini-la. **Decisão:** Ascension é um estágio de progressão permanente do Character, persistido como estado permanente (ADR-028); cada Ascension referencia uma Experience Table própria (ADR-025); o Level é derivado de Experience + Ascension + Experience Table (ADR-027). A mudança de Ascension acontece só por um caso de uso explícito, passando pelo Reward/Progression Pipeline (ADR-024). **Em aberto (PO):** quantas Ascensions existem, o que dispara a mudança e o que muda para o jogador.
- **A mecânica (o que uma Ascension significa no jogo) é decisão do PO**; o ADR só fixa o modelo de dados e quem é dono do estado.

**ADR-044 — proposta: declarar "número não utilizado"**
- Sem nenhuma evidência no código, nos documentos ou no histórico de qual decisão ocuparia o 044 (entre Pet Instance Model e Hunt Orchestrator). Inventar um tema seria achismo (`CLAUDE.md`).
- Texto proposto: arquivo `ADR-044-reserved.md` com `Status: NOT USED` e uma frase: "Número não utilizado na série original; mantido para não renumerar ADRs existentes. Registrado em PV-GOV-005."

**ADR-048 — proposta: declarar "número não utilizado"**
- Mesma situação do 044 (entre Spawn Lifecycle e Typed Event Bus). Um candidato plausível seria "`EngineEvent` como base de eventos" (ADR-049:19 diz "Todos os Events derivam de EngineEvent" sem ADR próprio), mas isso já está decidido dentro do ADR-049 — não justifica um ADR separado.
- Texto proposto: idem ao 044.

**ADR-055 — proposta: preencher "Event-Driven Quests"** (título já existe no nome do arquivo)
- Evidência: ADR-049 (Typed Event Bus), ADR-050 (Domain Events: "o Event Bus transporta exclusivamente Domain Events") e ADR-056 (cada Quest Definition tem uma Completion Policy; "a decisão de conclusão pertence exclusivamente à Completion Policy"). O `roadmap-to-playable.md` (M7, `PV-EVENT-011`) cita "`CombatCompletedEvent → Reward → (futuro) Achievement/Quest`".
- Texto proposto (rascunho, Status PROPOSED):
  > **Contexto:** Quests precisam reagir a acontecimentos do jogo (monstro derrotado, item obtido) sem que Combat, Loot ou Reward conheçam Quests. **Decisão:** o progresso de Quest é atualizado **somente** por assinatura de Domain Events (ADR-050) no Typed Event Bus (ADR-049). Nenhum sistema chama o módulo de Quest diretamente. A conclusão é decidida pela Completion Policy da Quest Definition (ADR-056). Recompensas de Quest passam pelo Reward Pipeline (ADR-024). **Restrições:** Quest não altera estado de outro agregado (Aggregate Ownership); eventos são tipados, nunca string. **Dependência:** só implementável depois do M7 (Event System).

**Decisão necessária:** Arquiteto (044, 048, estrutura do 026 e do 055); **PO** para a parte de mecânica do 026 (o que é Ascension no jogo).

---

## (b) Arquivos com nome corrompido (`#U`) em `docs/Visual-Identity`

### Fatos verificados

- **Nenhum arquivo com `#U` no nome existe** no repositório: nem no índice do git, nem no disco (`find . -name "*#U*"`, fora de `node_modules/bin/obj`), nem em nenhum commit do histórico (`git log --all --name-only`).
- Hipótese para o que foi visto: algumas ferramentas de zip no Windows gravam caracteres fora do ASCII como `#U` + código Unicode. O repositório tem exatamente **4 arquivos com travessão "—" (U+2014)** no nome, que nessas ferramentas aparecem como `#U2014`. Em `docs/Visual-Identity` são 2 deles.

Todos os nomes não ASCII do repositório (5):

| Arquivo | Caractere |
|---|---|
| `docs/Visual-Identity/Antigos/PV-ASSET-001 — Technical Specification.md` | U+2014 travessão + espaços |
| `docs/Visual-Identity/Antigos/PV-UI-010 — Revision Integration Contract.md` | U+2014 travessão + espaços |
| `Documentation/Contracts/VerticalSlice/PV-VS-007 — Combat Experience Layer.md` | U+2014 travessão + espaços |
| `docs/architecture/master/contracts/vertical-slice/PV-VS-009A — Monster Loot Reward Integration.md` | U+2014 travessão + espaços |
| `docs/engineering/Abordagem IA Produção.docx` | `ç`, `ã` + espaços |

### Proposta de mapa de renomeação (não aplicado)

| De | Para | Quem cita o nome atual (atualizar junto) |
|---|---|---|
| `docs/Visual-Identity/Antigos/PV-ASSET-001 — Technical Specification.md` | `docs/Visual-Identity/Antigos/PV-ASSET-001-TECHNICAL-SPECIFICATION.md` | `PV-ART-002-ASSET-ROADMAP.md`, `PV-ASSET-001-INTEGRATION-CONTRACT.md` |
| `docs/Visual-Identity/Antigos/PV-UI-010 — Revision Integration Contract.md` | `docs/Visual-Identity/Antigos/PV-UI-010-REVISION-INTEGRATION-CONTRACT.md` | `PV-UI-010-CORRECTION-CONTRACT.md`, `PV-UI-010-INTEGRATION-CONTRACT.md`, `PV-UI-010-LOGIN-EXPERIENCE-SPECIFICATION.md`, `PV-UI-010-TWO-LAYER-INTEGRATION-CONTRACT.md` |
| `Documentation/Contracts/VerticalSlice/PV-VS-007 — Combat Experience Layer.md` | `Documentation/Contracts/VerticalSlice/PV-VS-007-Combat-Experience-Layer.md` | `docs/vertical-slice/work-items/PV-VS-007.md`, `PV-VS-008.md`, `PV-VS-009.md`, `PV-VS-011.md` |
| `docs/architecture/master/contracts/vertical-slice/PV-VS-009A — Monster Loot Reward Integration.md` | `.../PV-VS-009A-Monster-Loot-Reward-Integration.md` | ninguém além do próprio arquivo |
| `docs/engineering/Abordagem IA Produção.docx` | `docs/engineering/Abordagem-IA-Producao.docx` | ninguém |

Padrão proposto: ID do contrato + hífen + título, sem espaços e só ASCII, seguindo o padrão já usado pelos vizinhos em `docs/Visual-Identity/Antigos/` (maiúsculas) e em `Documentation/Contracts/` (Title-Case). Aplicar com `git mv` (preserva histórico) e atualizar as citações no mesmo commit. Atenção: `Documentation/` é raiz paralela e "contratos citam o próprio caminho" (`CLEANUP-MANIFEST-2026-11-05.md`) — o renomeio ali precisa de decisão explícita.

**Decisão necessária:** Arquiteto (é organização de documentação). Recomendação: aplicar só os 2 de `docs/Visual-Identity` e o `.docx`; deixar `Documentation/` e o `PV-VS-009A` para quando as duas raízes de documentação forem unificadas.

---

## (c) Referências a `docs/CURRENT_CONTEXT.md`

`docs/CURRENT_CONTEXT.md` declara a si mesmo "Documento Histórico… deixou de representar o estado atual do projeto". O estado oficial é `developer-guide/architecture/project-state.md` (`CLAUDE.md`, ordem de leitura). Referências que ainda o tratam como atual — **4 documentos, 5 linhas** (é a pendência R-006):

| Documento | Status do documento | Linha | Texto atual | Proposta de texto |
|---|---|---|---|---|
| `docs/AI_WORKFLOW.md` | **LOCKED** | 35 | "2. docs/CURRENT_CONTEXT.md" (ordem de leitura antes de cada Sprint) | "2. developer-guide/architecture/project-state.md" |
| `docs/AI_WORKFLOW.md` | **LOCKED** | 142 | "Ao finalizar uma Sprint atualizar: docs/CURRENT_CONTEXT.md" | "developer-guide/architecture/project-state.md" |
| `docs/PROJECT_CONTEXT.md` | **LOCKED** | 99 | "2. CURRENT_CONTEXT.md" | "2. developer-guide/architecture/project-state.md" |
| `README_DEVELOPMENT.md` | sem status | 51 | "CURRENT_CONTEXT.md representa sempre o estado atual da Sprint." | "O estado atual oficial está em `developer-guide/architecture/project-state.md`; `docs/CURRENT_CONTEXT.md` é histórico." |
| `docs/checklists/architecture-checklist.md` | sem status | 9 | "□ Atualiza CURRENT_CONTEXT?" | "□ Atualiza developer-guide/architecture/project-state.md?" |

Citações **corretas** (falam do arquivo como histórico; não mudar): `CLAUDE.md:18`, `project-state.md` (R-006), `CLEANUP-MANIFEST-2026-11-05.md:29`, `PV-ANALYSIS-2026-11-05.md:77`.

**Decisão necessária:** `AI_WORKFLOW.md` e `PROJECT_CONTEXT.md` são **LOCKED** — só mudam por revisão arquitetural documentada (`roadmap.md`, `CLAUDE.md`). Recomendação: um contrato `PV-GOV-0xx` que faça a revisão formal das 5 linhas de uma vez, com nota de emenda nos dois LOCKED (mesmo padrão usado em ADR-011/012/004), e marque R-006 como resolvida.
