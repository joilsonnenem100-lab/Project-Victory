# Project-Victory — Memória do Projeto (Claude Code)

MMORPG Idle inspirado em Tibia: progressão automática por Hunts. Backend ASP.NET (net10.0), frontend React 19 + Vite + TypeScript, SQLite via EF Core.

## Papéis
- **Claude (sessão principal) = Desenvolvedor Chefe + Chefe de Arquitetura e Engenharia.** Decide, delega, revisa, valida e responde pelo resultado.
- **Equipe** (subagentes em `.claude/agents/`): `pv-domain-engineer`, `pv-application-engineer`, `pv-persistence-engineer`, `pv-api-security-engineer`, `pv-frontend-engineer`, `pv-qa-engineer`, `pv-docs-contracts-keeper`, `pv-architecture-reviewer`.
- **Dono do produto (Product Owner) = o usuário.** Por `decision-log.md` (ADR-016): PO decide Game Design, mecânicas, experiência do jogador e universo; o Arquiteto (Claude) decide arquitetura, engenharia, organização dos módulos, roadmap, documentação e revisões arquiteturais. Questões que misturam as duas (ex.: a Hunt sobrevive à desconexão? — ADR-057) têm a parte de mecânica decidida pelo PO.

## Ordem de leitura ao iniciar qualquer sessão
1. `CLAUDE.md` (este arquivo)
2. `developer-guide/architecture/project-state.md` (estado oficial — v2.0, verificado contra o código)
3. `developer-guide/architecture/roadmap-to-playable.md` (roadmap de execução) e `developer-guide/architecture/roadmap.md` (ordem oficial dos módulos)
4. `developer-guide/architecture/architecture-rules.md` (20 regras LOCKED)
5. `docs/architecture/master/PROJECT-VICTORY-ARCHITECTURE-HANDBOOK-V3.md` + `docs/architecture/master/PROJECT-VICTORY-ARCHITECTURE-RULES.md`
6. ADRs relevantes em `developer-guide/adr/` e o contrato da tarefa.

`docs/CURRENT_CONTEXT.md` é **histórico** (ele mesmo declara isso). Ignore as referências antigas a ele que ainda existem em `PROJECT_CONTEXT.md`, `AI_WORKFLOW.md` e `architecture-checklist.md`.

## Hierarquia da fonte da verdade (do próprio projeto)
1. Código oficial aprovado → 2. Contratos e decisões arquiteturais (ADRs) → 3. Documentação oficial → 4. Master Architecture Map.
Se duas fontes conflitam, **não escolha por intuição**: registre o conflito em `project-state.md` (seção Revisões Pendentes) e resolva por ADR.

## Regras inegociáveis
- **Nada de achismo.** Toda decisão cita o documento que a sustenta (ADR, contrato, regra, Handbook). Sem citação, não é decisão — é pergunta ao dono.
- **Contrato antes de código.** Fluxo: Arquitetura → Contrato → Engenharia → Implementação → Build → Teste → Validação → Aceite. Engenharia não toma decisão arquitetural.
- **Aditivo.** Implementar = adicionar. Nunca remover/substituir comportamento validado, endpoint, DTO, handler, tela ou persistência sem decisão arquitetural documentada + contrato + validação de impacto (`PROJECT-VICTORY-ARCHITECTURE-RULES.md`).
- **Módulo LOCKED só muda por revisão arquitetural documentada** (`roadmap.md`).
- **Server Authority:** o cliente solicita, o servidor valida e decide. Identidade vem do token (`ICurrentPlayerContext`), nunca do corpo da requisição.
- **Aggregate Ownership:** um sistema não altera o estado interno de outro (Combat → Reward → Progression → Character).
- **Clean Architecture:** Domain não conhece infraestrutura; Application coordena casos de uso; Infrastructure/Persistence servem ao domínio; API só traduz HTTP.
- **Orquestradores nunca implementam regra de outro domínio** (ADR-045, ADR-051).
- **Experience é a única verdade; Level é derivado** (ADR-023, 025, 027, 028). Todo ganho permanente passa pelo Reward Pipeline (ADR-024).

## Definition of Done (toda entrega)
Contrato citado · diff só no escopo autorizado · `dotnet build` verde · testes novos/ajustados verdes · fluxos validados anteriores intactos (smoke) · documentação e `project-state.md` atualizados · revisado por `pv-architecture-reviewer`.

## Comandos
```
dotnet build ProjectVictory.sln
dotnet test  ProjectVictory.sln
dotnet test  tests/ProjectVictory.SmokeTests          # já incluído no .sln desde PV-QA-001 parte 1 (1 teste com Skip até PV-REWARD-010)
dotnet run   --project src/ProjectVictory.API          # API (CORS permite http://localhost:5173)
cd src/ProjectVictory.Client && npm install && npm run dev
```
O ambiente de nuvem usado na análise inicial **não tem `dotnet`**: build e testes só são comprováveis na máquina do dono. Nunca declare "build passou" sem tê-lo executado; diga "não executado" e peça o resultado.

## Convenções
- Documentação e comentários em português; identificadores em inglês.
- IDs: contratos `PV-<ÁREA>-NNN`, work items `PV-VS-NNN`, ADRs `ADR-NNN` (próximo livre: ADR-058; lacunas 044, 048 e ADR-055 vazio são pendências, não números livres; ADR-026 preenchido em PV-GOV-006).
- Commits pequenos, mensagem `PV-<ID> - <resumo>`.
- Registro do que foi apagado/movido: `docs/architecture/CLEANUP-MANIFEST-2026-11-05.md`.
