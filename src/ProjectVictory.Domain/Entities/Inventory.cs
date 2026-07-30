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
