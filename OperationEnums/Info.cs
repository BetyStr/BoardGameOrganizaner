using System.ComponentModel.DataAnnotations;

namespace async_bgg.OperationEnums
{
    public enum Info
    {
        [Display(Name = "Get random game")] RandomGame,
        [Display(Name = "Show all games")] ShowGames,
        [Display(Name = "Show info about game")] ShowGameInfo,
        [Display(Name = "Show all players")] ShowPlayers,
        [Display(Name = "Show all sessions of game")] ShowSessionsOfGame,
        [Display(Name = "Back")] Back
    }
}