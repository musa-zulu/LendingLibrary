using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Web.ViewModels
{
    public class PersonViewModel : ViewModelBase
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public byte[] Photo { get; set; }
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
        public Title Title { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}