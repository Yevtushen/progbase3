﻿using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Progbase3
{
	public class OrdersRepository
	{
		private SqliteConnection connection;

		public OrdersRepository(SqliteConnection connection)
		{
			this.connection = connection;
		}

        static Order GetOrder(SqliteDataReader reader)
        {
            Order o = new Order()
            {
                id = int.Parse(reader.GetString(0)),
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
    INSERT INTO orders (id, customer, custom, adress) 
    VALUES ($id, $customer, $custom, $adress);
 
    SELECT last_insert_rowid();
";
            command.Parameters.AddWithValue("$id", o.id);
            command.Parameters.AddWithValue("$customer", o.customer);
            command.Parameters.AddWithValue("$custom", o.custom);
            command.Parameters.AddWithValue("$adress", o.adress);

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
            command.CommandText = @"DELETE FROM orders WHERE id = $id";
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

        public List<Order> GetCustomersOrders(Customer c)
        {
            connection.Open();
            List<Order> orders = new List<Order>();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT FROM orders WHERE customer_id = $customer_id";
            command.Parameters.AddWithValue("$customer_id", c.id);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
			{
                orders.Add(GetOrder(reader));
			}
            reader.Close();
            return orders;
        }
    }
}
