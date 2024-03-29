using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using async_bgg.OperationEnums;
using Sharprompt;

namespace async_bgg
{
    public static class App
    {
        private static readonly BoardGameManager boardGameManager = new();
        private static bool isRunning = true;

        private static readonly Dictionary<Enum, Action> operationsActions = new()
        {
            {Add.AddPlayer, boardGameManager.AddFriend},
            {Add.AddSession, boardGameManager.AddSession},
            {Info.RandomGame, boardGameManager.GetRandomGame},
            {Info.ShowGames, async () => await boardGameManager.ShowUserGames()}, 
            {Info.ShowGameInfo, boardGameManager.ShowGameInfo},
            {Info.ShowPlayers, boardGameManager.ShowPlayers},
            {Info.ShowSessionsOfGame, boardGameManager.GetSessionsByGame},
            {Delete.RemoveGame, boardGameManager.RemoveGame},
            {Delete.RemoveFriend, boardGameManager.RemovePlayer},
            {Delete.RemoveSession, boardGameManager.RemoveSession},
            {Settings.RefreshCollection, async () => await boardGameManager.RefreshCollection()},
            {Settings.ChangeNickname, boardGameManager.ChangeUsername},
            {Settings.ExportJson, boardGameManager.ExportToFile},
            {Settings.Help, BoardGameManager.PrintHelp},
            {Settings.End, StopRunning}
        };
        
        private static void StopRunning() => isRunning = false;

        public static void Main() => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            Prompt.ColorSchema.Answer = ConsoleColor.DarkBlue;
            Prompt.ColorSchema.Select = ConsoleColor.DarkMagenta;
            BoardGameManager.PrintHelp();
            const string message = "Select operation to perform";
            while (isRunning)
            {
                var type = Prompt.Select<SectionsOfOperations>("Select type of operation to perform");
                Enum value = type switch
                {
                    SectionsOfOperations.Add => Prompt.Select<Add>(message),
                    SectionsOfOperations.Delete => Prompt.Select<Delete>(message),
                    SectionsOfOperations.Info => Prompt.Select<Info>(message),
                    _ => Prompt.Select<Settings>(message)
                };
                var operation = operationsActions.GetValueOrDefault(value, () => { }); // back
                operation(); 
                Console.WriteLine();
            }
        }
    }
}