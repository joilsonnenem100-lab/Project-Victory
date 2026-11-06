using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVictory.Domain.Items;

namespace ProjectVictory.Domain.Entities;

public sealed class Inventory
{
    private readonly List<ItemInstance> _items;


    public IReadOnlyCollection<ItemInstance> Items =>
        _items.AsReadOnly();


    public int Count =>
        _items.Count;


    // PV-REWARD-010: ouro é um Resource do Inventory (ADR-035), guardado como saldo.
    // Só o Reward Pipeline o acrescenta nesta fase (comércio é Fase 7/8).
    public long Gold { get; private set; }


    public Inventory()
    {
        _items = [];
    }


    public void AddItem(
        ItemInstance item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
    }


    public void AddGold(
        long amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Gold amount cannot be negative.");
        }

        Gold = checked(Gold + amount);
    }


    public bool RemoveItem(
        Guid itemInstanceId)
    {
        var item =
            _items.FirstOrDefault(
                x => x.Id == itemInstanceId);


        if (item is null)
        {
            return false;
        }


        return _items.Remove(item);
    }


    public bool Contains(
        Guid itemInstanceId)
    {
        return _items.Any(
            x => x.Id == itemInstanceId);
    }


    public ItemInstance? GetItem(
        Guid itemInstanceId)
    {
        return _items.FirstOrDefault(
            x => x.Id == itemInstanceId);
    }


    public void Clear()
    {
        _items.Clear();
    }
}
