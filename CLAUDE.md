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

`docs/CURRENT_CONTEXT.md` é **histórico** (ele mesmo declara isso). As referências antigas a ele em `docs/PROJECT_CONTEXT.md`, `docs/AI_WORKFLOW.md`, `README_DEVELOPMENT.md` e `docs/checklists/architecture-checklist.md` foram corrigidas no `PV-GOV-007` (`DEC-20261106-13`) e hoje apontam para `project-state.md` (texto antigo tachado).

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
dotnet test  ProjectVictory.sln                        # todos os projetos de teste (abaixo)
dotnet user-secrets set "Jwt:Key" "<chave com 32+ caracteres>" --project src/ProjectVictory.API   # uma vez, antes do primeiro run
dotnet run   --project src/ProjectVictory.API          # Development: aplica as migrations no startup; CORS permite http://localhost:5173
cd src/ProjectVictory.Client && npm install && npm run dev
```
- **Sem `Jwt:Key` a API não sobe** em nenhum ambiente (`PV-PLATFORM-002`). Em Development a chave vem de **user-secrets**; fora dele, da variável `Jwt__Key`. Guia: `developer-guide/guides/configuracao-local.md`. Nunca escreva a chave no repositório (é público).
- **Projetos de teste** (todos no `.sln`): `ProjectVictory.Application.Tests`, `ProjectVictory.Domain.Tests`, `ProjectVictory.Runtime.Tests`, `ProjectVictory.SmokeTests` (DI + InMemory/SQLite) e `ProjectVictory.Api.Tests` (API real via `WebApplicationFactory`, chave JWT aleatória por execução).
- **Baseline atual:** a contagem oficial de testes fica em `project-state.md` (seção de testes automatizados); em 2026-11-06, depois do M2, eram **161 aprovados, 0 ignorados, 0 falhas**, com build sem avisos.
- Nunca declare "build passou" ou "testes verdes" sem tê-los executado; diga "não executado" e peça o resultado. Ao fim de cada contrato, confirme que não sobrou processo `dotnet`/`ProjectVictory.API` (`dotnet build-server shutdown`; no PowerShell: `Get-Process dotnet,ProjectVictory.API`).

## Convenções
- Documentação e comentários em português; identificadores em inglês.
- IDs: contratos `PV-<ÁREA>-NNN`, work items `PV-VS-NNN`, ADRs `ADR-NNN` (**próximo livre: ADR-058**). ADR-044 e ADR-048 são **NOT USED** (reservados, não reutilizar); ADR-026 ACCEPTED (`PV-GOV-006`); ADR-055 PROPOSED (`PV-GOV-007`). Decisões datadas do PO/Arquiteto: `DEC-AAAAMMDD-NN` no `decision-log.md`.
- Commits pequenos, mensagem `PV-<ID> - <resumo>`.
- Registro do que foi apagado/movido: `docs/architecture/CLEANUP-MANIFEST-2026-11-05.md`.

## Execução autônoma em fila (MODO NOITE / MODO FILA)
Protocolo usado quando o PO manda executar uma fila de contratos sem parar (ordens de 2026-11-06):
- **Branches empilhadas:** a primeira nasce da última branch pronta; cada seguinte nasce da anterior. Push de cada branch; **nunca** `push --force`, nunca alterar a `main` (o merge é do PO), nunca apagar arquivos sem autorização explícita, nunca editar migrations existentes, nunca escrever segredos.
- **Por contrato:** plano curto → implementação → `dotnet build` → `dotnet test ProjectVictory.sln` → `pv-architecture-reviewer` → aplicar ressalvas (podem entrar na branch seguinte, se a revisão chegar depois) → atualizar `project-state.md` e `roadmap-to-playable.md` → commits `PV-<ID> - <resumo>` → confirmar que não restou processo `dotnet`.
- **Se travar** por decisão de PO, ação destrutiva ou conflito com ADR/LOCKED sem solução documental: registrar no relatório, pular e seguir. Não inventar solução.
- **Relatório final** em `docs/architecture/` na última branch (ex.: `RELATORIO-NOITE-2026-11-06.md`, `RELATORIO-M2.md`, `RELATORIO-FILA-2.md`), com status de cada contrato, build/testes, achados do revisor, o que travou e decisões de PO pendentes; depois push e um resumo curto ao PO.
- **Worktrees:** se o checkout principal for trocado por fora da sessão (já aconteceu com uma IDE aberta), trabalhar num `git worktree` separado (`git -c core.longpaths=true worktree add ...`).
