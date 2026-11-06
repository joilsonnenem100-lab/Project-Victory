using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVictory.API.Controllers;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-QA-001 parte 2 (ressalva da revisão): todo controller novo nasce protegido.
// Regra do PV-SEC-001: só AccountController é público; os demais exigem token na classe
// (ou em todas as ações) e nenhuma ação usa [AllowAnonymous].
public sealed class ControllerAuthorizationConventionTests
{
    private static readonly Type[] PublicControllers = [typeof(AccountController)];

    public static TheoryData<string> Controllers()
    {
        var data = new TheoryData<string>();

        foreach (var type in ControllerTypes())
        {
            data.Add(type.FullName!);
        }

        return data;
    }

    [Fact]
    public void ApiHasControllers()
    {
        Assert.NotEmpty(ControllerTypes());
    }

    [Theory]
    [MemberData(nameof(Controllers))]
    public void Controller_RequiresAuthorization_UnlessPublic(string controllerName)
    {
        var type = ControllerTypes().Single(t => t.FullName == controllerName);

        var actions =
            type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => m.GetCustomAttributes<Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute>().Any())
                .ToList();

        if (PublicControllers.Contains(type))
        {
            return;
        }

        Assert.DoesNotContain(actions, a => a.GetCustomAttribute<AllowAnonymousAttribute>() is not null);

        var classProtected = type.GetCustomAttribute<AuthorizeAttribute>() is not null;

        Assert.True(
            classProtected || actions.All(a => a.GetCustomAttribute<AuthorizeAttribute>() is not null),
            $"{type.Name} precisa de [Authorize] (PV-SEC-001).");
    }

    private static IReadOnlyList<Type> ControllerTypes() =>
        typeof(AccountController).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(ControllerBase).IsAssignableFrom(t))
            .OrderBy(t => t.FullName)
            .ToList();
}
