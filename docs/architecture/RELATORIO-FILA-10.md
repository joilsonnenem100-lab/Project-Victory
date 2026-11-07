# Relatório — Fila 10 (MODO FILA, econômico, 2026-11-07)

Worktree `C:/pv10`; branches empilhadas a partir da `main` (`05128f0`); a última, **`feature/f10-gov-021`**, traz tudo. Detalhe por item: `FILA-10-STATUS.md` e os contratos em `docs/contracts/active/`.

| # | Contrato | Resultado | Revisão |
|---|---|---|---|
| 1 | PV-GOV-020 | PO-56 a PO-61 registrados (`PO-DECISOES-BASE.md` v1.2, `DEC-20261107-05`); **Monster AI LOCKED** (`DEC-20261107-06`; R-050/R-043 suspensos) | Aprovado c/ ressalvas (aplicadas) |
| 2 | PV-HUNT-014 | Tier/Continente só informativos (R-064); `GET api/hunt/catalog` | Aprovado c/ ressalvas (aplicadas) |
| 3 | PV-COMBAT-015 | Cura igual para todas as vocações, sem +5 do Druid (PO-59) | Aprovado c/ ressalvas (aplicadas) |
| 4 | PV-DAILY-010 | Recompensa diária por conta no `enter`, 1×/ciclo (R-068) | Aprovado c/ ressalvas (→ R-070/R-071) |
| 5 | PV-CHAR-013 | 5 slots (3 + 2 bloqueados), um por classe, nome Tibia, exclusão em 7 dias, gênero/aparência (R-066) | Aprovado c/ ressalvas (R1/R3 aplicadas; → R-072/R-073) |
| 6 | PV-LEGACY-002 | 15 arquivos apagados (commit isolado `c18cc85`, manifesto) | Aprovado |
| 7 | PV-FIX-001 | `enter` sem personagem → 404 (R-069) | Aprovado c/ ressalvas (aplicadas no item 8) |
| 8 | PV-GOV-021 | INDEX, project-state, roadmap-to-playable, painel R-xxx | — |

**Build:** 0 avisos. **Testes:** **528** verdes, 0 ignorados, 0 falhas (Application 34, Domain 156, Runtime 17, Smoke 187, Api 134), executados no item 7 (o item 8 só mexe em documentos). **Migrations:** 2 novas e só aditivas, `DailyRewardClaims` (CreateTable) e `CharacterAccountRules` (AddColumn); total 25. Nenhuma migration existente foi editada e o banco do PO não foi tocado.

**Leituras registradas (sem perguntar):** o `catch` de área inexistente no retorno automático ficou, e só o bloqueio por tier saiu. O `CreateCombatContext*` também ficou, porque o `CoreGameplayFlowSmokeTest` o usa (R-074). Os nomes de personagem nos testes passaram a usar `TestNames.Unique`, porque os nomes com dígitos do Guid são recusados pelo PO-38.

## PROVISÓRIOS e onde trocar
- **Recompensa diária:** reset às 09:00 UTC, 100 de ouro, 5 poções de vida e 5 de mana. Trocar em `appsettings.json`, seção `DailyReward`.
- **Nome:** só letras e espaço, de 2 a 29 caracteres, até 3 palavras. Trocar em `CharacterNameRules` (Domain). Confirmado em fonte pública: maiúscula só no início de cada palavra e nenhuma palavra de uma letra.
- **Aparência:** outfits Traveler, Scout, Scholar e Guard, padrão Male/Traveler. Trocar em `CharacterAppearanceOptions`.
- **Exclusão:** o cancelamento durante a espera é provisório (`CharacterDeletionHandler.CancelAsync`).
- **Catálogo:** a região da Spider Cave se chama "Spider Cave" (`SpiderCaveCatalog.RegionName`).
- **Cura:** 10/15/20/25 % e 20 de Mana. Trocar em `ClassHealingSpellTable`.

## Perguntas ao PO
1. **R-073:** pode pedir a exclusão de um personagem durante uma Hunt? E quando a exclusão é finalizada, a Hunt ou o runtime ativos devem ser encerrados?
2. **R-072:** com até 3 personagens, o `enter` entra no primeiro da conta. Liberar a escolha do personagem (contrato aditivo)?
3. Os valores PROVISÓRIOS acima, em especial o horário do reset diário (09:00 UTC ≈ server save do Tibia) e a lista da recompensa.
