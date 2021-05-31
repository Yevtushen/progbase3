using System.Collections.Generic;

namespace LibraryClass
{
	public class Customer
	{
		public long id;
		public string name;
		public string adress;
		public string password;
		public bool moderator;
		public List<Order> orders;

		public Customer()
		{
			id = 0;
			name = "";
			adress = "";
			password = "";
			moderator = false;
			orders = new List<Order>();
		}

		public Customer(long id, string name, string adress, string password, bool moderator, List<Order> orders)
		{
			this.id = id;
			this.name = name;
			this.adress = adress;
			this.password = password;
			this.moderator = moderator;
			this.orders = orders;
		}

		public override string ToString()
		{
			return $"#{id} {name}s adress is {adress}.";
		}
	}
}
