﻿namespace LendingLibrary.Core.Domain
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public long PhoneNumber { get; set; }
    }
}