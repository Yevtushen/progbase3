using System.Collections.Generic;

namespace Progbase3
{
	public class Order
	{
		public long id;
		public long customer_id;
		public Customer customer;
		public List<Product> product;

		public Order()
		{
			id = 0;
			customer_id = 0;
			customer = new Customer();
			product = new List<Product>();
		}

		public Order(long id, long customer_id, Customer customer, List<Product> product)
		{
			this.id = id;
			/*this.customer = customer;
			this.custom = custom;
			this.adress = adress;*/
			this.customer_id = customer_id;
			this.customer = customer;
			this.product = product;
		}

		public override string ToString()
		{
			return ""; //$"{customer}, your order #{id} {custom} will be delivered to {adress}";
		}
	}
}
