using System.Collections.Generic;
using System.Xml.Serialization;


namespace LibraryClass
{
	[System.Serializable()]
	[XmlType(AnonymousType = true)]

	public class Product
	{
		[XmlElement("id")]
		public long id { get; set; }
		[XmlElement("name")]
		public string name { get; set; }
		[XmlElement("price")]
		public int price { get; set; }
		[XmlElement("left")]
		public int left { get; set; }
		[XmlElement("desc")]
		public string description { get; set; }
		public List<Order> orders;

		public Product()
		{
			id = 0;
			name = "";
			price = 0;
			left = 0;
			description = "";
			orders = new List<Order>();
		}
		
		public override string ToString()
		{
			return $"#{id} {name}";
		}
	}
}
