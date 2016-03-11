using System;
using System.Collections.Generic;
using LendingLibrary.Core.Domain;


namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface ILendingRepository
    {
        List<Lending> GetAllILendings();
        void Save(Lending lending);
        Item GetById(Guid? id);
        void DeleteLending(Lending lending);
        void Update(Lending existingLending, Lending newLending);
    }
}