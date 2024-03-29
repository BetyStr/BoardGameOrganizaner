using System.ComponentModel.DataAnnotations;

namespace async_bgg.OperationEnums
{
    public enum Settings
    {
        [Display(Name = "Refresh game collection")] RefreshCollection,
        [Display(Name = "Change nickname")] ChangeNickname,
        [Display(Name = "Export as json to file")] ExportJson,
        [Display(Name = "Help")] Help,
        [Display(Name = "End")] End,
        [Display(Name = "Back")] Back
    }
}