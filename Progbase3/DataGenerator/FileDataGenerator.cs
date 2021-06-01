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
				price = random.Next(),
				left = random.Next(),
				name = GetRandomString("../../data/generator/name.txt"),
				description = GetRandomString("../../data/generator/description.txt")
			};
			return p;
		}

		public static Order GetRandomOrder()
		{
			Random random = new Random();
			Order o = new Order()
			{
				id = random.Next(),
				customer = new Customer()
				{
					id = random.Next(),
					name = GetRandomString("../../data/generator/name.txt"),
					adress = GetRandomString("../../data/generator/adress.txt"),
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
				name = GetRandomString("../../data/generator/name.txt"),
				adress = GetRandomString("../../data/generator/adress.txt"),
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
