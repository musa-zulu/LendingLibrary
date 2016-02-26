using System;
using System.Collections.Generic;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetAllPeople();
        void Save(Person person);
        Person GetById(Guid? id);
        void DeletePerson(Person person);
    }
}