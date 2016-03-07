using System;
using LendingLibrary.Core.Domain;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;


namespace LendingLibrary.Core.Tests.Domain
{
    [TestFixture]
    public class TestPerson
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new Person());
            //---------------Test Result -----------------------
        }
        
        [TestCase("FirstName", typeof(string))]
        [TestCase("LastName", typeof(string))]
        [TestCase("Email", typeof(string))]
        [TestCase("PhoneNumber", typeof(long))]
        public void Type_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(Person);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);

            //---------------Test Result -----------------------
        }
    }
}