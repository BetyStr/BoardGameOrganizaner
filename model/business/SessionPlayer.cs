using System;
using System.Text.Json.Serialization;

namespace async_bgg.model.business
{
    public class SessionPlayer
    {
        public int SessionId { get; init;}
        
        public int PlayerId { get; init; }

        [JsonIgnore]
        public Session Session { get; init; }
        public Player Player { get; init; }

        private bool Equals(SessionPlayer other) => SessionId == other.SessionId && PlayerId == other.PlayerId;

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
            return obj.GetType() == GetType() && Equals((SessionPlayer) obj);
        }

        public override int GetHashCode() => HashCode.Combine(SessionId, PlayerId);
    }
}