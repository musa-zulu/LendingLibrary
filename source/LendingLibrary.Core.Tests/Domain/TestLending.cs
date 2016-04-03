using System;
using LendingLibrary.Core.Domain;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace LendingLibrary.Core.Tests.Domain
{
    [TestFixture]
    public class TestLending
    {
        [Test]
        public void Contruct()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new Lending());
            //---------------Test Result -----------------------
        }

        [TestCase("LedingId", typeof(Guid))]
        [TestCase("PersonId", typeof(Guid))]
        [TestCase("ItemId", typeof(Guid))]
        [TestCase("PersonName", typeof(string))]
        [TestCase("DateBorrowed", typeof(DateTime?))]
        [TestCase("DateReturned", typeof(DateTime?))]
        [TestCase("LendingStatus", typeof(LendingStatus?))]
        public void Type_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(Lending);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);

            //---------------Test Result -----------------------
        }
    }
}