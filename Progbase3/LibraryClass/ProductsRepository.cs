using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace LibraryClass
{
	public class ProductsRepository
	{
		private SqliteConnection connection;

		public ProductsRepository(SqliteConnection connection)
		{
			this.connection = connection;
		}

        public static Product GetProduct(SqliteDataReader reader)
        {
            Product p = new Product()
            {
                id = int.Parse(reader.GetString(0)),
                name = reader.GetString(1),
                price = double.Parse(reader.GetString(2)),
                left = int.Parse(reader.GetString(3)),
                description = reader.GetString(4)
            };

            return p;
        }

        public Product GetById(int id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Product p = GetProduct(reader);
                reader.Close();
                connection.Close();
                return p;
            }
            else
            {
                connection.Close();
                return null;
            }
        }

        public long Insert(Product p)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
    INSERT INTO orders (id, name, price, left, description) 
    VALUES ($id, $name, $price, $left, $description);
 
    SELECT last_insert_rowid();
";
            command.Parameters.AddWithValue("$id", p.id);
            command.Parameters.AddWithValue("$name", p.name);
            command.Parameters.AddWithValue("$price", p.price);
            command.Parameters.AddWithValue("$left", p.left);
            command.Parameters.AddWithValue("$description", p.description);

            long newId = (long)command.ExecuteScalar();
            /*if (newId == 0)
            {
                Console.WriteLine("Internet provider not added.");
            }
            else
            {
                Console.WriteLine("Internet provider added. New id is: " + newId);
            }*/
            connection.Close();
            return newId;
        }

        public int Delete(int id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            return nChanged;
            /*if (nChanged == 0)
            {
                Console.WriteLine("Book NOT deleted.");
            }
            else
            {
                Console.WriteLine("Book deleted.");
            }
            */
        }

        public List<Product> GetExport(string valueX)
        {
            connection.Open();
            List<Product> csvList = new List<Product>();
            SqliteCommand command = connection.CreateCommand();
            valueX = $"%{valueX}%";
            command.CommandText = @"SELECT * FROM internetproviders WHERE name LIKE $valueX";
            command.Parameters.AddWithValue("$valueX", valueX);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                csvList.Add(GetProduct(reader));
            }
            reader.Close();
            connection.Close();
            return csvList;
        }

        public List<double> ExportProductPrice()
		{
            List<double> prices = new List<double>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT price FROM products";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
			{
                prices.Add(double.Parse(reader.GetString(0)));
			}
            reader.Close();
            connection.Close();
            return prices;
        }

        public List<string> ExportProductName()
        {
            List<string> names = new List<string>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT name FROM products";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                names.Add(reader.GetString(0));
            }
            reader.Close();
            connection.Close();
            return names;
        }

        public List<Order> GetOrdersOfProduct(int order_id)
		{
            List<Order> orders = new List<Order>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products CROSS JOIN product_to_order WHERE products.id = product_to_order.product_id AND product_to_order.order_id =$order_id";
            command.Parameters.AddWithValue("$order_id", order_id);
            command.CommandText = @" $product_id";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
			{
                orders.Add(OrdersRepository.GetOrder(reader));
			}
            reader.Close();
            connection.Close();
            return orders;
        }

    }
}
