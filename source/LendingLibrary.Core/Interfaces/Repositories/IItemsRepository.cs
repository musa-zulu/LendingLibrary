using System.Collections.Generic;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface IItemsRepository
    {
        IEnumerable<Item> GetAllItems();
        void Save(Item item);
    }
}