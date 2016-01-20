﻿using System.ComponentModel;

namespace LendingLibrary.Web.ViewModels
{
    public class ItemViewModel
    {
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [DisplayName("Person Name")]
        public string PersonName { get; set; }
        [DisplayName("Days Lent Out")]
        public int DaysLentOut { get; set; }
    }
}
