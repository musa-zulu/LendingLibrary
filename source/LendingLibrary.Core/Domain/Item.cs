using System;

namespace LendingLibrary.Core.Domain
{
    public class Item : EntityBase
    {
        public string ItemName { get; set; }
        public byte[] Photo { get; set; }
    }
}