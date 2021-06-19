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
            Product product = new Product()
            {
                id = long.Parse(reader.GetString(0)),
                name = reader.GetString(1),
                price = int.Parse(reader.GetString(2)),
                left = int.Parse(reader.GetString(3)),
                description = reader.GetString(4)
            };

            return product;
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
            command.CommandText = @"INSERT INTO orders (name, price, left, description) VALUES ($name, $price, $left, $description); SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$name", p.name);
            command.Parameters.AddWithValue("$price", p.price);
            command.Parameters.AddWithValue("$left", p.left);
            command.Parameters.AddWithValue("$description", p.description);
            long newId = (long)command.ExecuteScalar();

            for (int i = 0; i < p.orders.Count; i++)
			{
                SqliteCommand command1 = connection.CreateCommand();
                command1.CommandText = @"INSERT INTO product_to_order (product_id, order_id) VALUES ($product_id, $order_id); SELECT last_insert_rowid();";
                command1.Parameters.AddWithValue("$product_id", newId);
                command1.Parameters.AddWithValue("$order_id", p.orders[i].id);
                command1.ExecuteNonQuery();
            }
            
            connection.Close();
            return newId;
        }

        public bool Delete(long id)
        {
            connection.Open();

            SqliteCommand command1 = connection.CreateCommand();
            command1.CommandText = @"DELETE FROM product_to_order WHERE product_id = $id";
            command1.Parameters.AddWithValue("$id", id);
            command1.ExecuteNonQuery();

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

        public List<Product> GetProducts()
        {
            connection.Open();
            List<Product> productsList = new List<Product>();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                productsList.Add(GetProduct(reader));
            }
            reader.Close();
            connection.Close();
            return productsList;
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
            command.CommandText = @"UPDATE products SET name = $name, price = $price, left = $left, description = $description WHERE id = $id";
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

        public int GetSearchPagesCount(int pageSize, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return GetTotalPages(pageSize);
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            connection.Open();
            value = $"%{value}%";
            List<Product> searchProducts = new List<Product>();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products WHERE name LIKE $value"; //
            command.Parameters.AddWithValue("$value", value);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                searchProducts.Add(GetProduct(reader));
            }
            return (int)Math.Ceiling(searchProducts.Count / (double)pageSize);
        }

        public List<Product> GetSearchPage(string value, int pageNumber, int pageSize)
        {
            if (string.IsNullOrEmpty(value))
            {
                return GetPage(pageNumber, pageSize);
            }
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            int pageEnd = pageSize * (pageNumber - 1);
            value = $"%{value}%";
            command.CommandText = @"SELECT * FROM products WHERE name LIKE $value LIMIT $pageSize OFFSET $pageNumberEnd";
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumberEnd", pageEnd);
            command.Parameters.AddWithValue("$value", value);
            SqliteDataReader reader = command.ExecuteReader();
            List<Product> page = new List<Product>();
            while (reader.Read())
            {
                page.Add(GetProduct(reader));
            }
            reader.Close();
            connection.Close();
            return page;
        }


        public Dictionary<long, List<Product>> GetProductsInOrders()
        {
            Dictionary<long, List<Product>> productsInOrders = new Dictionary<long, List<Product>>();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT product_to_order.order_id, products.* FROM products JOIN product_to_order ON products.id = product_to_order.product_id";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var orderId = long.Parse(reader.GetString(0));
                if (productsInOrders.ContainsKey(orderId))
                {
                    List<Product> products = productsInOrders[orderId];
                    products.Add(GetProductForOrder(reader));
                }
                else
                {
                    List<Product> products = new List<Product>();
                    products.Add(GetProductForOrder(reader));
                    productsInOrders.Add(orderId, products);
                }
            }
            reader.Close();
            connection.Close();
            return productsInOrders;
        }

        private static Product GetProductForOrder(SqliteDataReader reader)
        {
            Product product = new Product()
            {
                id = long.Parse(reader.GetString(1)),
                name = reader.GetString(2),
                price = int.Parse(reader.GetString(3)),
                left = int.Parse(reader.GetString(4)),
                description = reader.GetString(5)
            };

            return product;
        }
    }
}
