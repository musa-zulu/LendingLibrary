using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.FakeRepository
{
    public class PersonRepository : IPersonRepository
    {
        private ILendingLibraryDbContext _lendingLibraryDbContext;

        public PersonRepository(ILendingLibraryDbContext lendingLibraryDbContext)
        {
            if (lendingLibraryDbContext == null) throw new ArgumentNullException(nameof(lendingLibraryDbContext));
            _lendingLibraryDbContext = lendingLibraryDbContext;
        }

        public List<Person> GetAllPeople()
        {
            return _lendingLibraryDbContext.People.ToList();
        }

        public void Save(Person person)
        {
           if(person == null) throw new ArgumentNullException(nameof(person));
            _lendingLibraryDbContext.People.Add(person);
        }

        public Person GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void DeletePerson(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
