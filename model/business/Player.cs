using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace async_bgg.model.business
{
    public class Player
    {
       // [ForeignKey("Session")] 
        public int PlayerId { get; init; }

        [JsonIgnore]
        public ICollection<SessionPlayer> ParticipatedSessions { get; }
        public string Firstname { get; init; }
        public string Surname { get; init; }

        public Player()
        {
            ParticipatedSessions = new List<SessionPlayer>();
        }

        private bool Equals(Player other) => PlayerId == other.PlayerId;

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
            return obj.GetType() == GetType() && Equals((Player) obj);
        }

        public override int GetHashCode() => PlayerId;

        public override string ToString() => $"{Firstname} {Surname}";
    }
}