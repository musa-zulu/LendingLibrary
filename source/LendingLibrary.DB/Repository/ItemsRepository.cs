using System;
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
            if (item == null) throw new ArgumentNullException(nameof(item));
            _lendingLibraryDbContext.Items.Add(item);
            _lendingLibraryDbContext.SaveChanges();
        }

        public Item GetById(Guid? id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            return _lendingLibraryDbContext.Items.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteItem(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _lendingLibraryDbContext.Items.Remove(item);
            _lendingLibraryDbContext.SaveChanges();
        }

        public void Update(Item existingItem, Item newItem)
        {
            if (existingItem == null) throw new ArgumentNullException(nameof(existingItem));
            if (newItem == null) throw new ArgumentNullException(nameof(newItem));

            existingItem.ItemName = newItem.ItemName;
            existingItem.CreatedUsername = newItem.CreatedUsername;
            existingItem.DateCreated = newItem.DateCreated;
            existingItem.DateLastModified = newItem.DateLastModified;
            existingItem.LastModifiedUsername = newItem.LastModifiedUsername;
            existingItem.Photo = newItem.Photo;

            _lendingLibraryDbContext.SaveChanges();
        }
    }
}