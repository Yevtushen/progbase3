using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace LibraryClass
{
	class Authentication
	{
		public void Register(CustomersRepository customersRepository)
		{
			Random random = new Random();
			Console.WriteLine("Enter your name:");
			string userName = Console.ReadLine();
			Console.WriteLine("Enter password:");
			string password = Console.ReadLine();
			string hashedPassword = Sha256encrypt(password);
			Console.WriteLine("Enter your adress:");
			string adress = Console.ReadLine();
			Customer registed = new Customer()
			{
				id = random.Next(),
				name = userName,
				adress = adress,
				orders = new List<Order>()
			};
			long newId = customersRepository.Insert(registed);
			if (newId != 0)
			{
				Console.WriteLine("Registrated successfully!");
			}
			else
			{
				Console.WriteLine("Something wrong. Registration failed.");
			}
		}

		public void LogIn()
		{

		}

		private static string Sha256encrypt(string phrase)
		{
			UTF8Encoding encoder = new UTF8Encoding();
			SHA256Managed sha256hasher = new SHA256Managed();
			byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
			return Convert.ToBase64String(hashedDataBytes);
		}
	}
}
