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
            Customer c = new Customer()
            {
                id = int.Parse(reader.GetString(0)),
                name = reader.GetString(1),
                adress = reader.GetString(2)
            };
            return c;
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
    INSERT INTO customers (adress, password) 
    VALUES ($adress, $password);
 
    SELECT last_insert_rowid();
";
            command.Parameters.AddWithValue("$adress", c.adress);
            command.Parameters.AddWithValue("$password", c.password);
            long newId = (long)command.ExecuteScalar();
         
            connection.Close();
            return newId;
        }

        public int Delete(int id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM customers WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            return nChanged;
        }

        public Customer GetToLogin(string name, string password)
		{
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "@SELECT * FROM customers WHERE name = $name, password = $password";
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
            command.CommandText = "@SELECT * FROM customers WHERE name = $name, adress = $adress";
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
    }
}
