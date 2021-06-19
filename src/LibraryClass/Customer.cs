using System.Collections.Generic;

namespace LibraryClass
{
	public class Customer
	{
		public long id;
		public string name;
		public string address;
		public string password;
		public bool moderator;
		public List<Order> orders;

		public Customer()
		{
			id = 0;
			name = "";
			address = "";
			password = "";
			moderator = false;
			orders = new List<Order>();
		}

		
		public override string ToString()
		{
			return $"#{id} {name}";
		}
	}
}
