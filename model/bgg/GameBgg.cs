using System.Xml.Serialization;

namespace async_bgg.model.bgg
{
    [XmlRoot(ElementName = "item")]
    public class GameBgg
    {
        [XmlElement(ElementName = "name")] 
        public NameBgg Name { get; set; }
        
        [XmlElement(ElementName = "stats")] 
        public StatsBgg Stats { get; set; }
        
        [XmlAttribute(AttributeName = "objectid")] 
        public int ObjectId { get; set; }
    }
}