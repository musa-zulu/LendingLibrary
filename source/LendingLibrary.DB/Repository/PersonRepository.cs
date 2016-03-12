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
            person.PersonId = Guid.NewGuid();
            _lendingLibraryDbContext.People.Add(person);
            _lendingLibraryDbContext.SaveChanges();
        }

        public Person GetById(Guid? id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            return (from person in _lendingLibraryDbContext.People where person.PersonId == id select person).FirstOrDefault();
        }

        public void DeletePerson(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            _lendingLibraryDbContext.People.Remove(person);
            _lendingLibraryDbContext.SaveChanges();
        }

        public void Update(Person existingPerson, Person newPerson)
        {
            if (existingPerson == null) throw new ArgumentNullException(nameof(existingPerson));
            if (newPerson == null) throw new ArgumentNullException(nameof(newPerson));

            existingPerson.FirstName = newPerson.FirstName;
            existingPerson.LastName = newPerson.LastName;
            existingPerson.PhoneNumber = newPerson.PhoneNumber;
            existingPerson.CreatedUsername = newPerson.CreatedUsername;
            existingPerson.DateCreated = newPerson.DateCreated;
            existingPerson.DateLastModified = newPerson.DateLastModified;
            existingPerson.LastModifiedUsername = newPerson.LastModifiedUsername;

            _lendingLibraryDbContext.SaveChanges();
        }
    }
}
