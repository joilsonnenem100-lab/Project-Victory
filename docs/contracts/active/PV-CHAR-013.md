# PV-CHAR-013 — Regras de personagem da conta (Fila 10, item 5)

Status: IMPLEMENTADO (2026-11-07). Lastro: `PO-DECISOES-BASE.md` **PO-36, PO-37, PO-38, PO-39** e PO-57 (`DEC-20261107-05`). Fecha **R-066**. Regras aplicadas: Server Authority (conta do token; posse pelo `PlayerOwnershipGuard`) e Regra Aditiva (campos e endpoints novos são opcionais; nada removido). Escopo: só servidor.

## Regras (Domain: `CharacterAccountRules.cs`; Application: `CharacterAccount.cs`)
1. **Slots (PO-36/PO-57):** 5 por conta, 3 utilizáveis e 2 bloqueados. Não existe mecanismo de destrave. Com 3 personagens, criar devolve **409**. `GET api/character/slots` lista os 5 slots (`Occupied`/`Free`/`Locked`) e as opções de aparência.
2. **Um por classe (PO-36):** uma segunda classe repetida devolve **409**.
3. **Nome (PO-38, formato do Tibia):**
   - Confirmado em fonte pública (TibiaQA, reforma de nomes de 2008): maiúscula **só** no início de cada palavra, e palavra de uma letra é proibida.
   - **PROVISÓRIO** (sem fonte oficial encontrada): só letras A–Z e espaço simples, de 2 a 29 caracteres, no máximo 3 palavras.
   - O nome é único entre os personagens não excluídos, sem diferenciar maiúsculas.
   - Formato inválido devolve **400**; nome em uso devolve **409**.
   - Trocar em `CharacterNameRules`.
4. **Gênero e aparência (PO-37):** `gender` (Male/Female) e `outfit` são opcionais no `POST api/character`, com padrão Male/Traveler. **PROVISÓRIOS:** as opções de outfit Traveler, Scout, Scholar e Guard, trocáveis em `CharacterAppearanceOptions`. Valor desconhecido devolve **400**. A lista de personagens ganha `characterClass`, `gender`, `outfit` e `deletionScheduledFor` (campos aditivos).
5. **Exclusão (PO-39):**
   - `POST api/character/{id}/delete` deixa a exclusão **pendente** e devolve a data (`agora + 7 dias`).
   - O servidor finaliza a exclusão quando a data passa, de forma preguiçosa, ao listar, ver os slots, criar, entrar ou mexer na exclusão. A finalização marca `DeletedAt`: o personagem some da conta e o slot, a classe e o nome ficam livres. O histórico fica no banco.
   - **PROVISÓRIO:** `POST api/character/{id}/cancel-deletion`, que cancela durante a espera.
   - Exclusão já pendente, ou cancelamento sem pendência, devolve **409**. Personagem de outra conta, inexistente ou já excluído devolve **403**.
6. **Concorrência:** a criação é serializada no processo (`CharacterCreationGate`). A premissa é de servidor único (R-062).

## Persistência
Migration **nova e aditiva** `CharacterAccountRules`, só com `AddColumn` em `Characters`:
- `Gender` (int, padrão 0 = não informado nos personagens anteriores);
- `Outfit`;
- `DeletionRequestedAt…`, `DeletionScheduledFor…` e `DeletedAt…` (long, anuláveis).

As consultas da conta (`GetByAccountIdAsync`, `GetAllByAccountIdAsync`) ignoram os personagens excluídos.

## Testes
- **Domain:** `CharacterAccountRulesTests` (20 testes: nomes válidos e inválidos, slots, 7 dias, opções).
- **Smoke:** `CharacterAccountRulesSmokeTests` (8 testes, relógio controlado):
  - 3 slots + 2 bloqueados;
  - um por classe;
  - formato e unicidade do nome;
  - aparência;
  - exclusão pendente → finalizada aos 7 dias, liberando slot, classe e nome;
  - cancelamento;
  - outra conta → 403;
  - enter sem personagem depois da exclusão.
- **API:** `CharacterAccountRulesHttpTests` (401 nos 3 endpoints novos; 400; 409; slots; exclusão com data, cancelamento e 403).
- **Ajuste com justificativa:** os geradores de nome dos testes usavam dígitos do Guid (`"X" + Guid…[..10]`), que o PO-38 recusa. Foram trocados, de forma mecânica, por `TestNames.Unique("X")`: uma palavra só de letras e única, em `SmokeTests` e `Api.Tests`. `"SmokeHero"` e `"LootTestCharacter"` também foram trocados, porque têm maiúscula no meio da palavra. Nenhuma asserção mudou.

## Limites conhecidos
- O `enter` continua escolhendo o primeiro personagem da conta (`GetByAccountIdAsync`). Escolher o personagem no enter fica para um contrato futuro (R-072).
- Não há destrave dos 2 slots bloqueados, que depende da loja de gemas (Fase 8).

## Resultado
Build sem avisos. **526 testes verdes** (Application 34, Runtime 17, Domain 156, Smoke 187, Api 132).
