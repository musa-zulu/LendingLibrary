using System;

namespace LendingLibrary.Core.Domain
{
    public enum Status
    {
        Available,
        NotAvailable
    }

    public class Lending
    {
        public Guid LedingId { get; set; }
        public Guid PersonId { get; set; }
        public Guid ItemId { get; set; }
        public DateTime? DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
        public Status Status { get; set; }
    }
}