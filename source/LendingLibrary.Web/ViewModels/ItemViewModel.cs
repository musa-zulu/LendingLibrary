using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LendingLibrary.Web.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        [Required]
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [Required]
        [DisplayName("Item Desciption")]
        public string ItemDescription { get; set; }
        public byte[] Photo { get; set; }
    }
}
