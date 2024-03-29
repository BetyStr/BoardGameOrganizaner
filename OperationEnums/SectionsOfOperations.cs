using System.ComponentModel.DataAnnotations;

namespace async_bgg.OperationEnums
{
    public enum SectionsOfOperations
    {
        [Display(Name = "Add entity")] Add,
        [Display(Name = "Show info")] Info,
        [Display(Name = "Delete entity")] Delete,
        [Display(Name = "Settings")] Settings
    }
}