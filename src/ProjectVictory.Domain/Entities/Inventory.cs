using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Entities
{
    public sealed class Inventory
    {
        private readonly HashSet<string> _items;

        public IReadOnlyCollection<string> Items => _items;

        public Inventory()
        {
            _items = new HashSet<string>();
        }

        public void AddItem(string item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (string.IsNullOrWhiteSpace(item))
            {
                return;
            }

            _items.Add(item);
        }

        public void RemoveItem(string item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (string.IsNullOrWhiteSpace(item))
            {
                return;
            }

            _items.Remove(item);
        }

        public bool Contains(string item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (string.IsNullOrWhiteSpace(item))
            {
                return false;
            }

            return _items.Contains(item);
        }
    }
}