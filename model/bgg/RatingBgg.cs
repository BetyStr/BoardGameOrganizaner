using System.Xml.Serialization;

namespace async_bgg.model.bgg
{
    [XmlRoot(ElementName = "rating")]
    public class RatingBgg
    {
        [XmlElement(ElementName = "average")] 
        public AverageBgg Average { get; set; }
        
        [XmlElement(ElementName = "bayesaverage")] 
        public BayesAverageBgg BayesAverage { get; set; }
    }

    [XmlRoot(ElementName = "average")]
    public class AverageBgg
    {
        [XmlAttribute(AttributeName = "value")] 
        public double Value { get; set; }
    }

    [XmlRoot(ElementName = "bayesaverage")]
    public class BayesAverageBgg
    {
        [XmlAttribute(AttributeName = "value")] 
        public double Value { get; set; }
    }
}