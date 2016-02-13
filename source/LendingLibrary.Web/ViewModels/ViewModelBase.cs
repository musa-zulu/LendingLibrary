using System;

namespace LendingLibrary.Web.ViewModels
{
    public class ViewModelBase
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedUsername { get; set; }
        public DateTime DateLastModified { get; set; }
        public string LastModifiedUsername { get; set; }
    }
}