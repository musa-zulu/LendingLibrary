using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.DB.Repository;
using LendingLibrary.Tests.Common.Builders.Domain;
using LendingLibrary.Tests.Common.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace LendingLibrary.DB.Tests.Repository
{
    [TestFixture]
    public class TestLendingRepository
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(()=>new LendingRepository(Substitute.For<ILendingLibraryDbContext>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenILendingLibraryDbContextIsNull_ShouldThrowExcption()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(()=> new LendingRepository(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("lendingLibraryDbContext", ex.ParamName);
        }

        [Test]
        public void GetAll_GivenNoItemsBorrowed_ShouldShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.GetAll();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void GetAll_GivenOneItemHasBeenBorrowed_ShouldReturnThatBorrowedItem()
        {
            //---------------Set up test pack-------------------
            var lending = new LendingBuilder().WithRandomProps().Build();
            var dbSet = CreateFakeDbSet(lending);
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var controller = CreateLendingController(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.GetAll();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Save_GivenInvalidLendingObject_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(()=> controller.Save(null));
            //---------------Test Result -----------------------
           Assert.AreEqual("lending", ex.ParamName);
        }

        [Test]
        public void Save_GivenValidLendingObject_ShouldSaveToRepo()
        {
            //---------------Set up test pack-------------------
            var lending = LendingBuilder.BuildRandom();
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var controller = CreateLendingController(lendingLibraryDbContext);
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            controller.Save(lending);
            //---------------Test Result -----------------------
            var lendingsFromRepo = controller.GetAll();
            Assert.AreEqual(1, lendingsFromRepo.Count);
            Assert.AreEqual(lending.LedingId, lendingsFromRepo.First().LedingId);
            Assert.AreEqual(lending.ItemId, lendingsFromRepo.First().ItemId);
            Assert.AreEqual(lending.PersonId, lendingsFromRepo.First().PersonId);
            CollectionAssert.Contains(lendingsFromRepo, lending);
        }

        [Test]
        public void Save_GivenValidLendingObject_ShouldShouldAssignLendingId()
        {
            //---------------Set up test pack-------------------
            var lending = new LendingBuilder().WithId(Guid.Empty).Build();
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var controller = CreateLendingController(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            controller.Save(lending);
            //---------------Test Result -----------------------
            var lendingsFromRepo = controller.GetAll();
            Assert.AreNotEqual(Guid.Empty, lendingsFromRepo.First().LedingId);
        }

        [Test]
        public void Save_GivenValidLendingObject_ShouldCallSaveChanges()
        {
            //---------------Set up test pack-------------------
            var lending = LendingBuilder.BuildRandom();
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var controller = CreateLendingController(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            controller.Save(lending);
            //---------------Test Result -----------------------
            lendingLibraryDbContext.Received().SaveChanges();
        }

        private static FakeDbSet<Lending> CreateFakeDbSet(Lending lending = null)
        {
            return new FakeDbSet<Lending> {lending};
        }

        private static LendingRepository CreateLendingController(ILendingLibraryDbContext lendingLibraryDbContext = null)
        {
            if (lendingLibraryDbContext == null)
            {
                lendingLibraryDbContext = Substitute.For<ILendingLibraryDbContext>();
            }
            return new LendingRepository(lendingLibraryDbContext);
        }

        private static IDbSet<Lending> CreateDbSetWithAddRemoveSupport(List<Lending> lendings)
        {
            var dbSet = Substitute.For<IDbSet<Lending>>();

            dbSet.Add(Arg.Any<Lending>()).Returns(info =>
            {
                lendings.Add(info.ArgAt<Lending>(0));
                return info.ArgAt<Lending>(0);
            });

            dbSet.Remove(Arg.Any<Lending>()).Returns(info =>
            {
                lendings.Remove(info.ArgAt<Lending>(0));
                return info.ArgAt<Lending>(0);
            });

            dbSet.GetEnumerator().Returns(info => lendings.GetEnumerator());
            return dbSet;
        }

        private static ILendingLibraryDbContext CreateLendingLibraryDbContext(IDbSet<Lending> dbSet = null)
        {
            if (dbSet == null) dbSet = CreateDbSetWithAddRemoveSupport(new List<Lending>());
            var lendingLibraryDbContext = Substitute.For<ILendingLibraryDbContext>();
            lendingLibraryDbContext.Lendings.Returns(info => dbSet);
            return lendingLibraryDbContext;
        }
    }
}