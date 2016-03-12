using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repository
{
    public class LendingRepository : ILendingRepository
    {
        private ILendingLibraryDbContext _lendingLibraryDbContext;

        public LendingRepository(ILendingLibraryDbContext lendingLibraryDbContext)
        {
            if (lendingLibraryDbContext == null) throw new ArgumentNullException(nameof(lendingLibraryDbContext));
            this._lendingLibraryDbContext = lendingLibraryDbContext;
        }

        public List<Lending> GetAll()
        {
            return _lendingLibraryDbContext.Lendings.ToList();
        }

        public void Save(Lending lending)
        {
            if (lending == null) throw new ArgumentNullException(nameof(lending));
            lending.LedingId = Guid.NewGuid();
            _lendingLibraryDbContext.Lendings.Add(lending);
            _lendingLibraryDbContext.SaveChanges();
        }

        public Item GetById(Guid? id)
        {
            throw new NotImplementedException();
        }

        public void DeleteLending(Lending lending)
        {
            throw new NotImplementedException();
        }

        public void Update(Lending existingLending, Lending newLending)
        {
            throw new NotImplementedException();
        }
    }
}