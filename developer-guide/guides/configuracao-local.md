# Configuração local e segredos da API

Contrato: `PV-PLATFORM-002`. Vale para `src/ProjectVictory.API`.

> O repositório é **público**. Nenhum segredo (chave JWT, senha, connection string com credencial) pode ser escrito em arquivo versionado — nem em `appsettings*.json`, código, documentação ou mensagem de commit.

## O que a API lê da configuração

| Chave | Onde fica | Obrigatória | Observação |
|---|---|---|---|
| `Jwt:Key` | **user-secrets** (Development) ou variável `Jwt__Key` | Sim, em **todos** os ambientes | Mínimo 32 caracteres. Sem valor padrão: faltando ou curta, a API **não sobe** |
| `Jwt:Issuer` | `appsettings.json` (`ProjectVictory.API`) | Sim | Validado no token |
| `Jwt:Audience` | `appsettings.json` (`ProjectVictory.Client`) | Sim | Validado no token |
| `ConnectionStrings:ProjectVictory` | `appsettings.json` (`Data Source=projectvictory.db`) | Sim | Caminho relativo ao diretório de trabalho (ver `CLEANUP-MANIFEST-2026-11-05.md`) |
| `Cors:AllowedOrigins` | `appsettings.Development.json` (`http://localhost:5173`) | Não | Vazio fora de Development: nenhuma origem liberada |

Ordem de precedência (padrão do ASP.NET Core): `appsettings.json` → `appsettings.{Ambiente}.json` → user-secrets (só em Development) → variáveis de ambiente → linha de comando.

## Primeira vez em Development (obrigatório)

A partir da raiz do repositório, gere uma chave aleatória e grave nos user-secrets:

```powershell
# PowerShell
$key = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(48))
dotnet user-secrets set "Jwt:Key" $key --project src/ProjectVictory.API
```

```bash
# Git Bash / Linux / macOS
dotnet user-secrets set "Jwt:Key" "$(openssl rand -base64 48)" --project src/ProjectVictory.API
```

Conferir (o valor aparece só no seu terminal; não copie para nenhum arquivo do repositório):

```
dotnet user-secrets list --project src/ProjectVictory.API
```

Os user-secrets ficam **fora do repositório**, em `%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json` (Windows) ou `~/.microsoft/usersecrets/<UserSecretsId>/secrets.json`. O `UserSecretsId` está no `ProjectVictory.API.csproj`.

Depois disso, rode normalmente:

```
dotnet run --project src/ProjectVictory.API
```

Sem a chave, o startup falha com a mensagem `Configuração JWT inválida — a API não pode subir...` e o comando acima. As ferramentas `dotnet ef` também executam o startup da API em Development e precisam do mesmo user-secret.

Trocar a chave invalida todos os tokens emitidos; basta fazer login de novo.

## Fora de Development (Production, Staging)

User-secrets **não** são lidos. Use variáveis de ambiente (`:` vira `__`):

```
Jwt__Key=<chave aleatória, 32+ caracteres, vinda do cofre de segredos>
ConnectionStrings__ProjectVictory=Data Source=<caminho absoluto do banco>
Cors__AllowedOrigins__0=https://<origem do client>
```

Comportamento verificado em 2026-11-05: sem `Jwt__Key` ou com chave de menos de 32 caracteres, a API encerra no startup com mensagem clara. `EnableSensitiveDataLogging` e `LogTo(Console)` do EF ficam **desligados** fora de Development.

**Limitação conhecida:** fora de Development o schema **não** é criado nem atualizado automaticamente (o `MigrateAsync()` roda só em Development — `PV-PLATFORM-001`). A estratégia de migrations em produção é um contrato próprio do roadmap.
