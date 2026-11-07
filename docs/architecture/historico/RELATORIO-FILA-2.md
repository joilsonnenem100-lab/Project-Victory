# Relatório da Fila 2 (2026-11-06)

Ordem: "MODO FILA 2" (Product Owner e Arquiteto-Chefe), mais o item 8 (`PV-CLEAN-001`), autorizado pelo PO durante a execução. Regras do modo noite:
- branches empilhadas a partir de `chief/m2-state-001`;
- push de cada branch;
- sem `push --force` e sem alterar a `main`;
- sem apagar arquivos não autorizados e sem editar migrations;
- sem segredos.

**Resultado: os 8 contratos foram concluídos e enviados ao `origin`.** Nenhum foi pulado e nenhum travou. Basta **um merge**: `chief/m1-clean-001` na `main`, porque ele contém todos os anteriores, inclusive o M2. A `origin/main` continua em `8b7caab`.

## Contratos

| # | Contrato | Branch (commit) | Revisão | Testes ao fim |
|---|---|---|---|---|
| 1 | `PV-GOV-007` ADRs 044/048 reservados, 055 PROPOSED, `CURRENT_CONTEXT` (R-006), Hunt Offline (R-013) — revisão formal dos LOCKED (`DEC-20261106-13`) | `chief/q2-gov-007` (`0541229`) | APROVADO COM RESSALVAS (aplicadas) | 161 (sem código) |
| 2 | `PV-GOV-008` `CLAUDE.md` e `project-state.md` no estado real | `chief/q2-gov-008` (`c354252`) | APROVADO COM RESSALVAS (aplicadas) | 161 (sem código) |
| 3 | `PV-QA-003` 403 por HTTP (runtime, hunt-session, encounter), token expirado e token sem claim | `chief/q2-qa-003` (`25d96a1`) | APROVADO COM RESSALVAS (aplicadas) | 166 |
| 4 | `PV-ARCH-001` composição de DI única (R-014) — projeto `ProjectVictory.Composition` (`DEC-20261106-14`) | `chief/q2-arch-001` (`90911d0`) | APROVADO COM RESSALVAS (aplicadas) | 167 |
| 5 | `PV-PLATFORM-004` migrations fora de Development só com `Database:ApplyMigrationsOnStartup` (padrão desligado) + guia de deploy | `chief/q2-platform-004` (`708667a`) | APROVADO COM RESSALVAS (aplicadas) | 173 |
| 6 | `PV-PLATFORM-005` cliente: `@types/react*`, `npm audit` 0, script `typecheck` | `chief/q2-platform-005` (`0b47ca1`) | APROVADO COM RESSALVAS (aplicadas) | 173 (+ build do cliente verde) |
| 7 | `PV-CI-001` `.github/workflows/ci.yml` | `chief/q2-ci-001` (`9c91392`) | APROVADO COM RESSALVAS (aplicadas) | 173 + **CI verde no GitHub** |
| 8 | `PV-CLEAN-001` remoção dos 42 arquivos em quarentena (R-007) | `chief/m1-clean-001` (`38d1be7` só a remoção; documentação em seguida) | APROVADO COM RESSALVAS (aplicadas) | 173 = 173 |

**Build e testes no último branch:** 0 avisos e 0 erros. **173 aprovados, 0 ignorados, 0 falhas**, assim distribuídos:

| Projeto | Testes |
|---|---|
| Domain | 68 |
| Runtime | 17 |
| Application | 1 |
| Smoke | 26 |
| Api | 61 |

Além disso:
- os revisores rodaram build e testes por conta própria nos contratos 3, 4, 5 e 8;
- o build do cliente está verde e o `npm audit` não aponta vulnerabilidades;
- não sobrou nenhum processo `dotnet` ao fim de cada contrato.

**CI no GitHub:** o push do workflow foi **aceito**, sem bloqueio de permissão.
- Execução 42225918358 (`chief/q2-ci-001`): **success**.
- Execução 42226163716 (`chief/m1-clean-001`): **success**.
- O número de testes no Linux não foi conferido, porque o artefato exige autenticação.

## O que mudou, em uma linha cada

1. **ADRs:**
   - 044 e 048: reservados (NOT USED);
   - 055 Event-Driven Quests: PROPOSED;
   - 026 Ascension: já estava ACCEPTED desde o `PV-GOV-006`.

   As 5 referências a `CURRENT_CONTEXT` e a Hunt Offline foram corrigidas com o texto antigo tachado, sem apagar nada.
2. **`CLAUDE.md`:** user-secrets antes do `dotnet run`, os 5 projetos de teste, o próximo ADR livre (058) e o protocolo de execução em fila.
3. **Testes de segurança novos:** outra conta recebe 403 em runtime, hunt-session e encounter; token expirado recebe 401. **Achado:** um token válido sem a claim da conta recebe **500**, quando deveria receber 401 (**R-018**). Nunca devolve dados.
4. **DI:** `Program.cs` e testes usam o mesmo `AddProjectVictoryCore`. Os registros foram conferidos por script: nenhum mudou.
5. **Banco fora de Development:** as migrations só são aplicadas com a chave ligada (padrão desligado). Guia novo: `developer-guide/guides/deploy.md`.
6. **Cliente:**
   - vulnerabilidades: 6 → 0;
   - erros de `tsc`: 444 → 7 (código das telas, lista no contrato);
   - versões: dependências diretas só em patch; indiretas em patch ou minor, dentro das faixas.
7. **CI:** build e testes .NET 10 em Release, mais o build do cliente, em todo push e pull request. O typecheck é informativo e mostra um aviso.
8. **Limpeza:** os 42 arquivos foram removidos num commit isolado e reversível (`git revert 38d1be7`). O `grep` confirmou zero referências antes de remover. Os caminhos estão no `CLEANUP-MANIFEST`.

## Principais achados dos revisores (já aplicados ou registrados)

- **Status no momento certo:** contratos marcados como "CONCLUÍDO" antes da revisão passaram a registrar o parecer.
- **Testes de controle:** o dono recebe 200 depois do 403 da outra conta; um token válido do mesmo gerador recebe 200.
- **Limpeza do `Program.cs`:** BOM removido, `using`s sem uso removidos.
- **Projeto `Composition` registrado** no decision-log e no `project-state.md`.
- **Pré-requisitos do `dotnet ef` documentados** no guia de deploy: `dotnet-ef` instalado e `Jwt__Key` no ambiente.
- **`Database:ApplyMigrationsOnStartup`:** um valor inválido derruba o startup. Aceito e documentado.
- **Precisão das versões do npm** corrigida (havia mudanças minor em dependências indiretas).
- **CI:** as actions `@v4` usam Node 20, já obsoleto. Atualizar fica para depois.
- **`Domain/Playtest`** (3 arquivos) ficou órfã depois da limpeza. **Não foi removida** porque está fora da autorização; registrada como **R-020**.

## O que travou

Nada. Nenhum contrato foi pulado.

## Incidentes

O checkout principal continua em `main`, limpo. O trabalho foi feito no worktree `C:\pvq`, como no M2, pelo risco de troca de checkout por fora da sessão. O worktree pode ser removido com `git worktree remove C:\pvq` depois do merge.

## Decisões pendentes do PO

1. **R-020:** remover `src/ProjectVictory.Domain/Playtest/` (3 arquivos sem referência)? Precisa de autorização explícita.
2. **R-019:** liberar o install script do `esbuild` (`npm approve-scripts`) ou manter bloqueado (o build funciona sem ele)?
3. **R-018:** autorizar um contrato em `src/` para que um token sem a claim da conta receba 401 em vez de 500.
4. **R-011:** confirmar a leitura "dono pelo token; id no corpo ou na rota só escolhe o recurso".
5. **Pendências que continuam do M2:**
   - preços provisórios (Rat Tail 2, Training Sword 10);
   - itens em aberto do ADR-026 (quantas Ascensions; automática ou pedida; XP depois do Level 500);
   - dúvidas do Offline Training;
   - janela de reconexão.
6. **Configuração do GitHub** (fora do código): proteção da `main` exigindo o CI verde. Opcional.

Depois do merge, o próximo marco é o **M3 — Combate real**.
