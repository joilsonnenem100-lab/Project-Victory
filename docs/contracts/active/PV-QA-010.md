# PV-QA-010 — Roteiro de playtest e matriz de aceite (pela API)

Origem: Fila Noturna 1 (2026-11-07), executada sem o PO. Lastro: `roadmap-to-playable.md` (M8, Primeiro Teste Jogável), contratos desta fila (`PV-HUD-ANALYZER-010`, `PV-ADMIN-010`, `PV-COMBAT-014`, `PV-CONTENT-012`, `PV-LEGACY-001`), `developer-guide/guides/configuracao-local.md` (`Jwt:Key` por user-secrets, `PV-PLATFORM-002`). **Só documentos**; nada em `src/` nem em testes.

## Escopo
- **D1 —** `docs/playtest/ROTEIRO-PLAYTEST.md`: pré-requisitos (user-secrets, `dotnet run`, banco local) e o passo a passo de ponta a ponta pelo backend, com o status HTTP e os campos esperados em cada passo; configuração do administrador do XP rate (`Admin:Accounts` = Id da conta, por user-secrets).
- **D2 —** `docs/playtest/MATRIZ-ACEITE.md`: critério × como verificar × teste automatizado que já cobre (classe) × resultado esperado.
- **D3 —** Swagger/OpenAPI **não existe** no `Program.cs` (nenhum `AddSwaggerGen`/`AddOpenApi`/`MapOpenApi`). Por isso: `docs/playtest/api.http` (extensão REST Client do VS Code), variáveis `@baseUrl` e `@token`, requisições na ordem do roteiro. Sem pacote novo; sem segredo (senha de exemplo genérica, marcada como exemplo).

## Fonte das rotas e corpos
Conferidos nos controllers (`src/ProjectVictory.API/Controllers/`) e DTOs da Application (`CreateAccountCommand`, `LoginCommand`/`LoginResponse`, `CreateCharacterCommand`, `EnterGameResult`, `HeartbeatResult`, `ExecuteHuntCommand`/`ExecuteHuntResult`, `AttackEncounterCommand`/`AttackEncounterResult`, `SetSupplyPolicyCommand`, `LeaveHuntCommand`/`EndHuntResult`, `HuntAnalyzerLiveResult`, `HuntAnalyzerSummaryDto`, `LogoutResult`, `ExperienceRateResult`) e no `ApiExceptionHandler` (mapeamento 400/401/403/409/500). Enums trafegam como texto (`JsonStringEnumConverter`).

## Leituras registradas (sem o PO)
1. "Cura por spell" não tem endpoint: é automática (seletor do servidor, `Combat:HealingSpellBelowPercent`, padrão 50 %); o roteiro a verifica pelos turnos `kind = "UseHealingSpell"` na resposta do ataque.
2. "Atacar e acompanhar o loop" = repetir `POST api/hunt/attack` com o `encounterId` corrente; ao vencer, seguir com `nextEncounterId`.
3. Comportamentos atuais conhecidos e registrados (não corrigidos aqui): entrar sem personagem → 500 (`InvalidOperationException`); atacar encounter descartado → 500 (R-061).

## Fora do escopo
Teste automatizado novo; cliente; Swagger (exigiria pacote/alteração de `src/`).

## Validação
`dotnet build ProjectVictory.sln` (0 avisos) e `dotnet test ProjectVictory.sln` executados no fim do item (resultado no `project-state.md`).
