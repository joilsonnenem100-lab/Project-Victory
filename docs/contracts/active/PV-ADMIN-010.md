# PV-ADMIN-010 — Ferramenta de administração: papel de administrador e ajuste do XP rate

Origem: Fila Noturna 1 (2026-11-07), executada sem o PO (leituras PROVISÓRIAS marcadas abaixo). Lastro: `DEC-20261106-12` (XP rate = configuração do servidor, 1x; aplicado no Reward Pipeline; no M8 uma ferramenta de administração restrita a uma **conta administradora**, com papel de administrador na Account e autorização por papel no servidor definidos neste contrato), `roadmap-to-playable.md` (linha `PV-ADMIN-010`), ADR-024 (o Reward Pipeline é o único que muda Experience), Server Authority e Clean Architecture (`CLAUDE.md`), PV-SEC-001/002 (identidade só do token; 401/403), PV-REWARD-010 (`Progression:ExperienceRate`). **Só o servidor**; a tela no cliente fica pendente.

## Escopo
- **D1 — Papel (Domain + Application):** `ProjectVictory.Domain.Account.AccountRole { Player, Administrator }`. O papel **não é gravado na conta**: vem da configuração do servidor `Admin:Accounts` (lista; `AdminSettings` em `Application/Admin`). Cada entrada é ~~o **Id da conta (Guid)** ou o **e-mail de login** da conta (comparação exata com o valor gravado)~~ **somente o Id da conta (Guid)** (ressalva R1, abaixo). **Padrão: lista vazia = ninguém é administrador.** `IAccountRoleResolver` / `ConfiguredAccountRoleResolver` resolve o papel a partir do Id da conta do token (e-mails resolvidos por `IAccountRepository.GetByEmailAsync`, sem método novo no repositório).
- **D2 — Autorização por papel (API):** policy ASP.NET `Administrator` (`AdministratorRequirement` + handler que lê a claim da conta, `AccountClaim`, e pergunta ao `IAccountRoleResolver`). Sem token → **401**; token de jogador comum → **403**. Defesa em profundidade: os casos de uso também conferem o papel pela identidade do token (`ICurrentPlayerContext`) e lançam `AdministratorRequiredException` → **403**.
- **D3 — XP rate efetivo (Application/Reward):** `IExperienceRateProvider` / `ExperienceRateProvider`: valor **persistido** pelo administrador, se existir; senão o da configuração (`RewardSettings`, `Progression:ExperienceRate`). O `RewardResolutionHandler` (e o `ApplyRewardHandler`) recebem o provedor como parâmetro **opcional novo**; sem provedor ou sem valor persistido o comportamento é **idêntico ao de antes** (Regra Aditiva). Vale para as **novas recompensas** (lido a cada recompensa; XP já entregue não muda).
- **D4 — Casos de uso (Application/Admin):** `GetExperienceRateHandler` e `SetExperienceRateHandler`. Validação: valor `> 0` e `<= ExperienceRateLimits.Maximum` → senão `InvalidExperienceRateException` → **400**. Grava quem alterou (Id da conta) e quando.
- **D5 — Persistência (Persistence/Admin):** `IExperienceRateStore` / `ExperienceRateStore`, tabela nova `ServerExperienceRate` (linha única `Id = 1`: `Value`, `ChangedByAccountId`, `ChangedAtUnixMilliseconds`).
- **D6 — Endpoints aditivos** (controller novo `AdminController`, `[Authorize(Policy = "Administrator")]`, fino):
  - `GET api/admin/xp-rate` → `ExperienceRateResult { Value, ConfiguredValue, IsOverridden, ChangedAt?, Maximum }`.
  - `PUT api/admin/xp-rate` com `{ "value": 2.5 }` → o mesmo resultado, já com o valor novo.

## Leituras PROVISÓRIAS (onde trocar)
1. **Administrador por configuração, não por coluna na conta** — `Admin:Accounts` (`AdminSettings.AccountsKey`, lida no `Program.cs`). Em Development: `dotnet user-secrets set "Admin:Accounts:0" "<id-da-conta>"`; fora dele `Admin__Accounts__0`. Nada de Id/e-mail real no repositório (`appsettings.json` traz a lista vazia). Trocar em `ConfiguredAccountRoleResolver`.
2. ~~**Entrada aceita Id (Guid) ou e-mail de login** (e-mail com comparação exata, como o login) — `ConfiguredAccountRoleResolver`.~~ **Superado pela ressalva R1:** só o Id (Guid).
3. **Limite superior = 100x** — `ExperienceRateLimits.Maximum` (`Application/Admin/ExperienceRateAdministration.cs`).
4. **Sem histórico de alterações**: só o valor atual, com autor e instante — `ExperienceRateStore`.
5. **Lido do banco a cada recompensa** (uma linha; sem cache) — `ExperienceRateProvider`.
6. **Precisão do valor:** decimal do servidor, aplicado com `floor` como no PV-REWARD-010 (`ExperienceRate.Apply`).

## Ajuste de teste existente (justificado)
`ExperienceRateConfigurationTests.NoEndpoint_ExposesExperienceRate` afirmava a regra do **M2** ("nenhum endpoint expõe o rate"). O próprio `DEC-20261106-12` prevê o endpoint de administração no M8. O teste passa a afirmar: a **única** rota com "rate"/"admin" é `api/admin/xp-rate`, protegida pela policy `Administrator`; nenhuma rota com "progression". Os demais testes do PV-REWARD-010 ficam intactos.

## Migration
Nova e aditiva: `20261107071252_ServerExperienceRate` (`CreateTable`; `Down` = `DropTable`; o snapshot só ganha linhas). Gerada com `dotnet ef migrations add`, chave JWT aleatória só no ambiente do comando e banco temporário; nenhuma migration existente editada. Necessária: o valor alterado precisa sobreviver ao reinício.

## Fora do escopo
Tela no cliente; papel gravado na conta; outros parâmetros de administração; auditoria completa.

## Testes
Application.Tests (validação do valor, 403 para não administrador, provedor: sem valor persistido = configuração). SmokeTests (gravação + leitura do rate pela composição real; nova recompensa usa o rate novo). Api.Tests `AdminExperienceRateHttpTests` (401 sem token; 403 jogador comum; 200 para administrador configurado, leitura e alteração; valor inválido → 400; derrotar um Rat depois do ajuste entrega XP × rate novo). Resultado: build 0 avisos; **433 testes verdes** (420 + 3 unidade + 6 smoke + 3 E2E + 1 caso novo da convenção de autorização dos controllers).

## Ressalvas da revisão (2026-11-07, Fila Noturna 1)
- **R1 (segurança) — só Id da conta.** O cadastro não verifica e-mail; com e-mail aceito em `Admin:Accounts`, quem se cadastrasse primeiro com o e-mail configurado viraria administrador. Agora `AdminSettings` aceita **somente Guid** (≠ `Guid.Empty`); qualquer outra entrada não vazia é ignorada e contada em `IgnoredEntryCount`, e o `Program.cs` registra um **aviso no log da inicialização** (só a quantidade; o valor não é escrito, pois pode ser um e-mail). `ConfiguredAccountRoleResolver` deixou de depender de `IAccountRepository`. Testes: `AdminRoleAndRateTests` (Guid só; e-mail nunca concede papel) e `AdminExperienceRateHttpTests` (novo `ConfiguredEmail_IsIgnored_StillForbidden`; o teste do administrador cadastra a conta num primeiro servidor e sobe o segundo no **mesmo banco** com `Admin__Accounts__0` = Id — `ApiFactory(sharingDatabaseWith:)`). **Ajuste justificado:** os testes antigos que configuravam e-mail deixaram de valer por decisão de segurança desta ressalva.
- **R2 — variáveis de ambiente nos testes HTTP.** Verificado: todas as classes do `ProjectVictory.Api.Tests` que sobem a API ou mudam `Admin__Accounts__0`/`Combat__FixedSeed` (e as demais variáveis) estão na collection `"Api"` (`ApiCollection.Name`), que não roda em paralelo consigo mesma. As 4 classes fora dela (`ControllerAuthorizationConventionTests`, `HttpCurrentPlayerContextTests`, `HuntConflictMappingTests`, `JwtSettingsTests`) não leem variável de ambiente nem sobem a API. Nenhuma correção necessária.
- **R3 — leitura do rate no banco a cada monstro** (laço do `RewardResolutionHandler`): registrada como dívida no `project-state.md` (Revisões Pendentes).
