using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace LibraryClass
{
	public class CustomersRepository
	{
		private SqliteConnection connection;

		public CustomersRepository(SqliteConnection connection)
		{
			this.connection = connection;
		}

        static Customer GetCustomer(SqliteDataReader reader)
        {
            Customer customer = new Customer()
            {
                id = int.Parse(reader.GetString(0)),
                name = reader.GetString(1),
                adress = reader.GetString(2),
                password = reader.GetString(3),
                moderator = Convert.ToBoolean(int.Parse(reader.GetString(4)))
            };
            return customer;
        }

        public Customer GetById(int id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM customers WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Customer c = GetCustomer(reader);
                reader.Close();
                connection.Close();
                return c;
            }
            else
            {
                connection.Close();
                return null;
            }
        }

        public long Insert(Customer c)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
    INSERT INTO customers (name, adress, password, moderator) 
    VALUES ($name, $adress, $password, $moderator);
 
    SELECT last_insert_rowid();
";
            command.Parameters.AddWithValue("$name", c.name);
            command.Parameters.AddWithValue("$adress", c.adress);
            command.Parameters.AddWithValue("$password", c.password);
            command.Parameters.AddWithValue("$moderator", 0);
            long newId = (long)command.ExecuteScalar();
         
            connection.Close();
            return newId;
        }

        public bool Delete(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM customers WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            if (nChanged == 0)
			{
                return false;
			}
            return true;
        }

        public Customer GetToLogin(string name, string password)
		{
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM customers WHERE name = $name AND password = $password";
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$password", password);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
			{
                Customer c = GetCustomer(reader);
                reader.Close();
                connection.Close();
                return c;
            }
            else
			{
                reader.Close();
                connection.Close();
                return null;
			}
        }

        public bool GetToRegister(string name, string adress)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM customers WHERE name = $name AND adress = $adress";
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$adress", adress);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Update(long id, Customer c)
		{
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE customers SET name = $name, adress = $adress, password = $password WHERE id = $id";
            command.Parameters.AddWithValue("$name", c.name);
            command.Parameters.AddWithValue("$adress", c.adress);
            command.Parameters.AddWithValue("$password", c.password);
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
            command.CommandText = @"SELECT COUNT(*) FROM customers";
            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        public int GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        }

        public List<Customer> GetPage(int pageNumber, int pageSize)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            int pageEnd = pageSize * (pageNumber - 1);
            command.CommandText = @"SELECT * FROM customers LIMIT $pageSize OFFSET $pageNumberEnd";
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumberEnd", pageEnd);
            SqliteDataReader reader = command.ExecuteReader();
            List<Customer> customers = new List<Customer>();
            while (reader.Read())
            {
                customers.Add(GetCustomer(reader));
            }
            reader.Close();
            connection.Close();
            return customers;
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
            List<Customer> searchCustomers = new List<Customer>();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM customers WHERE name LIKE $value";
            command.Parameters.AddWithValue("$value", value);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                searchCustomers.Add(GetCustomer(reader));
            }
            return (int)Math.Ceiling(searchCustomers.Count / (double)pageSize);
        }

        public List<Customer> GetSearchPage(string value, int pageNumber, int pageSize)
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
            command.CommandText = @"SELECT * FROM customers LIMIT $pageSize OFFSET $pageNumberEnd WHERE name LIKE $value";
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumberEnd", pageEnd);
            command.Parameters.AddWithValue("$value", value);
            SqliteDataReader reader = command.ExecuteReader();
            List<Customer> page = new List<Customer>();
            while (reader.Read())
            {
                page.Add(GetCustomer(reader));
            }
            reader.Close();
            connection.Close();
            return page;
        }
    }
}
