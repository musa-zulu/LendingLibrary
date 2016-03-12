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
    }
}