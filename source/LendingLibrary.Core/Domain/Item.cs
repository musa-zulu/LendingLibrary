using System;

namespace LendingLibrary.Core.Domain
{
    public class Item : EntityBase
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public byte[] Photo { get; set; }
    }
}