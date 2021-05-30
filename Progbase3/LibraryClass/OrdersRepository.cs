using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace LibraryClass
{
	public class OrdersRepository
	{
		private SqliteConnection connection;

		public OrdersRepository(SqliteConnection connection)
		{
			this.connection = connection;
		}

        public static Order GetOrder(SqliteDataReader reader)
        {
            Order o = new Order()
            {
                id = long.Parse(reader.GetString(0)),
                customer_id = long.Parse(reader.GetString(1))
                /*
                customer = reader.GetString(1),
                custom = int.Parse(reader.GetString(2)),
                adress = reader.GetString(3)*/
            };

            return o;
        }

        public Order GetById(int id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM orders WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Order o = GetOrder(reader);
                reader.Close();
                connection.Close();
                return o;
            }
            else
            {
                connection.Close();
                return null;
            }
        }

        public long Insert(Order o)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
    INSERT INTO orders (id, customer_id) 
    VALUES ($id, $customer_id);
 
    SELECT last_insert_rowid();
";
            command.Parameters.AddWithValue("$id", o.id);
            command.Parameters.AddWithValue("$customer", o.customer_id);
            long newId = (long)command.ExecuteScalar();
            connection.Close();
            return newId;
        }

        public int Delete(int id)
		{
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM orders WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            return nChanged;
        }

        public List<Order> GetCustomersOrders(long customer_id)
        {
            connection.Open();
            List<Order> orders = new List<Order>();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM orders WHERE customer_id = $customer_id";
            command.Parameters.AddWithValue("$customer_id", customer_id);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
			{
                orders.Add(GetOrder(reader));
			}
            reader.Close();
            connection.Close();
            return orders;
        }

        public List<Product> GetProductsInOrder(int product_id)
		{
            List<Product> products = new List<Product>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM orders CROSS JOIN product_to_order WHERE orders.id = product_to_order.order_id AND product_to_order.product_id = $product_id";
            command.Parameters.AddWithValue("$product_id", product_id);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
			{
                products.Add(ProductsRepository.GetProduct(reader));
			}
            reader.Close();
            return products;
        }

    }
}
