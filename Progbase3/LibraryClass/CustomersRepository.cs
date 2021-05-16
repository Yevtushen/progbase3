using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Progbase3
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
                //orders = 
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
    INSERT INTO customers (id, name, adress) 
    VALUES ($id, $name, $adress);
 
    SELECT last_insert_rowid();
";
            command.Parameters.AddWithValue("$id", c.id);
            command.Parameters.AddWithValue("$name", c.name);
            command.Parameters.AddWithValue("$adress", c.adress);

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
            command.CommandText = @"DELETE FROM customers WHERE id = $id";
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

        /*public List<Order> GetCustomersOrders ()
		{
            connection.Open();
            SqliteCommand command = connection.CreateCommand();

        }*/
    }
}
