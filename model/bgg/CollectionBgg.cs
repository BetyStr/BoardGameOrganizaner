using System.Collections.Generic;
using System.Xml.Serialization;

namespace async_bgg.model.bgg
{
    [XmlRoot(ElementName = "items")]
    public class CollectionBgg
    {
        [XmlElement(ElementName = "item")] 
        public List<GameBgg> Games { get; set; }
    }
}