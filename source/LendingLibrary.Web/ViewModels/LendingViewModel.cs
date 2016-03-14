using System;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Web.ViewModels
{
    public class LendingViewModel : ViewModelBase
    {
        public Person Person { get; set; }
        public Guid PersonId { get; set; }
        public Item Item { get; set; }
        public Guid ItemId { get; set; }
        public DateTime? DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
        public Status Status { get; set; }


        public int DaysLentOut
        {
            get
            {
                if (DateBorrowed != null && DateReturned != null) 
                    return DateReturned.Value.Day - DateBorrowed.Value.Day;
                return 0;
            }
        }
    }
}