using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Schema;
using async_bgg.DAL;
using async_bgg.model.business;
using Sharprompt;

namespace async_bgg
{
    public class BoardGameManager
    {
        private readonly BggClient bggClient = new();
        private readonly GameBggRepository bggRepository = new();
        private readonly IList<Func<object, ValidationResult>> validator = 
            new[] { Validators.Required(), Validators.MinLength(3) };
        
        private string username = "betkaS";
        private const int LinesPerSite = 10;

        public async Task RefreshCollection()
        {
            try
            {
                var bggCollection = await bggClient.DownloadUserCollectionAsync(username);
                foreach (var game in bggCollection.Games.Select(bggGame => new Game
                {
                    GameId = bggGame.ObjectId,
                    Name = bggGame.Name.Text,
                    AverageBggRating = bggGame.Stats.Rating.Average.Value,
                    BayesAverageBgg = bggGame.Stats.Rating.BayesAverage.Value,
                    MaxPlayers = bggGame.Stats.MaxPlayers,
                    MaxPlaytime = bggGame.Stats.MaxPlayTime,
                    MinPlayers = bggGame.Stats.MinPlayers,
                    MinPlaytime = bggGame.Stats.MinPlayTime,
                }))
                {
                    if (!bggRepository.ContainsGame(game))
                    {
                        bggRepository.AddGame(game);
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException)
            {
                await Console.Error.WriteLineAsync(
                    "Failed to refresh game collection. Try to check your internet connection.");
            }
            catch (InvalidOperationException)
            {
                await Console.Error.WriteLineAsync(
                    "Failed to refresh game collection. Try to change nickname.");
            }
        }

        public async Task ShowUserGames()
        {
            if (bggRepository.GetAllGames().Count == 0)
            {
                await RefreshCollection();
            }

            foreach (var game in bggRepository.GetAllGames())
            {
                Console.WriteLine(game);
            }
        }
        
        public void ShowGameInfo()
        {
            var game = Prompt.Select("Which game info would you like to see?", bggRepository.GetAllGames(), pageSize: LinesPerSite);
            Console.WriteLine(bggRepository.GetGameById(game.GameId).Info());
        }

        public static void PrintHelp()
        {   
            Console.WriteLine("**************************************************************************************");
            Console.WriteLine("*                                Board Game Tracker                                  *");
            Console.WriteLine("**************************************************************************************");
            Console.WriteLine("* At the begging you can choose from 4 categories:                                   *");
            Console.WriteLine("*   - Add entity    : Add player, Add session                                        *");
            Console.WriteLine("*   - Delete entity : Remove game/s, Remove player/s, Remove session/s               *");
            Console.WriteLine("*   - Show info     : Get random game, Show all games, Show info about game,         *");
            Console.WriteLine("*                     Show all players, Show all sessions of game                    *");
            Console.WriteLine("*   - Settings      : Refresh game collection, Change nickname, Export as json, End  *");
            Console.WriteLine("*                                                                                    *");
            Console.WriteLine("*  Controller: ->      next page                                                     *");
            Console.WriteLine("*              <-      previous page                                                 *");
            Console.WriteLine("*              space   select multiple choices                                       *");
            Console.WriteLine("*              enter   confirm choice/s                                              *");
            Console.WriteLine("**************************************************************************************");
        }

        public void AddFriend()
        {
            var name = Prompt.Input<string>("What's firstname of a new player?",validators: validator);
            var surname = Prompt.Input<string>("What's surname of a new player?", validators: validator);
            bggRepository.AddPlayer(new Player {Firstname = name, Surname = surname});
        }

        public void AddSession()
        {
            var game = Prompt.Select("Which game would you like to play?", 
                bggRepository.GetAllGames(), LinesPerSite);
            var players = (List<Player>) Prompt.MultiSelect("Which players played?", 
                bggRepository.GetAllPlayers(), pageSize: LinesPerSite);
            var winner = Prompt.Select("Which player won?", players, pageSize: LinesPerSite);
            var idPlayers = players.Select(player => player.PlayerId).ToList();
            var session = bggRepository.AddSession(game, winner);
            foreach (var playerId in idPlayers)
            {
                bggRepository.AddPlayerToSession(session, bggRepository.GetPlayerById(playerId));
            }
        }
        
        public void ShowPlayers()
        {
            foreach (var player in bggRepository.GetAllPlayers())
            {
                Console.WriteLine(player);
            }
        }
        
        public void GetSessionsByGame()
        {
            var game = Prompt.Select("Which game?", bggRepository.GetAllGames(), pageSize: LinesPerSite);
            foreach (var session in bggRepository.GetSessionsByGameId(game.GameId))
            {
                Console.WriteLine(session);
            }
        }
        
        public void RemoveGame()
        {
            var games = bggRepository.GetAllGames();
            if (games.Count == 0)
            {
                Console.WriteLine("There are no games, please create some.");
                return;
            }
            var gamesToDelete = Prompt.MultiSelect("Which games would you like to remove?", games, pageSize: LinesPerSite);
            foreach (var game in gamesToDelete)
            {
                bggRepository.RemoveGame(game.GameId);
            }
        }
        
        public void RemovePlayer()
        {
            var players = bggRepository.GetAllPlayers();
            if (players.Count == 0)
            {
                Console.WriteLine("There are no players, please add some.");
                return;
            }
            var playersToDelete = Prompt.MultiSelect("Which friends would you like to remove?", players, pageSize: LinesPerSite);
            foreach (var friend in playersToDelete)
            {
                bggRepository.RemovePlayer(friend.PlayerId);
            }
        }
        
        public void RemoveSession()
        {
            var sessions = bggRepository.GetAllSessions();
            if (sessions.Count == 0)
            {
                Console.WriteLine("There are no sessions, please create some.");
                return;
            }
            var sessionsToDelete = Prompt.MultiSelect("Which sessions would you like to remove?",
                sessions, pageSize: LinesPerSite);
            foreach (var session in sessionsToDelete)
            {
                bggRepository.RemoveSession(session.SessionId);
            }
        }
        
        public void GetRandomGame()
        {
            var random = new Random();
            var games = (List<Game>) bggRepository.GetAllGames();
            if (games.Count == 0)
            {
                Console.WriteLine("There are no games, please add some.");
                return;
            }
            Console.WriteLine(games[random.Next(games.Count)].Info());
        }
        
        public void ChangeUsername()
        {
            username = Prompt.Input<string>("What's your nickname at BGG?", validators: validator);
        }
        
        public async void ExportToFile()
        {
            var games = bggRepository.GetAllGames();
            if (games.Count == 0)
            {
                Console.WriteLine("There is nothing to be exported.");
                return;
            }
            var exportPath = Prompt.Input<string>("Where do you want to export the games?");
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(games, options);
            try
            {
                await File.WriteAllLinesAsync(exportPath, new[] {jsonString});
            }
            catch (IOException)
            {
                await Console.Error.WriteLineAsync("Writing process failed. Check provided information.");
            }
        }
    }
}