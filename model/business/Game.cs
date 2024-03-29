using System;
using System.Collections.Generic;
using System.Text;

namespace async_bgg.model.business
{
    public class Game
    {
        public int GameId { get; init; }
        public string Name { get; init; }

        public ICollection<Session> Sessions { get; }
        public int MinPlayers { get; init; }
        public int MaxPlayers { get; init; }
        public int MinPlaytime { get; init; }
        public int MaxPlaytime { get; init; }
        public double AverageBggRating { get; init; }
        public double BayesAverageBgg { get; init; }

        public Game()
        {
            Sessions = new List<Session>();
        }

        public override string ToString() => Name;
        
        public string Info() => new StringBuilder()
                .Append("Name: ").Append(Name).Append(Environment.NewLine)
                .Append("Number_of_players: ").Append(MinPlayers).Append('-').Append(MaxPlayers).Append(Environment.NewLine)
                .Append("Play_time: ").Append(MinPlaytime).Append(" - ").Append(MaxPlaytime).Append(Environment.NewLine)
                .Append("Rating: ").Append(AverageBggRating).Append(" - ").Append(BayesAverageBgg).ToString();
        

        private bool Equals(Game other) => GameId == other.GameId;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == GetType() && Equals((Game) obj);
        }

        public override int GetHashCode() => GameId;
    }
}