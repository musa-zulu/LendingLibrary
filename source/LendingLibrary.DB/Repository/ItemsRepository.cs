﻿using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repository
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly ILendingLibraryDbContext _lendingLibraryDbContext;

        public ItemsRepository(ILendingLibraryDbContext lendingLibraryDbContext)
        {
            if (lendingLibraryDbContext == null) throw new ArgumentNullException(nameof(lendingLibraryDbContext));
            _lendingLibraryDbContext = lendingLibraryDbContext;
        }

        public List<Item> GetAllItems()
        {
            return _lendingLibraryDbContext.Items.ToList();
        }

        public void Save(Item item)
        {
            throw new NotImplementedException();
        }
    }
}