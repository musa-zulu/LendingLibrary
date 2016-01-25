using System;
using System.Collections.Generic;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Repository
{
    public interface IItemsRepository
    {
        void Save(ItemViewModel item);
        List<ItemViewModel> GetAll();

    }

    public class ItemsRepository : IItemsRepository
    {
        private List<ItemViewModel> list = new List<ItemViewModel>();

        public void Save(ItemViewModel item)
        {
            list.Add(item);
        }

        public List<ItemViewModel> GetAll()
        {
            return list;
        }
    }
}