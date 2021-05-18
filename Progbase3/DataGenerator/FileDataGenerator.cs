using System;
using System.Collections.Generic;
using System.IO;
using LibraryClass;

namespace DataGenerator
{
	class FileDataGenerator
	{
		public static Product GetRandomProduct()
		{
			Random random = new Random();
			Product p = new Product()
			{
				id = random.Next(),
				price = random.NextDouble(),
				left = random.Next(),
				name = GetRandomString("D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\name"),
				description = GetRandomString("D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\description")
			};
			return p;
		}

		public static Order GetRandomOrder(Product p)
		{
			Random random = new Random();
			Order o = new Order()
			{
				id = random.Next(),
				customer = new Customer()
				{
					id = random.Next(),
					name = GetRandomString("D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\name"),
					adress = GetRandomString("D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\adress"),
					orders = new List<Order>()
				}
			};
			return o;
		}

		public static Customer GetRandomCustomer()
		{
			Random random = new Random();
			Customer c = new Customer()
			{
				id = random.Next(),
				name = GetRandomString("D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\name"),
				adress = GetRandomString("D:\\victo\\kpi\\progbase projects\\progbase3\\data\\generator\\adress"),
				orders = new List<Order>()
			};
			return c;
		}

		public static string GetRandomString(string filePath)
		{
			Random random = new Random();
			List<string> newList = new List<string>();
			StreamReader sr = new StreamReader(filePath);
			while (true)
			{

				string s = sr.ReadLine();
				if (s == null)
				{
					break;
				}
				newList.Add(s);
			}
			sr.Close();
			int index = random.Next(newList.Count);
			return newList[index];
		}
	}
}
