using System.Xml.Serialization;

namespace async_bgg.model.bgg
{
    [XmlRoot(ElementName = "name")]
    public class NameBgg
    {
        [XmlText] 
        public string Text { get; set; }
    }
}