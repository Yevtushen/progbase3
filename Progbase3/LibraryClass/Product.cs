using System.Collections.Generic;


namespace LibraryClass
{
	[System.Serializable()]

	public class Product
	{
		public long id;
		public string name;
		public int price;
		public int left;
		public string description;
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
