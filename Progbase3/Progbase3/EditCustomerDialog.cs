using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class EditCustomerDialog : Dialog
	{
		public bool canceled;
		protected TextField nameInput;
		protected TextField adressInput;
		protected CheckBox isModerator;

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
				moderator = bool.Parse(isModerator.Text.ToString())
			};
		}
	}
}
