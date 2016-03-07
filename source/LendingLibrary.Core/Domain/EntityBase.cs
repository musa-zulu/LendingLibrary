using System;

namespace LendingLibrary.Core.Domain
{
    public class EntityBase
    {
        public DateTime DateCreated { get; set; }
        public string CreatedUsername { get; set; }
        public DateTime DateLastModified { get; set; }
        public string LastModifiedUsername { get; set; }
    }
}
