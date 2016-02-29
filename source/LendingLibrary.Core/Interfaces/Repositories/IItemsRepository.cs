using System;
using System.Collections.Generic;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface IItemsRepository
    {
        List<Item> GetAllItems();
        void Save(Item item);
        Item GetById(Guid id);
        void DeleteItem(Item item);
        void Update(Item existingItem, Item newItem);
    }
}