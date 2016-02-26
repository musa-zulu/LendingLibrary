using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repository
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
            if (person == null) throw new ArgumentNullException(nameof(person));
            _lendingLibraryDbContext.People.Add(person);
            _lendingLibraryDbContext.SaveChanges();
        }

        public Person GetById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            return _lendingLibraryDbContext.People.FirstOrDefault(p => p.Id == id);
        }

        public void DeletePerson(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            _lendingLibraryDbContext.People.Remove(person);
            _lendingLibraryDbContext.SaveChanges();
        }
    }
}
