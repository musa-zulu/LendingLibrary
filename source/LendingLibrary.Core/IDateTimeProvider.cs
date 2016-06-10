using System;

namespace LendingLibrary.Core
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}