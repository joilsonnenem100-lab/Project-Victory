using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Entities;

public sealed class Equipment
{
    private readonly HashSet<string> _equippedItems;

    public IReadOnlyCollection<string> EquippedItems =>
        _equippedItems;

    public int Count =>
        _equippedItems.Count;


    public Equipment()
    {
        _equippedItems = new HashSet<string>();
    }


    public void Equip(string item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (string.IsNullOrWhiteSpace(item))
        {
            return;
        }

        _equippedItems.Add(item);
    }


    public void Remove(string item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (string.IsNullOrWhiteSpace(item))
        {
            return;
        }

        _equippedItems.Remove(item);
    }


    public bool Contains(string item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (string.IsNullOrWhiteSpace(item))
        {
            return false;
        }

        return _equippedItems.Contains(item);
    }


    public IReadOnlyCollection<string> GetItems()
    {
        return _equippedItems;
    }
}