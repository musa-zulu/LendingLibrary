using System;
using System.Collections.Generic;

namespace LendingLibrary.Core.Domain
{
    /// <summary>
    /// for more infor about titles
    /// https://en.wikipedia.org/wiki/Title 
    /// https://www.drupal.org/node/1439292
    /// </summary>
    public enum Title
    {
        Mr,
        Mrs,
        Miss,
        Mx,
        Master,
        Maid,
        Madam,
        Dr,
        Prof
    }
    public class Person : EntityBase
    {
        public Guid PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public Title Title { get; set; }
    }
}