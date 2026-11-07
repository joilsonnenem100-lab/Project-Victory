# Anexo A — Fila Noturna 1: commits, revisões e ressalvas

## Leituras registradas (sem PO acordado)
- Os itens 1–6 são idênticos à Fila 9, executada antes na mesma sessão, e não foram refeitos. A Parte B nasceu da última branch pronta.
- Cada item de código atravessa várias camadas. Por isso foi executado por um agente general-purpose que respeita as camadas, com o contrato escrito antes do código. O revisor leu o diff por commit.

## Commits por item
| Item | Branch | Commits |
|---|---|---|
| 7 | `feature/n1-hud-analyzer-010` | 5caae61 (STATUS), 9688df5, ac9ca17 |
| 8 | `feature/n1-admin-010` | 648eb6e, 7454172 |
| 9 | `feature/n1-combat-014` | 0278023 (ressalvas do 7), 684c8e0, a94d06a, f44203a |
| 10 | `feature/n1-content-012` | b71acd3 (ressalvas do 8), baa31f3, c530229, a888c8d, d864ac5 |
| 11 | `feature/n1-legacy-001` | e5dc67c (ressalvas do 9), 694217c, be9e094 (remoção isolada), 20b8cfa, 4d65228 |
| 12 | `feature/n1-qa-010` | c18eb5e (ressalvas do 10), d02af23, f958632, fechamento |

## Ressalvas das revisões
- **Item 7:** observador do Reward com falha isolada (R1); entidade desanexada quando a gravação falha (R2); teste de 403 para personagem inexistente (R3); ramo de morte do E2E (R4). Tudo aplicado.
- **Item 8:** admin só por Id, porque e-mail sem verificação permitia tomar o papel (R1, segurança); collection dos testes conferida (R2); leitura do rate a cada monstro registrada como dívida R-067 (R3). Tudo aplicado.
- **Item 9:** smoke pela composição real provando a cura ligada (A); startup cai com limite inválido (B); vida cheia vira `NotExecuted` sem gastar Mana (C); bônus do Druid marcado como "inferência" (D). Tudo aplicado.
- **Item 10:** trecho de e-mail tachado no contrato do admin (R1); limitação do `ISupplyEffects` documentada (R2). Tudo aplicado. O INDEX foi atualizado no fechamento (R3), e a recompensa diária está nas perguntas ao PO (R4).
- **Item 11:** texto do `PV-LEGACY-002` corrigido (só o `ExplorationDecisionHandler` e o smoke pedem autorização nova; ressalva 1). As limpezas da linha de DI comentada e dos comentários ficam no `PV-LEGACY-002` (ressalva 3).
- **Item 12:** ver STATUS.

## Revisões pendentes novas (`project-state.md`)
R-067: XP rate lido a cada monstro. R-068: recompensa diária não implementada. R-053: retirada parcial, com o resto no `PV-LEGACY-002`. R-065: resolvido no servidor.

## Limites conhecidos
- O `api.http` não foi executado contra a API rodando.
- `enter` sem personagem e `attack` em encounter descartado devolvem 500 (R-061).
- A meta de "~30 min" de conteúdo foi estimada, não medida.
