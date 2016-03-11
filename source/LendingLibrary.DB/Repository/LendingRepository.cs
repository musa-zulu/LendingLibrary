using System;
using System.Collections.Generic;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repository
{
    public class LendingRepository : ILendingRepository
    {
        private ILendingLibraryDbContext lendingLibraryDbContext;

        public LendingRepository(ILendingLibraryDbContext lendingLibraryDbContext)
        {
            this.lendingLibraryDbContext = lendingLibraryDbContext;
        }

        public List<Lending> GetAllILendings()
        {
            throw new NotImplementedException();
        }

        public void Save(Lending lending)
        {
            throw new NotImplementedException();
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