using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using LibraryClass;

namespace Progbase3
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}


		private static void DBProcessing()
		{
			string databaseString = "D:\\victo\\kpi\\progbase projects\\progbase3\\data\\newbase.db";
			SqliteConnection connection = new SqliteConnection($"Data Source={databaseString}");
			
		}

		private static Product GetRandomProduct()
		{
			Random random = new Random();
			Product p = new Product() 
			{
			id = random.Next(),
			price = random.NextDouble(),
			left = random.Next(),
			name = "D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\name",
			description = "D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\description"
			}; 
			return p;
		}

		private static Order GetRandomOrder(Product p)
		{
			Random random = new Random();
			Order o = new Order()
			{
				id = random.Next(),
				customer = new Customer()
				//customer = "D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\customer",
			};
			return o;
		}

		private static Customer GetRandomCustomer()
		{
			Random random = new Random();
			Customer c = new Customer()
			{
				id = random.Next(),
				name = "D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\customer",
				adress = "D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\adress",
				orders = new List<Order>()
			};
			return c;
		}
	}
}
