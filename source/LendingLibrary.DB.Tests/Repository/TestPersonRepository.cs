using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Schema;
using LendingLibrary.Core.Domain;
using LendingLibrary.DB.Repository;
using LendingLibrary.Tests.Common.Builders.Domain;
using LendingLibrary.Tests.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.DB.Tests.Repository
{
    [TestFixture]
    public class TestPersonRepository
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new PersonRepository(Substitute.For<ILendingLibraryDbContext>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenILendingLibraryIsNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new PersonRepository(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("lendingLibraryDbContext", ex.ParamName);
        }

        [Test]
        public void GetAllPeople_GivenNoPeopleSaved_ShouldReturnAnEmptyList()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personRepository.GetAllPeople();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetAllPeople_GivenPeopleExistInRepo_ShouldReturnAListOfPeopleFromDataBase()
        {
            //---------------Set up test pack-------------------
            var people = new List<Person>();
            var person = PersonBuilder.BuildRandom();
            people.Add(person);

            var dbSet = CreateDbSetWithAddRemoveSupport(people);
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personRepository.GetAllPeople();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void Save_GivenNullPersonObject_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => personRepository.Save(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("person", ex.ParamName);
        }

        [Test]
        public void Save_GivenValidPersonObject_ShouldSavePersonToRepo()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildDefault();
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            personRepository.Save(person);
            //---------------Test Result -----------------------
            var peopleFromRepo = personRepository.GetAllPeople();
            Assert.AreEqual(1, peopleFromRepo.Count);
            Assert.AreEqual(person.PersonId, peopleFromRepo.First().PersonId);
            CollectionAssert.Contains(peopleFromRepo, person);
        }

        [Test]
        public void Save_GivenValidPersonObject_ShouldCallSaveChanges()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            personRepository.Save(person);
            //---------------Test Result -----------------------
            lendingLibraryDbContext.Received().SaveChanges();
        }

        [Test]
        public void GetById_GivenInvalidId_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => personRepository.GetById(Guid.Empty));
            //---------------Test Result -----------------------
            Assert.AreEqual("id", ex.ParamName);
        }

        [Test]
        public void GetById_GivenValidId_ShouldReturnPersonWithMatchingId()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var dbSet = new FakeDbSet<Person> { person };

            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personRepository.GetById(person.PersonId);
            //---------------Test Result -----------------------
            Assert.AreEqual(person, result);
        }

        [Test]
        public void DeletePerson_GivenInvalidExistingPerson_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => personRepository.DeletePerson(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("person", ex.ParamName);
        }

        [Test]
        public void DeletePerson_GivenInvalidNewPerson_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => personRepository.DeletePerson(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("person", ex.ParamName);
        }

        [Test]
        public void DeletePerson_GivenValidPersonObject_ShouldDeleteThePerson()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var dbSet = new FakeDbSet<Person> { person };
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            personRepository.DeletePerson(person);
            //---------------Test Result -----------------------
            var peopleFromRepo = personRepository.GetAllPeople();
            CollectionAssert.DoesNotContain(peopleFromRepo, person);
        }

        [Test]
        public void DeletePerson_GivenAPersonHasBeenDeleted_ShouldCallSaveChanges()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var dbSet = new FakeDbSet<Person> { person };
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            personRepository.DeletePerson(person);
            //---------------Test Result -----------------------
            lendingLibraryDbContext.Received().SaveChanges();
        }

        [Test]
        public void Update_GivenPersonObjectIsNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => personRepository.Update(null, new Person()));
            //---------------Test Result -----------------------
            Assert.AreEqual("existingPerson", ex.ParamName);
        }

        [Test]
        public void Update_GivenValidPersonObject_ShouldReplaceExistingPersonObject()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => personRepository.Update(new Person(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("newPerson", ex.ParamName);
        }

        [Test]
        public void Update_GivenValidExistingAndNewPersonObject_ShouldReplaceExistingPersonWithNewPersonObject()
        {
            //---------------Set up test pack-------------------
            var existingPerson = new PersonBuilder()
                .WithFirstName(RandomValueGen.GetRandomString())
                .WithLastName(RandomValueGen.GetRandomString())
                .WithPhoneNumber(RandomValueGen.GetRandomInt())
                .WithRandomProps()
                .Build();
            var newPerson = new PersonBuilder().WithRandomProps().Build();

            var dbSet = new FakeDbSet<Person> { existingPerson };
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------
            Assert.AreNotEqual(existingPerson.PersonId, newPerson.PersonId);
            Assert.AreNotEqual(existingPerson.Email, newPerson.Email);
            Assert.AreNotEqual(existingPerson.FirstName, newPerson.FirstName);
            Assert.AreNotEqual(existingPerson.LastName, newPerson.LastName);
            Assert.AreNotEqual(existingPerson.CreatedUsername, newPerson.CreatedUsername);
            Assert.AreNotEqual(existingPerson.DateCreated, newPerson.DateCreated);
            Assert.AreNotEqual(existingPerson.DateLastModified, newPerson.DateLastModified);
            Assert.AreNotEqual(existingPerson.LastModifiedUsername, newPerson.LastModifiedUsername);
            //---------------Execute Test ----------------------
            personRepository.Update(existingPerson, newPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(existingPerson.FirstName, newPerson.FirstName);
            Assert.AreEqual(existingPerson.LastName, newPerson.LastName);
            Assert.AreEqual(existingPerson.PhoneNumber, newPerson.PhoneNumber);
            Assert.AreEqual(existingPerson.CreatedUsername, newPerson.CreatedUsername);
            Assert.AreEqual(existingPerson.DateCreated, newPerson.DateCreated);
            Assert.AreEqual(existingPerson.DateLastModified, newPerson.DateLastModified);
            Assert.AreEqual(existingPerson.LastModifiedUsername, newPerson.LastModifiedUsername);
        }

        [Test]
        public void Update_GivenExistingPersonObjectHasBeenUpdated_ShouldCallSaveChanges()
        {
            //---------------Set up test pack-------------------
            var existingPerson = new PersonBuilder().WithRandomProps().Build();
            var newPerson = new PersonBuilder().WithRandomProps().Build();
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var personRepository = CreatePersonRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            personRepository.Update(existingPerson, newPerson);
            //---------------Test Result -----------------------
            lendingLibraryDbContext.Received().SaveChanges();
        }

        private static PersonRepository CreatePersonRepository(ILendingLibraryDbContext lendingLibraryDbContext)
        {
            return new PersonRepository(lendingLibraryDbContext);
        }

        private static IDbSet<Person> CreateDbSetWithAddRemoveSupport(List<Person> people)
        {
            var dbSet = Substitute.For<IDbSet<Person>>();

            dbSet.Add(Arg.Any<Person>()).Returns(info =>
            {
                people.Add(info.ArgAt<Person>(0));
                return info.ArgAt<Person>(0);
            });

            dbSet.Remove(Arg.Any<Person>()).Returns(info =>
            {
                people.Remove(info.ArgAt<Person>(0));
                return info.ArgAt<Person>(0);
            });

            dbSet.GetEnumerator().Returns(info => people.GetEnumerator());
            return dbSet;
        }

        private static ILendingLibraryDbContext CreateLendingLibraryDbContext(IDbSet<Person> dbSet = null)
        {
            if (dbSet == null) dbSet = CreateDbSetWithAddRemoveSupport(new List<Person>());
            var lendingLibraryDbContext = Substitute.For<ILendingLibraryDbContext>();
            lendingLibraryDbContext.People.Returns(info => dbSet);
            return lendingLibraryDbContext;
        }
    }
}