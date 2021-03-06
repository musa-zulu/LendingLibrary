﻿using System;
using System.Collections.Generic;
using LendingLibrary.Core.Domain;


namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface ILendingRepository
    {
        List<Lending> GetAll();
        void Save(Lending lending);
        Lending GetById(Guid? lendingId);
        void DeleteLending(Lending lending);
        void Update(Lending existingLending, Lending newLending);
    }
}