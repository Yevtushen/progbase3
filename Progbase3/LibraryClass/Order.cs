using System.Collections.Generic;

namespace LibraryClass
{
	public class Order
	{
		public long id;
		public long customer_id;
		public List<Product> products;

		public Order()
		{
			id = 0;
			customer_id = 0;
			products = new List<Product>();
		}

		public override string ToString()
		{
			return $"See order {id}";
		}
	}
}
