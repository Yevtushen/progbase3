using System.Collections.Generic;

namespace LibraryClass
{
	public class Order
	{
		public long id;
		public long customer_id;
		//public Customer customer;
		public List<Product> products;

		public Order()
		{
			id = 0;
			customer_id = 0;
			//customer = new Customer();
			products = new List<Product>();
		}

		public Order(long id, long customer_id, Customer customer, List<Product> products)
		{
			this.id = id;
			this.customer_id = customer_id;
			//this.customer = customer;
			this.products = products;
		}

		public override string ToString()
		{
			return $"See order {id}";
		}
	}
}
