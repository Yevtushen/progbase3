namespace Progbase3
{
	class Order
	{
		public int id;
		public string customer;
		public int custom;
		public string adress;

		public Order()
		{
			id = 0;
			customer = "";
			custom = 0;
			adress = "";
		}

		public Order(int id, string customer, int custom, string adress)
		{
			this.id = id;
			this.customer = customer;
			this.custom = custom;
			this.adress = adress;
		}

		public override string ToString()
		{
			return $"{customer}, your order #{id} {custom} will be delivered to {adress}";
		}
	}
}
