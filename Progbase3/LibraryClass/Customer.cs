using System.Collections.Generic;

namespace LibraryClass
{
	public class Customer
	{
		public long id;
		public string name;
		public string adress;
		public List<Order> orders;

		public Customer()
		{
			id = 0;
			name = "";
			adress = "";
			orders = new List<Order>();
		}

		public Customer(long id, string name, string adress, List<Order> orders)
		{
			this.id = id;
			this.name = name;
			this.adress = adress;
			this.orders = orders;
		}

		public override string ToString()
		{
			return $"#{id} {name}s adress is {adress}.";
		}
	}
}
