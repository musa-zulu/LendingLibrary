namespace LendingLibrary.Web.ViewModels
{
    public class PersonViewModel : ViewModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Photo { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}