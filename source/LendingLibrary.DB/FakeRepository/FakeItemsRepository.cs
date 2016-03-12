using System.Collections.Generic;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.DB.FakeRepository
{
    public interface IItemsRepositoryFake
    {
        void Save(Item item);
        List<Item> GetAll();
    }

    public class FakeItemsRepository : IItemsRepositoryFake
    {
        private List<Item> list = new List<Item>();

        public void Save(Item item)
        {
            list.Add(item);
        }

        public List<Item> GetAll()
        {
            return list;
        }
    }
}