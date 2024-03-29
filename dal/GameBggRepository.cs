using System;
using System.Collections.Generic;
using System.Linq;
using async_bgg.model.business;
using Microsoft.EntityFrameworkCore;

namespace async_bgg.DAL
{
    public class GameBggRepository
    {
        private BggDbContext db = new();

        public ICollection<Game> GetAllGames() => db.Games.ToList();

        public ICollection<Player> GetAllPlayers() => db.Players.ToList();

        public ICollection<Session> GetAllSessions() => db.Sessions.ToList();

        public void AddGame(Game newGame)
        {
            db.Games.Add(newGame);
            db.SaveChanges();
        }

        public void AddPlayer(Player newPlayer)
        {
            db.Players.Add(newPlayer);
            db.SaveChanges();
        }
        
        public void AddPlayerToSession(Session session, Player player)
        { 
            var sessionPlayer = new SessionPlayer { Player = player, Session = session};
            player.ParticipatedSessions.Add(sessionPlayer);
            db.Players.Update(player);
            db.SaveChanges();
        }
        
        public Session AddSession(Game game, Player winner)
        {
            var players = new List<SessionPlayer>();
            var session = new Session {Winner = winner, Date = DateTime.Now, SessionPlayers = players};
            game.Sessions.Add(session);
            db.Games.Update(game);
            db.SaveChanges();
            return session;
        }
        
        public void RemoveGame(int gameId)
        {
            var game = GetGameById(gameId);
            db.Games.Remove(game);
            db.SaveChanges();
        }
        
        public void RemovePlayer(int playerId)
        {
            var player = GetPlayerById(playerId);
            db.Players.Remove(player);
            db.SaveChanges();
        }
        
        public void RemoveSession(int sessionId)
        {
            var session = GetSessionBySessionId(sessionId);
            db.Sessions.Remove(session);
            db.SaveChanges();
        }
        
        public bool ContainsGame(Game game) => db.Games.Contains(game);

        public Game GetGameById(int id) => db.Games.SingleOrDefault(game => game.GameId == id);

        public Player GetPlayerById(int playerId) => db.Players.SingleOrDefault(player => player.PlayerId == playerId);

        public IEnumerable<Session> GetSessionsByGameId(int gameId) => db.Games
            .Where(g => g.GameId == gameId)
            .Include(g => g.Sessions)
            .SelectMany(g => g.Sessions)
            .Include(s => s.Winner)
            .Include(s => s.SessionPlayers)
            .ToList();
        
        private Session GetSessionBySessionId(int sessionId) => db.Sessions
            .Where(session => session.SessionId == sessionId)
            .Include(s => s.SessionPlayers)
            .SingleOrDefault();
    }
}