using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	class OpenCustomerDialog : Dialog
	{
		public bool deleted;
		public bool updated;
		protected Customer customer;
		protected TextField idInput;
		protected TextField nameInput;
		protected TextField adressInput;
		protected CheckBox isModerator;
		private TextField passwordInput;

		public OpenCustomerDialog(Customer c)
		{
			this.Title = "Open Customer";

			Button backBtn = new Button("Back");
			backBtn.Clicked += OnOpenDialogCanceled;
			this.AddButton(backBtn);

			int rightColumnX = 20;

			Label idLbl = new Label(2, 2, "ID:");
			idInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(idLbl),
				Width = 20,
				ReadOnly = true
			};
			this.Add(idLbl, idInput);

			Label nameLbl = new Label(2, 4, "Name:");
			nameInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(nameLbl),
				Width = 40,
				ReadOnly = true
			};
			this.Add(nameLbl, nameInput);

			Label adressLbl = new Label(2, 6, "Adress:");
			adressInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(adressLbl),
				Width = 40,
				ReadOnly = true
			};
			this.Add(adressLbl, adressInput);

			isModerator = new CheckBox(2, 10, "Is moderator");
			isModerator.Visible = false;
			this.Add(isModerator);

			Label passwordLbl = new Label(2, 8, "Your password");
			passwordLbl.Visible = false;
			passwordInput = new TextField("")
			{ 
				X = rightColumnX,
				Y = Pos.Top(passwordLbl),
				Width = 40,
				ReadOnly = true,
				Visible = false
			};
			this.Add(passwordLbl, passwordInput);

			Button editBtn = new Button(2, 16, "Update");
			editBtn.Clicked += OnCustomerEdit;
			this.Add(editBtn);

			Button deleteBtn = new Button("Delete")
			{
				X = Pos.Right(editBtn) + 2,
				Y = Pos.Top(editBtn)
			};
			deleteBtn.Clicked += OnCustomerDelete;
			this.Add(deleteBtn);
		}

		private void OnCustomerDelete()
		{
			int index = MessageBox.Query("Delete customer", "Are you sure?", "No", "Yes");
			if (index == 1)
			{
				this.deleted = true;
				Application.RequestStop();
			}
		}

		private void OnCustomerEdit()
		{
			EditCustomerDialog dialog = new EditCustomerDialog();
			dialog.SetCustomer(this.customer);
			Application.Run(dialog);
			if (!dialog.canceled)
			{
				nameInput.ReadOnly = false;
				adressInput.ReadOnly = false;
				passwordInput.Visible = true;
				passwordInput.ReadOnly = false;
				if (customer.moderator)
				{
					isModerator.Visible = true;
				}
				Customer updatedCustomer = dialog.GetCustomer();
				this.updated = true;
				this.SetCustomer(updatedCustomer);
			}
			nameInput.ReadOnly = true;
			adressInput.ReadOnly = true;
			passwordInput.ReadOnly = false;
			passwordInput.Visible = false;
			isModerator.Visible = false;
		}

		private void OnOpenDialogCanceled()
		{
			Application.RequestStop();
		}

		public void SetCustomer(Customer customer)
		{
			this.customer = customer;
			this.idInput.Text = customer.id.ToString();
			this.nameInput.Text = customer.name;
			this.adressInput.Text = customer.adress;
			
		}

		public Customer GetCustomer()
		{
			return this.customer;
		}
	}
}
