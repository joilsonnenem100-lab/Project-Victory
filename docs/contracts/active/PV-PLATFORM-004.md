# PV-PLATFORM-004 — Schema do banco fora de Development

Status: **IMPLEMENTADO** (branch `chief/q2-platform-004`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 2", item 5). Pedido: "Aplicar migrations no startup fora de Development apenas se uma chave de configuração explícita estiver ligada (padrão: desligada em Production)".
Lastro: pendência herdada do `PV-PLATFORM-001`/`002`; Análise §4.4.

## Escopo implementado

1. `src/ProjectVictory.API/Database/DatabaseStartupSettings`:
   - define a chave `Database:ApplyMigrationsOnStartup`;
   - define a regra `ShouldApplyMigrations(isDevelopment, flag)`: aplica em Development **ou** com a chave ligada.
2. `Program.cs`:
   - Development continua aplicando sempre, sem mudança;
   - fora de Development, aplica só com a chave ligada;
   - com a chave desligada e migrations pendentes, registra um **aviso** no log com a lista delas. Não derruba o startup, como antes.
3. `appsettings.json`: `"Database": { "ApplyMigrationsOnStartup": false }` (padrão explícito).
4. Documentação:
   - linha nova em `developer-guide/guides/configuracao-local.md`, com a limitação antiga tachada;
   - guia novo `developer-guide/guides/deploy.md` (não existia documento de deploy), com as duas estratégias (migrations no pipeline ou no startup) e o comando `dotnet ef database update` ou bundle.

## Decisão do Arquiteto

- **D1 — Nenhuma falha nova no startup com a chave desligada.**
  - Com banco vazio, o seed já falhava no startup ("no such table") e continua falhando.
  - Com banco existente e migrations pendentes, a API só registra o aviso.
  - Derrubar o startup seria uma mudança de comportamento que não foi pedida.

## Testes (`tests/ProjectVictory.Api.Tests/MigrationsOnStartupTests.cs`)

- **Regra:** as 4 combinações de ambiente e chave.
- **Production com a chave ligada:**
  - a API real sobe com o banco vazio;
  - as migrations são aplicadas (`__EFMigrationsHistory` preenchido);
  - sem token, responde 401.
- **Production com a chave desligada (padrão):**
  - nenhuma migration é aplicada;
  - o startup falha no seed com uma `SqliteException` "no such table", como antes.
- O `ApiFactory` ganhou um construtor `internal` que recebe o ambiente. O fixture do xUnit continua com um único construtor público, em Development.

Resultado da solução: build com 0 avisos; **173 aprovados, 0 ignorados, 0 falhas**. Nenhuma migration foi criada ou editada.
