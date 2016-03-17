using System;

namespace LendingLibrary.Core.Domain
{
    public class Lending
    {
        public Guid LedingId { get; set; }
        
        public Guid PersonId { get; set; }
        public string PersonName { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public DateTime? DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
        public LendingStatus? LendingStatus { get; set; }
    }
}