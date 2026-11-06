# Relatório do M2 — Conformidade com a arquitetura LOCKED (2026-11-06)

Ordem do PO (manhã de 2026-11-06): registrar as decisões em `PV-GOV-006` e executar o M2 na ordem do `roadmap-to-playable.md`, com as regras do modo noite. Ou seja: branches empilhadas, plano, build, testes, `pv-architecture-reviewer` e push de cada branch, sem apagar arquivos nem editar migrations existentes.

**Resultado: os 5 contratos foram concluídos e enviados ao `origin`.** Nenhum foi pulado e nenhum travou por decisão de PO. Basta **um merge**: `chief/m2-state-001` na `main`, porque contém todos os anteriores. A `main` (`origin/main` = `8b7caab`) não foi alterada.

## Contratos

| # | Contrato | Branch | Revisão | Testes ao fim |
|---|---|---|---|---|
| 1 | `PV-GOV-006` Decisões do PO + preparação do M2 (só `.md`) | `chief/m2-gov-006` | APROVADO COM RESSALVAS (aplicadas) | — |
| 2 | `PV-PROG-011` Experience Table gerada pela curva | `chief/m2-prog-011` | APROVADO COM RESSALVAS (aplicadas) | 122 aprovados, 1 ignorado |
| 3 | `PV-PROG-010` Level derivado, Experience `long` | `chief/m2-prog-010` | APROVADO COM RESSALVAS (aplicadas) | 134 aprovados, 1 ignorado |
| 4 | `PV-REWARD-010` Reward Pipeline real | `chief/m2-reward-010` | APROVADO COM RESSALVAS (aplicadas) | 156 aprovados, 0 ignorados |
| 5 | `PV-STATE-001` Posse de estado | `chief/m2-state-001` | APROVADO COM RESSALVAS (aplicadas) | **161 aprovados, 0 ignorados, 0 falhas** |

Build da solução no último branch: **0 avisos, 0 erros**. Em cada contrato, o revisor também rodou o build e os testes por conta própria. As ressalvas de um contrato foram aplicadas no branch seguinte, que já estava em andamento; o resultado completo está no último.

Ordem: o `PV-PROG-011` veio antes do `PV-PROG-010`, porque o Level derivado depende da tabela (decisão do Arquiteto D1 no `PV-GOV-006`, ADR-016).

## O que mudou para o jogo

- **Curva de XP** decidida pelo PO: `XP(L) = 50/3 × (L³ − 6L² + 17L − 12)`, igual em todas as Ascensions, com Levels 1 a 500 (teto por Ascension, ADR-026).
- **Level calculado**, não guardado: sobe sozinho com a Experience, que agora é `long`.
- **Derrotar um monstro dá XP de verdade.** Antes o ataque real nunca entregava XP. Rat: 10 XP × XP rate, ouro de 1 a 5 e Rat Tail. Comprovado pela API real (`RewardFlowHttpTests`).
- **XP rate = 1x**, só configuração do servidor (`Progression:ExperienceRate`), sem endpoint. Valor ≤ 0 ou não numérico impede a API de subir.
- **Ouro** guardado no Inventory do personagem (persistido); drops respeitam a chance; **preço de venda ao NPC** nos itens. Preços PROVISÓRIOS: Rat Tail 2, Training Sword 10.
- **Segurança e estado:** stores em memória seguros para requisições simultâneas; `RuntimeId` de outra conta → 403.

## Banco de dados

- 2 migrations **novas**, nenhuma existente editada:
  - `CharacterExperienceLong`: sem mudança de schema, porque o `INTEGER` do SQLite já é 64 bits; só muda o snapshot;
  - `InventoryGoldAndItemSellPrice`: colunas novas com padrão 0, mais o preço do Training Sword em bancos já semeados.
- **A coluna `Level` não foi apagada** (`PV-GOV-006` D2): fica como cache gravado, nunca lido. Apagá-la exige decisão própria.
- **Validado numa cópia** do `projectvictory.db` local do PO, com um personagem antigo inserido na cópia:
  - as duas migrations foram aplicadas;
  - Experience e Level preservados, Gold = 0;
  - preço do Training Sword = 10.

  O MD5 do arquivo original não mudou (`107170ad…`).

## Pendências registradas (nenhuma bloqueia o M3)

| Item | Onde |
|---|---|
| Vida/Mana máximas não crescem com o Level (o crescimento ficava em um método nunca chamado); virar atributo derivado | `project-state.md`; ADR-029 (contrato futuro, candidato M3) |
| Cave Rat sem definição de XP/loot; Rat Tail fora da tabela `Items`; chance de drop sem validação 0–1 | `PV-CONTENT-011` (M3) |
| ADR-036: não existe Inventory Service | R-015 |
| Dois caminhos de recompensa paralelos (ADR-024) | R-016 |
| Recompensa gravada em dois passos (não atômica) | R-017 → `PV-PERSIST-011` (M5) |
| Verificação de `RuntimeId` não atômica e só em memória | `project-state.md` → `PV-RUNTIME-011` |
| Limpeza de memória de Hunts encerradas; concorrência dentro do mesmo encounter | checklist do `PV-HUNT-011` (M6) |
| Ouro ainda não aparece em nenhuma resposta HTTP | M8 (UI/Analyzer) |
| Itens do M1 ainda abertos: `PV-PLATFORM-004`, `PV-PLATFORM-005`, `PV-CLEAN-001` (este apaga arquivos: só com autorização) | `roadmap-to-playable.md` |

## Perguntas para o PO

1. **Preços provisórios** (Rat Tail 2, Training Sword 10): ajustar agora ou no balanceamento do M3?
2. **Itens em aberto do ADR-026** (não bloqueiam nada até a Ascension ser implementada):
   - quantas Ascensions existem;
   - se a Ascension é automática ou pedida pelo jogador;
   - o que acontece com a XP ganha depois do Level 500.
3. **Pendências anteriores que continuam abertas:** confirmação do R-011, as dúvidas do Offline Training e a janela de reconexão.

## Incidente durante a execução

O checkout principal (`C:\Users\Joilson\Documents\GitHub\Project-Victory`) foi trocado para `main` **duas vezes, por algo fora desta sessão** (provavelmente uma IDE aberta). Numa delas, o commit do `PV-GOV-006` caiu na `main` **local**. Ele foi movido para o branch certo e a `main` local voltou para `origin/main`; a `main` remota nunca foi tocada. Daí em diante o trabalho seguiu em worktrees separados (`C:\pvw`, `C:\pvw2`, `C:\pvw3`, `C:\pvw4`), que podem ser removidos com `git worktree remove` depois do merge. O checkout principal ficou em `main`, limpo.
