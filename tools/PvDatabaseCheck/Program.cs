using Microsoft.EntityFrameworkCore;
using ProjectVictory.Persistence.Database;

var options =
    new DbContextOptionsBuilder<ProjectVictoryDbContext>()
        .UseSqlite(
            "Data Source=../../src/ProjectVictory.API/projectvictory.db")
        .Options;


using var context =
    new ProjectVictoryDbContext(options);


var characters =
    await context.Characters.ToListAsync();


foreach (var character in characters)
{
    Console.WriteLine(
        $"Name: {character.Name}");

    Console.WriteLine(
        $"Level: {character.Level}");

    Console.WriteLine(
        $"Experience: {character.Experience}");

    Console.WriteLine(
        "--------------------");
}