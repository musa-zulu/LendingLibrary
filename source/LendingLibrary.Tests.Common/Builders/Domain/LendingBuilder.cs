using System;
using LendingLibrary.Core.Domain;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Tests.Common.Builders.Domain
{
    public class LendingBuilder : GenericBuilder<LendingBuilder, Lending>
    {
        public LendingBuilder WithId(Guid id)
        {
            return WithProp(x => x.LedingId = id);
        }

        public LendingBuilder WithStatus(Status status)
        {
            return WithProp(x => x.Status = status);
        }

        public LendingBuilder WithRandomGeneratedId()
        {
            return WithProp(x => x.LedingId = Guid.NewGuid());
        }
    }
}