using System.Xml.Serialization;

namespace async_bgg.model.bgg
{
    [XmlRoot(ElementName = "stats")]
    public class StatsBgg
    {
        [XmlElement(ElementName = "rating")] 
        public RatingBgg Rating { get; set; }
        
        [XmlAttribute(AttributeName = "minplayers")] 
        public int MinPlayers { get; set; }

        [XmlAttribute(AttributeName = "maxplayers")]
        public int MaxPlayers { get; set; }

        [XmlAttribute(AttributeName = "minplaytime")]
        public int MinPlayTime { get; set; }

        [XmlAttribute(AttributeName = "maxplaytime")]
        public int MaxPlayTime { get; set; }

    }
}