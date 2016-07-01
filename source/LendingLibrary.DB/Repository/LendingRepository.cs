using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repository
{
    public class LendingRepository : ILendingRepository
    {
        private readonly ILendingLibraryDbContext _lendingLibraryDbContext;

        public LendingRepository(ILendingLibraryDbContext lendingLibraryDbContext)
        {
            if (lendingLibraryDbContext == null) throw new ArgumentNullException(nameof(lendingLibraryDbContext));
            this._lendingLibraryDbContext = lendingLibraryDbContext;
        }

        public List<Lending> GetAll() => _lendingLibraryDbContext.Lendings.ToList();
        
        public void Save(Lending lending)
        {
            if (lending == null) throw new ArgumentNullException(nameof(lending));
            lending.LedingId = Guid.NewGuid();
            lending.LendingStatus = LendingStatus.NotAvailable;
            _lendingLibraryDbContext.Lendings.Add(lending);
            _lendingLibraryDbContext.SaveChanges();
        }

        public Lending GetById(Guid? lendingId)
        {
            if (lendingId == null) throw new ArgumentNullException(nameof(lendingId));
            return _lendingLibraryDbContext.Lendings.FirstOrDefault(x => x.LedingId == lendingId);
        }

        public void DeleteLending(Lending lending)
        {
            if (lending == null) throw new ArgumentNullException(nameof(lending));
            _lendingLibraryDbContext.Lendings.Remove(lending);
            _lendingLibraryDbContext.SaveChanges();
        }

        public void Update(Lending existingBorrowedItem, Lending newItem)
        {
            if (existingBorrowedItem == null) throw new ArgumentNullException(nameof(existingBorrowedItem));
            if (newItem == null) throw new ArgumentNullException(nameof(newItem));

            existingBorrowedItem.ItemId = newItem.ItemId;
            existingBorrowedItem.PersonId = newItem.PersonId;
            existingBorrowedItem.DateBorrowed = newItem.DateBorrowed;
            existingBorrowedItem.DateReturned = newItem.DateReturned;
            existingBorrowedItem.LendingStatus = newItem.LendingStatus;
         
      
            _lendingLibraryDbContext.SaveChanges();
        }
    }
}