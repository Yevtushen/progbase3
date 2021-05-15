namespace Progbase3
{
	class Customer
	{
		public int id;
		public string name;
		public string adress;

		public Customer()
		{
			id = 0;
			name = "";
			adress = "";
		}

		public Customer(int id, string name, string adress)
		{
			this.id = id;
			this.name = name;
			this.adress = adress;
		}

		public override string ToString()
		{
			return $"#{id} {name}s adress is {adress}";
		}
	}
}
