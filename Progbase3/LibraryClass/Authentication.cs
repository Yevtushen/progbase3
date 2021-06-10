using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace LibraryClass
{
	public class Authentication
	{
		private SHA256 sha256Hash = SHA256.Create();

		public Customer Register(CustomersRepository customersRepository, string userName, string password, string adress)
		{


			if (!customersRepository.GetToRegister(userName, adress))
			{
				string hashedPassword = GetHash(sha256Hash, password);
				Customer registered = new Customer()
				{
					name = userName,
					adress = adress,
					password = hashedPassword,
					orders = new List<Order>()
				};
				long newId = customersRepository.Insert(registered);
				if (newId != 0)
				{
					registered.id = newId;
					return registered;
				}
				return null;
			}
			return null;
		}

		public Customer LogIn(CustomersRepository customersRepository, string possibleName, string possiblePassword)
		{
			string hashed = GetHash(sha256Hash, possiblePassword);
			if (VerifyHash(sha256Hash, possiblePassword, hashed))
			{
				return customersRepository.GetToLogin(possibleName, hashed);
			}
			return null;
		}

		private static string GetHash(HashAlgorithm hashAlgorithm, string input)
		{

			byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
			var sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}

		private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
		{
			var hashOfInput = GetHash(hashAlgorithm, input);
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;
			return comparer.Compare(hashOfInput, hash) == 0;
		}
	}
}
