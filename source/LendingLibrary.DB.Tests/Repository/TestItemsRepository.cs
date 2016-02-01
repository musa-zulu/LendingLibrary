using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.DB.Repository;
using LendingLibrary.Tests.Common.Builders.Domain;
using NSubstitute;
using NUnit.Framework;

namespace LendingLibrary.DB.Tests.Repository
{
    [TestFixture]
    public class TestItemsRepository
    {
        [Test]
        public void Contruct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new ItemsRepository(Substitute.For<ILendingLibraryDbContext>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenLendingLibraryDbContextIsNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new ItemsRepository(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("lendingLibraryDbContext", ex.ParamName);
        }

        [Test]
        public void GetAllItems_GivenNoItemsExist_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = Substitute.For<ILendingLibraryDbContext>();
            var itemsRepository = CreateLendingLibraryDbContext(lendingLibraryDbContext);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = itemsRepository.GetAllItems();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetAllItems_GivenItemsExistInRepo_ShouldReturnListOfItems()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = Substitute.For<ILendingLibraryDbContext>();
            var dbSet = Substitute.For<IDbSet<Item>>();
            var items = new List<Item>();
            var item = ItemBuilder.BuildRandom();
            items.Add(item);
            dbSet.GetEnumerator().Returns(info => items.GetEnumerator());
            lendingLibraryDbContext.Items.Returns(info => dbSet);
            
            var itemsRepository = CreateLendingLibraryDbContext(lendingLibraryDbContext);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = itemsRepository.GetAllItems();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, result.Count());
        }

        private static ItemsRepository CreateLendingLibraryDbContext(ILendingLibraryDbContext lendingLibraryDbContext)
        {
            return new ItemsRepository(lendingLibraryDbContext);
        }
    }
}