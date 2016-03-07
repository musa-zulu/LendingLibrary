using System;
using System.Collections;
using System.Collections.Generic;
using LendingLibrary.Core.Domain;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace LendingLibrary.Core.Tests.Domain
{
    [TestFixture]
    public class TestItem
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new Item());
            //---------------Test Result -----------------------
        }

        [TestCase("ItemName", typeof(string))]
        public void Type_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(Item);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);

            //---------------Test Result -----------------------
        }
    }
}