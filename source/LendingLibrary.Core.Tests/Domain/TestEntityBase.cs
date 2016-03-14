using System;
using LendingLibrary.Core.Domain;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace LendingLibrary.Core.Tests.Domain
{
    [TestFixture]
    public class TestEntityBase
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new EntityBase());
            //---------------Test Result -----------------------
        }
        
        [TestCase("DateCreated", typeof(DateTime?))]
        [TestCase("CreatedUsername", typeof(string))]
        [TestCase("DateLastModified", typeof(DateTime?))]
        [TestCase("LastModifiedUsername", typeof(string))]
        public void Type_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(EntityBase);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);

            //---------------Test Result -----------------------
        }
    }
}