using System.Xml.Serialization;

namespace WebServiceAutomation.Model.Xml_Model
{
    [XmlRoot(ElementName = "laptopDetailss")]
    public class LaptopDetailss
    {
        [XmlElement(ElementName = "Laptop")]
        public Laptop Laptop { get; set; }
    }
}
