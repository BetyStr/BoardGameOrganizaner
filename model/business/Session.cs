using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace async_bgg.model.business
{
    public class Session
    {
        public int SessionId { get; init; }
        public Player Winner { get; init; }

        public ICollection<SessionPlayer> SessionPlayers { get; set; }
        public DateTime Date { get; init; }
        
        public Session()
        {
            SessionPlayers = new List<SessionPlayer>();
        }

        public override string ToString()
        { 
            var result =  new StringBuilder($"Game session played at {Date} won player {Winner}")
                .Append(Environment.NewLine);
            if (SessionPlayers.Count == 0)
            {
                result.Append("No others players or all players has been deleted");
            }
            else
            {
                result.Append("Player/s in this session: ")
                    .Append(string.Join(", ", SessionPlayers.Select(sp => sp.Player)));
            }

            return result.ToString();
        }
           

        private bool Equals(Session other) => SessionId == other.SessionId;

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
            return obj.GetType() == GetType() && Equals((Session) obj);
        }

        public override int GetHashCode() => SessionId;
        
    }
    
}