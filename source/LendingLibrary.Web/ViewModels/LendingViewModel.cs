﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Web.ViewModels
{
    public class LendingViewModel : ViewModelBase
    {
        //  public Person Person { get; set; }
        [Display(Name = "Borrowed By")]
        public string PersonName { get; set; }
        public Guid PersonId { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        public Guid ItemId { get; set; }
        [Required]
        [Display(Name = "Date Borrowed")]
        public DateTime? DateBorrowed { get; set; }
        [Required]
        [Display(Name = "Return Date")]
        public DateTime? DateReturned { get; set; }

        public SelectList PeopleSelectList { get; set; }
        public SelectList ItemsSelectList { get; set; }
        [Display(Name = "Days Lent Out")]
        public int DaysLentOut
        {
            get
            {
                if (DateBorrowed != null && DateReturned != null)
                    return DateReturned.Value.Day - DateBorrowed.Value.Day;
                return 0;
            }
        }

        public LendingStatus Status
        {
            get
            {
                if (DaysLentOut > 0)
                    return LendingStatus.NotAvailable;
                return LendingStatus.Available;
            }
        }

        public LendingViewModel()
        {
            PeopleSelectList = new SelectList("Value", "Text");
            ItemsSelectList = new SelectList("Value", "Text");
        }
    }
}