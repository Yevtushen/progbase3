using System;
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
                id = long.Parse(reader.GetString(0)),
                name = reader.GetString(1),
                price = int.Parse(reader.GetString(2)),
                left = int.Parse(reader.GetString(3)),
                description = reader.GetString(4)
            };

            return p;
        }

        public Product GetById(long id)
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
    INSERT INTO orders (name, price, left, description) 
    VALUES ($name, $price, $left, $description);
 
    SELECT last_insert_rowid();
";

            command.Parameters.AddWithValue("$name", p.name);
            command.Parameters.AddWithValue("$price", p.price);
            command.Parameters.AddWithValue("$left", p.left);
            command.Parameters.AddWithValue("$description", p.description);
            long newId = (long)command.ExecuteScalar();
            connection.Close();
            return newId;
        }

        public bool Delete(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            if (nChanged == 0)
			{
                return false;
			}
            return true;
        }

        public List<Product> GetExport(string valueX)
        {
            connection.Open();
            List<Product> csvList = new List<Product>();
            SqliteCommand command = connection.CreateCommand();
            valueX = $"%{valueX}%";
            command.CommandText = @"SELECT * FROM products WHERE name LIKE $valueX";
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

        public List<int> ExportProductPrice()
		{
            List<int> prices = new List<int>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT price FROM products";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
			{
                prices.Add(int.Parse(reader.GetString(0)));
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

        public List<Order> GetOrdersOfProduct(long customer_id)
		{
            List<Order> orders = new List<Order>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products CROSS JOIN product_to_order WHERE products.id = product_to_order.product_id AND product_to_order.order_id = $customer_id";
            command.Parameters.AddWithValue("$customer_id", customer_id);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
			{
                orders.Add(OrdersRepository.GetOrder(reader));
			}
            reader.Close();
            connection.Close();
            return orders;
        }

        public bool Update(long id, Product p)
		{
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE activities SET name = $name, price = $price, left = $left, description = $description WHERE id = $id";
            command.Parameters.AddWithValue("$name", p.name);
            command.Parameters.AddWithValue("$price", p.price);
            command.Parameters.AddWithValue("$left", p.left);
            command.Parameters.AddWithValue("$description", p.description);
            command.Parameters.AddWithValue("$id", id);
            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            if (nChanged == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private long GetCount()
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM products";
            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        public int GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        }

        public List<Product> GetPage(int pageNumber, int pageSize)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            int pageEnd = pageSize * (pageNumber - 1);
            command.CommandText = @"SELECT * FROM products LIMIT $pageSize OFFSET $pageNumberEnd";
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumberEnd", pageEnd);
            SqliteDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                products.Add(GetProduct(reader));
            }
            reader.Close();
            connection.Close();
            return products;
        }
    }
}
