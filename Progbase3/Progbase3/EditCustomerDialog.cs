using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;
using System.Security.Cryptography;



namespace Progbase3
{
	public class EditCustomerDialog : Dialog
	{
		public bool canceled;
		protected TextField nameInput;
		protected TextField adressInput;
		protected CheckBox isModerator;
		protected TextField passwordInput;
		private SHA256 sha256Hash = SHA256.Create();

		public EditCustomerDialog()
		{
			this.Title = "Edit customer";
		}

		public void SetCustomer(Customer customer)
		{
			this.nameInput.Text = customer.name;
			this.adressInput.Text = customer.adress;
			this.isModerator.Text = customer.moderator.ToString();
		}

		public Customer GetCustomer()
		{
			return new Customer()
			{
				name = nameInput.Text.ToString(),
				adress = adressInput.Text.ToString(),
				moderator = bool.Parse(isModerator.Text.ToString()),
				password = GetHash(sha256Hash, passwordInput.Text.ToString())
			};
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
	}
}
