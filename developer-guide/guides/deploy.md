# Deploy — banco de dados e configuração (fora de Development)

Status: guia operacional · Criado em 2026-11-06 (`PV-PLATFORM-004`)
Ver também: `developer-guide/guides/configuracao-local.md` (todas as chaves de configuração).

## Variáveis obrigatórias

```
ASPNETCORE_ENVIRONMENT=Production
Jwt__Key=<chave aleatória, 32+ caracteres, vinda do cofre de segredos>
ConnectionStrings__ProjectVictory=Data Source=<caminho absoluto do banco SQLite>
Cors__AllowedOrigins__0=https://<origem do client>
```

Opcional: `Progression__ExperienceRate` (XP rate do servidor, padrão `1.0`; `DEC-20261106-12`).

## Schema do banco (migrations)

O startup aplica as migrations existentes:

| Ambiente | Aplica migrations no startup? |
|---|---|
| Development | **Sempre** (`PV-PLATFORM-001`) |
| Fora de Development (Production, Staging…) | **Só** com `Database__ApplyMigrationsOnStartup=true`. **Padrão: desligado** (`appsettings.json`) |

Escolha **uma** das estratégias por ambiente:

1. **Migrations no pipeline de deploy (recomendado para Production).** Mantenha a chave desligada e aplique as migrations antes de subir a nova versão:
   ```
   dotnet ef database update --project src/ProjectVictory.Persistence --startup-project src/ProjectVictory.API --connection "Data Source=<caminho do banco>"
   ```
   Outra opção é gerar um bundle executável: `dotnet ef migrations bundle --project src/ProjectVictory.Persistence --startup-project src/ProjectVictory.API`. Faça **backup** do banco antes (o arquivo `.db` junto com `-wal` e `-shm`).
2. **Migrations no startup.** Ligue `Database__ApplyMigrationsOnStartup=true`. É simples para um servidor único, mas o schema é atualizado no momento em que a aplicação sobe, sem etapa de aprovação.

Com a chave desligada:
- se houver migrations pendentes, a API registra um **aviso** no log com a lista delas;
- se o banco estiver **vazio**, o seed de itens falha no startup com "no such table". É o sinal de que o schema não foi criado.

## Regras

- Nunca editar migrations existentes, porque elas já foram aplicadas em bancos reais. Toda mudança de schema entra por uma migration **nova**.
- Nunca versionar o arquivo do banco nem segredos (o repositório é público).
