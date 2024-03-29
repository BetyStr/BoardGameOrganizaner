using System.ComponentModel.DataAnnotations;

namespace async_bgg.OperationEnums
{
    public enum Add
    {
        [Display(Name = "Add player")] AddPlayer,
        [Display(Name = "Add session")] AddSession,
        [Display(Name = "Back")] Back
    }
}