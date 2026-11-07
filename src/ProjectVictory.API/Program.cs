using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectVictory.API.Authentication;
using ProjectVictory.API.Database;
using ProjectVictory.API.Middleware;
using ProjectVictory.API.Session;
using ProjectVictory.Composition;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Database.Seed;
using System.Text;
using System.Text.Json.Serialization;

var builder =
    WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// PV-PLATFORM-002: origens CORS vêm de configuração (Cors:AllowedOrigins).
// Lista vazia = nenhuma origem liberada (a API sobe, o navegador é bloqueado).
var allowedOrigins =
    builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>()
    ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "ProjectVictoryClient",
        policy =>
        {
            policy
                .WithOrigins(
                    allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<
    ICurrentPlayerContext,
    HttpCurrentPlayerContext>();

builder.Services.AddScoped<
    JwtTokenGenerator>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddProblemDetails();

// PV-PLATFORM-002: chave, Issuer e Audience vêm de configuração, sem valor padrão.
// Falta ou chave fraca derruba o startup com mensagem clara (JwtSettings.Validate).
var jwtSettings =
    builder.Configuration
        .GetSection(JwtSettings.SectionName)
        .Get<JwtSettings>()
    ?? new JwtSettings();

jwtSettings.Validate(
    builder.Environment.IsDevelopment());

// A mesma instância validada serve à emissão (JwtTokenGenerator) e à validação (JwtBearer).
builder.Services.AddSingleton(
    Microsoft.Extensions.Options.Options.Create(jwtSettings));

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)),

                ValidateIssuer = true,

                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,

                ValidAudience = jwtSettings.Audience
            };

        // PV-SEC-002 (R-018): token válido sem a claim da conta (ou com valor que não é Guid)
        // não autentica → 401 em todo endpoint [Authorize], em vez de 500 adiante.
        options.Events =
            new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    if (!AccountClaim.TryGetAccountId(context.Principal, out _))
                    {
                        context.Fail("Token sem a claim da conta.");
                    }

                    return Task.CompletedTask;
                }
            };
    });

// PV-ADMIN-010: autorização por papel no servidor — policy "Administrator" (sem token → 401;
// jogador comum → 403). O papel vem da configuração, resolvido pela Application.
builder.Services.AddScoped<
    Microsoft.AspNetCore.Authorization.IAuthorizationHandler,
    AdministratorAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        AdministratorAuthorizationHandler.PolicyName,
        policy => policy
            .RequireAuthenticatedUser()
            .AddRequirements(new AdministratorRequirement()));
});

// PV-PLATFORM-002: connection string vem de configuração (ConnectionStrings:ProjectVictory).
var connectionString =
    builder.Configuration.GetConnectionString("ProjectVictory");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "ConnectionStrings:ProjectVictory não configurada — a API não pode subir. "
        + "Defina em appsettings.json ou pela variável de ambiente ConnectionStrings__ProjectVictory. "
        + "Ver developer-guide/guides/configuracao-local.md.");
}

builder.Services.AddDbContext<ProjectVictoryDbContext>(
    options =>
    {
        options.UseSqlite(
            connectionString);

        // PV-PLATFORM-002: log de SQL com dados sensíveis só em Development.
        if (builder.Environment.IsDevelopment())
        {
            options
                .EnableSensitiveDataLogging()
                .LogTo(
                    Console.WriteLine,
                    LogLevel.Information);
        }
    });

// PV-ARCH-001 (R-014): todos os serviços do jogo vêm do método único de composição,
// o mesmo usado pelos testes (src/ProjectVictory.Composition). Aqui ficam só a
// configuração do servidor e o que é próprio da API (HTTP, JWT, CORS, DbContext).
//
// PV-REWARD-010: XP rate só do servidor (DEC-20261106-12), padrão 1x; valor inválido
// (<= 0 ou não numérico) derruba o startup. Nenhum endpoint lê ou altera este valor.
// PV-ADMIN-010: este continua sendo o valor PADRÃO; o administrador pode gravar outro
// (api/admin/xp-rate), que passa a valer para as novas recompensas.
var experienceRateValue =
    builder.Configuration.GetValue<decimal?>(RewardSettings.ExperienceRateKey) ?? 1m;

// PV-COMBAT-011: "Combat:FixedSeed" fixa a semente dos combates — só vale em Development
// (testes/depuração). Fora dele é IGNORADA (revisão do PV-COMBAT-011: semente fixa em produção
// tornaria todo combate previsível).
var fixedCombatSeed =
    builder.Configuration.GetValue<int?>(ProjectVictory.Application.Combat.TurnBased.CombatSettings.FixedSeedKey);

if (fixedCombatSeed is not null && !builder.Environment.IsDevelopment())
{
    Console.Error.WriteLine(
        $"[aviso] {ProjectVictory.Application.Combat.TurnBased.CombatSettings.FixedSeedKey} ignorado fora de Development.");
    fixedCombatSeed = null;
}

builder.Services.AddProjectVictoryCore(
    new RewardSettings(
        new ProjectVictory.Domain.Reward.ExperienceRate(experienceRateValue)),
    new ProjectVictory.Application.Combat.TurnBased.CombatSettings(fixedCombatSeed),
    // PV-SUPPLY-010: poções iniciais (PROVISÓRIO; padrão 0 = nenhuma).
    // PV-CONTENT-012 (PO-16): + kit por nível em "Supplies:StarterKit:<Key>" (appsettings.json; PROVISÓRIO).
    new ProjectVictory.Application.Supply.SupplySettings(
        builder.Configuration.GetValue<int?>(ProjectVictory.Application.Supply.SupplySettings.StarterHealthPotionsKey) ?? 0,
        builder.Configuration.GetValue<int?>(ProjectVictory.Application.Supply.SupplySettings.StarterManaPotionsKey) ?? 0,
        ProjectVictory.Application.Supply.SupplySettings.ParseStarterKit(
            builder.Configuration.GetSection(ProjectVictory.Application.Supply.SupplySettings.StarterKitKey)
                .GetChildren()
                .Select(entry => new KeyValuePair<string, int>(
                    entry.Key,
                    int.TryParse(entry.Value, out var quantity)
                        ? quantity
                        : throw new InvalidOperationException($"{ProjectVictory.Application.Supply.SupplySettings.StarterKitKey}:{entry.Key} deve ser um número inteiro."))))));

// PV-RUNTIME-010 (ADR-057 v1.2; DEC-20261106-20): tempos da sessão de jogo em configuração
// (queda > 1 min encerra a Hunt; retorno depois de > 10 s online) e varredura periódica de quedas.
Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Replace(
    builder.Services,
    ServiceDescriptor.Singleton(new ProjectVictory.Application.GameSession.GameSessionSettings(
        TimeSpan.FromSeconds(builder.Configuration.GetValue<double?>(ProjectVictory.Application.GameSession.GameSessionSettings.DisconnectTimeoutKey) ?? 60),
        TimeSpan.FromSeconds(builder.Configuration.GetValue<double?>(ProjectVictory.Application.GameSession.GameSessionSettings.ReturnDelayKey) ?? 10),
        TimeSpan.FromSeconds(builder.Configuration.GetValue<double?>(ProjectVictory.Application.GameSession.GameSessionSettings.SweepIntervalKey) ?? 5))));
builder.Services.AddHostedService<ProjectVictory.API.Configuration.GameSessionSweepService>();

// PV-HUD-ANALYZER-010: tamanho do histórico do Hunt Analyzer (PROVISÓRIO, padrão 10; < 1 derruba o startup).
Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Replace(
    builder.Services,
    ServiceDescriptor.Singleton(new ProjectVictory.Application.HuntAnalyzer.HuntAnalyzerSettings(
        builder.Configuration.GetValue<int?>(ProjectVictory.Application.HuntAnalyzer.HuntAnalyzerSettings.HistorySizeKey)
        ?? ProjectVictory.Application.HuntAnalyzer.HuntAnalyzerSettings.DefaultHistorySize)));

// PV-COMBAT-014 (PO-53): spell de cura quando a vida fica abaixo de X% da máxima (PROVISÓRIO, padrão 50;
// 0 desliga a cura por spell; fora de 0..100 derruba o startup).
var healingSpellBelowPercent = builder.Configuration.GetValue<int?>(ProjectVictory.Domain.Combat.Rules.HealingSpellPolicy.BelowPercentKey)
    ?? ProjectVictory.Domain.Combat.Rules.HealingSpellPolicy.DefaultBelowPercent;
if (healingSpellBelowPercent is < 0 or > 100)
{
    throw new InvalidOperationException($"{ProjectVictory.Domain.Combat.Rules.HealingSpellPolicy.BelowPercentKey} deve ficar entre 0 e 100.");
}
Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Replace(
    builder.Services,
    ServiceDescriptor.Singleton(new ProjectVictory.Domain.Combat.Rules.HealingSpellPolicy(healingSpellBelowPercent)));

// PV-ADMIN-010 (DEC-20261106-12): contas administradoras vêm só da configuração do servidor
// ("Admin:Accounts": SOMENTE o Id da conta (Guid) — ressalva R1; user-secrets/variável de ambiente).
// Padrão: ninguém. Entradas que não são Guid são ignoradas e avisadas no log depois do Build().
var adminSettings = new ProjectVictory.Application.Admin.AdminSettings(
    builder.Configuration.GetSection(ProjectVictory.Application.Admin.AdminSettings.AccountsKey).Get<string[]>() ?? []);
Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Replace(
    builder.Services,
    ServiceDescriptor.Singleton(adminSettings));

// PV-PROG-013 (DEC-20261106-30): títulos por Level lidos da configuração ("LevelTitles") a cada
// consulta — trocar rótulos/faixas no appsettings.json não exige recompilar nem reiniciar.
// Revisão: registrado pela DI (recebe IConfiguration e ILogger); substitui o padrão da composição.
// Revisão final da Fila 5: Replace deixa explícita a substituição (um único registro no container).
Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Replace(
    builder.Services,
    ServiceDescriptor.Singleton<
        ProjectVictory.Application.Progression.ILevelTitleProvider,
        ProjectVictory.API.Configuration.ConfigurationLevelTitleProvider>());

var app =
    builder.Build();

// PV-ADMIN-010 (ressalva R1): aviso sem escrever o valor da entrada (pode ser um e-mail).
if (adminSettings.IgnoredEntryCount > 0)
{
    app.Logger.LogWarning(
        "{Key}: {Count} entrada(s) ignorada(s) — só o Id da conta (Guid) é aceito; e-mail não vale mais.",
        ProjectVictory.Application.Admin.AdminSettings.AccountsKey,
        adminSettings.IgnoredEntryCount);
}

using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<ProjectVictoryDbContext>();

    // PV-PLATFORM-001: em Development, aplica as migrations existentes antes do seed,
    // para que um clone limpo suba com o schema criado.
    // PV-PLATFORM-004: fora de Development, só aplica se a chave explícita
    // "Database:ApplyMigrationsOnStartup" estiver ligada (padrão: desligada, appsettings.json).
    var applyMigrationsOutsideDevelopment =
        app.Configuration.GetValue<bool>(DatabaseStartupSettings.ApplyMigrationsOnStartupKey);

    if (DatabaseStartupSettings.ShouldApplyMigrations(
            app.Environment.IsDevelopment(),
            applyMigrationsOutsideDevelopment))
    {
        await context.Database.MigrateAsync();
    }
    else
    {
        var pending =
            (await context.Database.GetPendingMigrationsAsync()).ToList();

        if (pending.Count > 0)
        {
            app.Logger.LogWarning(
                "Há {Count} migration(s) pendente(s) e a aplicação automática está desligada fora de Development. Aplique-as no deploy (dotnet ef database update / bundle) ou ligue {Key}=true. Pendentes: {Pending}",
                pending.Count,
                DatabaseStartupSettings.ApplyMigrationsOnStartupKey,
                string.Join(", ", pending));
        }
    }

    await ItemDefinitionSeed.ExecuteAsync(
        context);

    // PV-RUNTIME-011 (Handbook §18; ADR-057): recarrega personagem + Runtime dos checkpoints e
    // reconstrói o contexto em memória (tela inicial, Idle). Hunt Session NÃO é recuperada. Uma falha
    // DO RECOVERY é registrada e não derruba o servidor — o "entrar no jogo" continua reconstruindo o
    // Runtime sob demanda. (Schema desatualizado já falharia antes, no seed de itens acima.)
    try
    {
        var recovery =
            await scope.ServiceProvider
                .GetRequiredService<ProjectVictory.Application.Runtime.RuntimeRecoveryService>()
                .RecoverAsync();

        app.Logger.LogInformation(
            "Recovery do Runtime: {Recovered} recuperado(s), {Skipped} sem personagem.",
            recovery.Recovered,
            recovery.SkippedWithoutCharacter);
    }
    catch (Exception exception)
    {
        app.Logger.LogError(exception, "Falha no Recovery do Runtime ao iniciar o servidor.");
    }
}

app.UseExceptionHandler();

app.UseCors(
    "ProjectVictoryClient");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();