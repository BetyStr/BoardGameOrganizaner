using System.ComponentModel.DataAnnotations;

namespace async_bgg.OperationEnums
{
    public enum Delete
    {
        [Display(Name = "Remove game/s")] RemoveGame, 
        [Display(Name = "Remove friend/s")] RemoveFriend,
        [Display(Name = "Remove session/s")] RemoveSession,
        [Display(Name = "Back")] Back
    }
}