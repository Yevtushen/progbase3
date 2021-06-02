using System;
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
                customer_id = long.Parse(reader.GetString(1)),
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

        public List<Product> GetProductsInOrder(long customer_id)
		{
            List<Product> products = new List<Product>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products CROSS JOIN product_to_order WHERE products.id = product_to_order.product_id AND product_to_order.order_id = $customer_id";
            command.Parameters.AddWithValue("$customer_id", customer_id);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
			{
                products.Add(ProductsRepository.GetProduct(reader));
			}
            reader.Close();
            return products;
        }

        /*public bool Update(long id, Order o)
		{
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE orders SET type = $type, name = $name, comment = $comment WHERE id = $id";
            command.Parameters.AddWithValue("$type", a.type);
            command.Parameters.AddWithValue("$name", a.name);
            command.Parameters.AddWithValue("$comment", a.comment);
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
		}*/

        private long GetCount(long customer_id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM orders WHERE customer_id = $customer_id";
            command.Parameters.AddWithValue("$customer_id", customer_id);
            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        public int GetTotalPages(int pageSize, long customer_id)
        {
            return (int)Math.Ceiling(this.GetCount(customer_id) / (double)pageSize);
        }

        public List<Order> GetPage(int pageNumber, int pageSize, long customer_id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            int pageEnd = pageSize * (pageNumber - 1);
            command.CommandText = @"SELECT * FROM orders WHERE customer_id = $customer_id LIMIT $pageSize OFFSET $pageNumberEnd";
            command.Parameters.AddWithValue("$customer_id", customer_id);
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumberEnd", pageEnd);
            SqliteDataReader reader = command.ExecuteReader();
            List<Order> orders = new List<Order>();
            while (reader.Read())
            {
                orders.Add(GetOrder(reader));
            }
            reader.Close();
            connection.Close();
            return orders;
        }
    }
}
