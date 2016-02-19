using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.DB.FakeRepository;
using LendingLibrary.Tests.Common.Builders.Domain;
using NSubstitute;
using NUnit.Framework;

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
            Assert.AreEqual(person.Id, peopleFromRepo.First().Id);
            CollectionAssert.Contains(peopleFromRepo, person);
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