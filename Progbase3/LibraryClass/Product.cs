using System.Collections.Generic;

namespace LibraryClass
{
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

		public Product(long id, string name, int price, int left, string description, List<Order> orders)
		{
			this.id = id;
			this.name = name;
			this.price = price;
			this.left = left;
			this.description = description;
			this.orders = orders;
		}

		public override string ToString()
		{
			return $"#{id} {name}";
		}
	}
}
