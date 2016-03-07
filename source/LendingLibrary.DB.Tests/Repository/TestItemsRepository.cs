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
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);
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
            var items = new List<Item>();
            var dbSet = CreateDbSetWithAddRemoveSupport(items);
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            var item = ItemBuilder.BuildRandom();
            items.Add(item);
            dbSet.GetEnumerator().Returns(info => items.GetEnumerator());
            lendingLibraryDbContext.Items.Returns(info => dbSet);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = itemsRepository.GetAllItems();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Save_GivenNullItem_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                itemsRepository.Save(null);
            });
            //---------------Test Result -----------------------
            Assert.AreEqual("item", ex.ParamName);
        }

        [Test]
        public void Save_GivenItem_ShouldSaveItem()
        {
            //---------------Set up test pack-------------------
            var item = ItemBuilder.BuildRandom();
            var items = new List<Item>();

            var dbSet = CreateDbSetWithAddRemoveSupport(items);
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            itemsRepository.Save(item);
            //---------------Test Result -----------------------
            var itemsFromRepo = itemsRepository.GetAllItems();
            CollectionAssert.Contains(itemsFromRepo, item);
        }

        [Test]
        public void Save_GivenValidItem_ShouldCallSaveChanges()
        {
            //---------------Set up test pack-------------------
            var item = ItemBuilder.BuildRandom();
            var items = new List<Item>();
            var dbSet = CreateDbSetWithAddRemoveSupport(items);
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            itemsRepository.Save(item);
            //---------------Test Result -----------------------
            lendingLibraryDbContext.Received().SaveChanges();
        }

        [Test]
        public void GetById_GivenIdIsNull_ShouldThrowExcption()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                itemsRepository.GetById(Guid.Empty);
            });
            //---------------Test Result -----------------------
            Assert.AreEqual("id", ex.ParamName);
        }
        
        [Test]
        public void GetById_GivenValidId_ShoulReturnItemWithMatchingId()
        {
            //---------------Set up test pack-------------------
            var item = new ItemBuilder().WithRandomProps().Build();
            var dbSet = new FakeDbSet<Item> {item}; 
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = itemsRepository.GetById(item.ItemId);
            //---------------Test Result -----------------------
            Assert.AreEqual(item, result);
        }

        [Test]
        public void DeleteItem_GivenItemNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                itemsRepository.DeleteItem(null);
            });
            //---------------Test Result -----------------------
            Assert.AreEqual("item", ex.ParamName);
        }

        [Test]
        public void DeleteItem_GivenValidItemId_ShouldDeleteItem()
        {
            //---------------Set up test pack-------------------
            var items = new List<Item>();
            var item = ItemBuilder.BuildRandom();

            var dbSet = CreateDbSetWithAddRemoveSupport(items);
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            itemsRepository.DeleteItem(item);
            //---------------Test Result -----------------------
            var itemsFromRepo = itemsRepository.GetAllItems();
            CollectionAssert.DoesNotContain(itemsFromRepo, item);
        }

        [Test]
        public void DeleteItem_GivenValidItem_ShouldCallSaveChanges()
        {
            //---------------Set up test pack-------------------
            var items = new List<Item>();
            var item = ItemBuilder.BuildRandom();

            var dbSet = CreateDbSetWithAddRemoveSupport(items);
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            itemsRepository.DeleteItem(item);
            //---------------Test Result -----------------------
            lendingLibraryDbContext.Received().SaveChanges();
        }

        [Test]
        public void Update_GivenInvalidExistingItem_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => itemsRepository.Update(null, new Item()));
            //---------------Test Result -----------------------
            Assert.AreEqual("existingItem", ex.ParamName);
        }

        [Test]
        public void Update_GivenInvalidNewItem_ShouldShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => itemsRepository.Update(new Item(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("newItem", ex.ParamName);
        }

        [Test]
        public void Update_GivenValidExistingAndNewItems_ShouldUpdateExistingItem()
        {
            //---------------Set up test pack-------------------
            var existingItem = new ItemBuilder().WithRandomProps().Build();
            var newItem = new ItemBuilder().WithRandomProps().Build();

            var dbSet = new FakeDbSet<Item> { existingItem};
            var lendingLibraryDbContext = CreateLendingLibraryDbContext(dbSet);
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);

            //---------------Assert Precondition----------------
            Assert.AreNotEqual(existingItem.ItemId, newItem.ItemId);
            Assert.AreNotEqual(existingItem.ItemName, newItem.ItemName);
            Assert.AreNotEqual(existingItem.CreatedUsername, newItem.CreatedUsername);
            Assert.AreNotEqual(existingItem.DateCreated, newItem.DateCreated);
            Assert.AreNotEqual(existingItem.DateLastModified, newItem.DateLastModified);
            Assert.AreNotEqual(existingItem.LastModifiedUsername, newItem.LastModifiedUsername);
            Assert.AreNotEqual(existingItem.Photo, newItem.Photo);
            //---------------Execute Test ----------------------
            itemsRepository.Update(existingItem, newItem);
            //---------------Test Result -----------------------
            Assert.AreEqual(existingItem.ItemName, newItem.ItemName);
            Assert.AreEqual(existingItem.CreatedUsername, newItem.CreatedUsername);
            Assert.AreEqual(existingItem.DateCreated, newItem.DateCreated);
            Assert.AreEqual(existingItem.DateLastModified, newItem.DateLastModified);
            Assert.AreEqual(existingItem.LastModifiedUsername, newItem.LastModifiedUsername);
            Assert.AreEqual(existingItem.Photo, newItem.Photo);
        }

        [Test]
        public void Update_GivenExistingItemHasBeenUpdatedWithNewValues_ShouldCallSaveChanges()
        {
            //---------------Set up test pack-------------------
            var existingItem = new ItemBuilder().WithRandomProps().Build();
            var newItem = new ItemBuilder().WithRandomProps().Build();

            var lendingLibraryDbContext = CreateLendingLibraryDbContext();
            var itemsRepository = CreateItemsRepository(lendingLibraryDbContext);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            itemsRepository.Update(existingItem, newItem);
            //---------------Test Result -----------------------
            lendingLibraryDbContext.Received().SaveChanges();
        }

        private static ItemsRepository CreateItemsRepository(ILendingLibraryDbContext lendingLibraryDbContext)
        {
            return new ItemsRepository(lendingLibraryDbContext);
        }

        private static IDbSet<Item> CreateDbSetWithAddRemoveSupport(List<Item> items)
        {
            var dbSet = Substitute.For<IDbSet<Item>>();
            
            dbSet.Add(Arg.Any<Item>()).Returns(info =>
            {
                items.Add(info.ArgAt<Item>(0));
                return info.ArgAt<Item>(0);
            });
            
            dbSet.Remove(Arg.Any<Item>()).Returns(info =>
            {
                items.Remove(info.ArgAt<Item>(0));
                return info.ArgAt<Item>(0);
            });

            dbSet.GetEnumerator().Returns(info => items.GetEnumerator());
            return dbSet;
        }

        private static ILendingLibraryDbContext CreateLendingLibraryDbContext(IDbSet<Item> dbSet = null)
        {
            if (dbSet == null) dbSet = CreateDbSetWithAddRemoveSupport(new List<Item>());
            var lendingLibraryDbContext = Substitute.For<ILendingLibraryDbContext>();
            lendingLibraryDbContext.Items.Returns(info => dbSet);
            return lendingLibraryDbContext;
        }
    }
}