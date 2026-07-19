using System;

namespace ProjectVictory.Domain.Entities;

public sealed class Monster
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public int Health { get; private set; }

    public string State { get; private set; }


    private Monster()
    {
        Name = string.Empty;
        State = string.Empty;
    }


    public Monster(
        Guid id,
        string name,
        int health,
        string state)
    {
        Id = id;

        Name = name;

        Health = health;

        State = state;
    }
}