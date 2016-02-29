using System;
using LendingLibrary.Core.Domain;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Tests.Common.Builders.Domain
{
    public class PersonBuilder : GenericBuilder<PersonBuilder, Person>
    {
        public PersonBuilder WithFirstName(string firstName)
        {
            return WithProp(p => p.FirstName = firstName);
        }

        public PersonBuilder WithLastName(string lastName)
        {
            return WithProp(p => p.LastName = lastName);
        }

        public PersonBuilder WithPhoneNumber(long phoneNumber)
        {
            return WithProp(p => p.PhoneNumber = phoneNumber);
        }
        
        public PersonBuilder WithId(Guid id)
        {
            return WithProp(p => p.Id = id);
        }
    }
}