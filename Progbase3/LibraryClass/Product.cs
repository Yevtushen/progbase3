namespace Progbase3
{
	class Product
	{
		public int id;
		public string name;
		public double price;
		public int left;
		public string description;

		public Product()
		{
			id = 0;
			name = "";
			price = 0;
			left = 0;
			description = "";
		}

		public Product(int id, string name, double price, int left, string description)
		{
			this.id = id;
			this.name = name;
			this.price = price;
			this.left = left;
			this.description = description;
		}

		public override string ToString()
		{
			return $"#{id} {name} {price}hrn \n {description} \n {left} left";
		}
	}
}
