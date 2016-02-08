﻿using System.Collections.Generic;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface IItemsRepository
    {
        List<Item> GetAllItems();
        void Save(Item item);
    }
}