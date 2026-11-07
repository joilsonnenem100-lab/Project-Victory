# Roteiro de playtest — de ponta a ponta pelo backend

Item `PV-QA-010` (Fila Noturna 1, 2026-11-07). Rotas e corpos conferidos nos controllers e DTOs (contrato `docs/contracts/active/PV-QA-010.md`). Arquivo pronto para o VS Code (REST Client): `docs/playtest/api.http`. Critérios de aceite: `docs/playtest/MATRIZ-ACEITE.md`.

> Não há Swagger/OpenAPI no projeto. Use o `api.http` ou qualquer cliente HTTP (curl, Postman).
> Enums trafegam como **texto** (`"Knight"`, `"Victory"`, `"Manual"`…). Respostas de erro vêm como `ProblemDetails` (`status`, `title`, `detail`).

## 1. Pré-requisitos

1. **.NET 10 SDK** e o repositório clonado.
2. **Chave JWT por user-secrets** (sem ela a API não sobe — `PV-PLATFORM-002`). Siga `developer-guide/guides/configuracao-local.md`: gere uma chave aleatória de 32+ caracteres e grave com
   `dotnet user-secrets set "Jwt:Key" <sua-chave-gerada> --project src/ProjectVictory.API`.
   **Nunca** escreva a chave em arquivo do repositório (o repositório é público).
3. **Subir a API** (raiz do repositório): `dotnet run --project src/ProjectVictory.API` → `http://localhost:5250` (perfil `http` do `launchSettings.json`, ambiente Development).
4. **Banco local de desenvolvimento:** SQLite `projectvictory.db` (`ConnectionStrings:ProjectVictory`, caminho relativo ao diretório de trabalho do processo). Em Development as migrations são aplicadas no startup e os itens são semeados (`ItemDefinitionSeed`). Para começar do zero, pare a API e apague esse arquivo local (é seu, não está no repositório).
5. **Opcional — combate reprodutível:** `Combat:FixedSeed` (só vale em Development) fixa a semente do combate.
6. **Kit inicial:** todo personagem novo nasce com o kit do `appsettings.json` (`Supplies:StarterKit`: Health 30, Strong Health 10, Great Health 3, Mana 20, Strong Mana 5, Great Mana 2 — `PV-CONTENT-012`).

**Heartbeat obrigatório durante a Hunt:** sem heartbeat por mais de 60 s (`GameSession:DisconnectTimeoutSeconds`) o servidor trata como queda e encerra a Hunt (`Disconnect`). Mande `POST api/game/heartbeat` a cada 20–30 s durante o teste.

## 2. Sequência

Todas as rotas abaixo, exceto registro e login, exigem `Authorization: Bearer <token>`. Sem token → **401**. Recurso de outra conta (ou inexistente, nos casos com posse) → **403**.

| # | Passo | Requisição | Esperado |
|---|---|---|---|
| 1 | Registro | `POST api/account` `{ "email": "...", "password": "..." }` | **200**; corpo = Id da conta (Guid, texto JSON). Use um e-mail novo a cada playtest. |
| 2 | Login | `POST api/account/login` (mesmo corpo) | **200** `{ accountId, token }`. Senha errada → **401** (`detail: "Invalid credentials."`). Guarde `token` e `accountId`. |
| 3 | Criar personagem | `POST api/character` `{ "name": "...", "characterClass": "Knight" }` (Knight, Paladin, Sorcerer, Druid ou Monk) | **200**; corpo = Id do personagem (Guid). Conferir com `GET api/character` → lista `{ accountId, characterId, name, level: 1, experience: 0, title: "Novice", titleTier: 0, availableAbilities }`. Opcional: `GET api/inventory/{characterId}` mostra o kit (`items`). |
| 4 | Entrar no mundo | `POST api/game/enter` (sem corpo) | **200** `{ accountId, characterId, runtimeId, characterName, level, experience, title, titleTier }`. Guarde `runtimeId`. Sem personagem → **500** (comportamento atual registrado). Use **um personagem por conta** (o enter pega o personagem da conta). |
| 5 | Heartbeat | `POST api/game/heartbeat` (sem corpo) | **200** `{ characterId, status: "Online", huntId: null, returnedToLastHunt: false, lastHuntArea, encounter: null }`. |
| 6 | Iniciar Hunt (Spider Cave) | `POST api/hunt/execute` `{ "characterId", "runtimeId", "huntArea": "Spider Cave" }` | **200** `{ huntId, completed: false, experience, gold, level, encounter: { id, huntId, currentTargetId, monsterName, currentHealth, maximumHealth } }`. Guarde `huntId` e `encounter.id`. Área desconhecida → **400**. Segunda Hunt com outra em andamento → **409** (a primeira continua). |
| 7 | Atacar e acompanhar o loop | `POST api/hunt/attack` `{ "encounterId" }` — repetir | **200** `{ encounterId, monsterName, currentHealth, maximumHealth, monsterDefeated, playerHealth, playerMaximumHealth, playerDefeated, combatFinished, combatState, experience, gold, level, drops, turns[], huntEndReason, returnedToCity, nextEncounterId, nextEncounter, huntElapsedSeconds, huntExperienceGained }`. Cada `turns[]` traz `actorName`, `targetName`, `kind` (`BasicAttack`, `UseSkill`, `UseHealthPotion`, `UseManaPotion`, `UseHealingSpell`…), `damage`, `healing`, `actorHealthAfter`, `targetHealthAfter`, `skillName`. Ao vencer: `combatState: "Victory"`, `huntEndReason: null`, a Hunt **continua** e `nextEncounterId` é o próximo alvo — passe a atacar por ele. Na morte: `playerDefeated: true`, `huntEndReason: "Death"`, `returnedToCity: true`, `returnCity: "Victoria"`, `deathNotice.checks = ["Potions","Runes","HuntLevel"]`. XP aparece em `GET api/character`. |
| 8 | Configurar poções | `POST api/hunt/supplies` `{ "characterId", "healthPotionBelowPercent": 60, "manaPotionBelowPercent": 40 }` (1–100 ou `null` = não usar) | **200** `{ characterId, healthPotionBelowPercent, manaPotionBelowPercent, appliesFrom: "NextTurn" }`. Fora de 1–100 → **400**. Vale a partir do próximo turno; o servidor escolhe o nível da poção (menor que resolve, senão a maior). Verifique turnos `UseHealthPotion`/`UseManaPotion` com `healing > 0` nos ataques seguintes. |
| 9 | Cura por spell | (sem endpoint — automática) | Com vida abaixo de 50 % da máxima (`Combat:HealingSpellBelowPercent`) e Mana ≥ 20, aparece um turno `kind: "UseHealingSpell"` do próprio jogador (`actorName` = `targetName`), `healing > 0` (hoje `skillName` vem `null` nesse turno; o nome da spell — ex.: Knight → Wound Mending — não é exposto na resposta). Tem prioridade sobre a poção de vida; sem Mana, a poção é a reserva (`PV-COMBAT-014`). |
| 10 | Analyzer em tempo real | `GET api/hunt/analyzer/{characterId}` | **200** `{ hasActiveHunt: true, huntId, analyzer: { experienceGained, elapsedSeconds, experiencePerHour, debit, lootValue, gold, profit, netProfit, suppliesUsed, itemsLooted }, lastHunt }`. Personagem de outra conta ou inexistente → **403**. |
| 11 | Sair da Hunt | `POST api/hunt/leave` `{ "huntId" }` | **200** `{ huntId, reason: "Manual", huntState: "Completed", returnedToCity: true, currentHealth, currentMana, returnCity: "Victoria", elapsedSeconds, experienceGained, experiencePerHour }`. Hunt já encerrada → **409**; de outra conta/inexistente → **403**. XP e loot ficam (nada é perdido). |
| 12 | Histórico do Analyzer | `GET api/hunt/analyzer/{characterId}/history` (e de novo o passo 10) | **200** lista `[{ huntId, endReason: "Manual", endedAt, analyzer: {...} }]`, mais recente primeiro (até 10). O passo 10 agora responde `hasActiveHunt: false` e `lastHunt` com o resumo. |
| 13 | Logout | `POST api/game/logout` (sem corpo) | **200** `{ characterId, huntEnded, returnCity: "Victoria", ending }` (`huntEnded: true` e `ending.reason: "Logout"` se havia Hunt). Depois, `POST api/game/heartbeat` → `status: "LoggedOut"` até um novo `enter`; não há retorno automático à Hunt. |

**Queda (opcional):** com uma Hunt em andamento, pare o heartbeat por mais de 60 s → a Hunt termina por `Disconnect`. Volte a mandar heartbeat: após 10 s online (`GameSession:ReturnDelaySeconds`) o heartbeat responde `returnedToLastHunt: true` com uma **nova** Hunt na última área (ADR-057, `DEC-20261106-20/21`).

## 3. Administrador do XP rate (`PV-ADMIN-010`)

1. Faça registro e login (passos 1–2) com a conta que será administradora e anote o `accountId` da resposta do login.
2. Pare a API e grave o Id (**somente o Guid**; e-mail é ignorado — ressalva R1):
   `dotnet user-secrets set "Admin:Accounts:0" "<accountId>" --project src/ProjectVictory.API`
   (fora de Development: variável `Admin__Accounts__0`). Não grave Id real em `appsettings.json`.
3. Suba a API de novo e faça login com essa conta.
4. `GET api/admin/xp-rate` → **200** `{ value, configuredValue, isOverridden, changedAt, maximum }` (padrão `value: 1.0`, `isOverridden: false`, `maximum: 100`).
5. `PUT api/admin/xp-rate` `{ "value": 2.5 }` → **200** com `value: 2.5`, `isOverridden: true`, `changedAt` preenchido. Valor ≤ 0 ou > 100 → **400**. Vale para as **novas** recompensas (XP já entregue não muda) e sobrevive ao reinício.
6. Jogador comum → **403**; sem token → **401**.

## 4. Comportamentos atuais conhecidos (não são falha do playtest)
- `POST api/game/enter` sem personagem → 500.
- `POST api/hunt/attack` em encounter descartado/inexistente → 500 (R-061).
- `POST api/gameplay/action` → 404 (retirado no `PV-LEGACY-001`).
- Recompensa diária de poções (PO-16) não implementada; sem tela no cliente para Analyzer, poções e admin.
