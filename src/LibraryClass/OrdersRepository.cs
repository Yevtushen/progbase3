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
            Order order = new Order()
            {
                id = long.Parse(reader.GetString(0)),
                customer_id = long.Parse(reader.GetString(1)),
            };

            return order;
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
            command.CommandText = @"INSERT INTO orders (customer_id) VALUES ($customer_id); SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$customer_id", o.customer_id);
            long newId = (long)command.ExecuteScalar();

            for (int i = 0; i < o.products.Count; i++)
			{
                SqliteCommand command1 = connection.CreateCommand();
                command1.CommandText = @"INSERT INTO product_to_order (product_id, order_id) VALUES ($product_id, $order_id); SELECT last_insert_rowid();";
                command1.Parameters.AddWithValue("$product_id", o.products[i].id);
                command1.Parameters.AddWithValue("$order_id", newId);
                command1.ExecuteScalar();
            }

            connection.Close();
            return newId;
        }

        public bool Delete(long id)
		{
            connection.Open();

            SqliteCommand command1 = connection.CreateCommand();
            command1.CommandText = @"DELETE FROM product_to_order WHERE order_id = $id";
            command1.Parameters.AddWithValue("$id", id);
            command1.ExecuteNonQuery();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM orders WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
           
            long nChanged = command.ExecuteNonQuery();
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

        public List<Order> GetOrdersOfProduct(long productId)
        {
            List<Order> orders = new List<Order>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT orders.* FROM orders CROSS JOIN product_to_order WHERE orders.id = product_to_order.order_id AND product_to_order.product_id = $product_id";
            command.Parameters.AddWithValue("$product_id", productId);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(GetOrder(reader));
            }
            reader.Close();
            connection.Close();
            return orders;
        }

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
            List<Order> orders = new List<Order>();
            SqliteCommand command = connection.CreateCommand();
            int pageEnd = pageSize * (pageNumber - 1);
            command.CommandText = @"SELECT * FROM orders WHERE customer_id = $customer_id LIMIT $pageSize OFFSET $pageNumberEnd";
            command.Parameters.AddWithValue("$customer_id", customer_id);
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumberEnd", pageEnd);
            SqliteDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {                    
                Order order = new Order()
                {
                    id = long.Parse(reader.GetString(0)),
                    customer_id = long.Parse(reader.GetString(1)),
                };
                order.products = GetProductsInOrder(order.id);
                orders.Add(order);
             }
            reader.Close();
            connection.Close();
            return orders;
        }

        public List<Product> GetProductsInOrder(long orderId)
        {
            List<Product> products = new List<Product>();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT products.* FROM products CROSS JOIN product_to_order WHERE products.id = product_to_order.product_id AND product_to_order.order_id = $order_id";
            command.Parameters.AddWithValue("$order_id", orderId);
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
