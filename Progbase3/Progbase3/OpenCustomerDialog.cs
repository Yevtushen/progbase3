using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	class OpenCustomerDialog : Dialog
	{
		public bool deleted;
		public bool updated;
		private Customer customer;
		private Customer listCustomer;
		private TextField idInput;
		private TextField nameInput;
		private TextField addressInput;
		private TextField passwordInput;

		public OpenCustomerDialog(Customer customer, Customer listCustomer)
		{			
			this.customer = customer;
			this.listCustomer = listCustomer;
			Title = "Customer settings";

			Button backBtn = new Button("Back");
			backBtn.Clicked += OnOpenDialogCanceled;
			AddButton(backBtn);

			int rightColumnX = 20;

			Label idLbl = new Label(2, 2, "ID:");
			idInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(idLbl),
				Width = 20,
				ReadOnly = true
			};
			Add(idLbl, idInput);

			Label nameLbl = new Label(2, 4, "Name:");
			nameInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(nameLbl),
				Width = 40,
				ReadOnly = true
			};
			Add(nameLbl, nameInput);

			Label addressLbl = new Label(2, 6, "Adress:");
			addressInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(addressLbl),
				Width = 40,
				ReadOnly = true
			};
			Add(addressLbl, addressInput);

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
			Add(passwordLbl, passwordInput);

			Button deleteBtn = new Button(2, 16, "Delete");
			deleteBtn.Clicked += OnCustomerDelete;
			Add(deleteBtn);

			if (customer.id == listCustomer.id)
			{
				Button editBtn = new Button("Update")
				{
					X = Pos.Right(deleteBtn) + 2,
					Y = Pos.Top(deleteBtn)
				};
				editBtn.Clicked += OnCustomerEdit;
				Add(editBtn);
			}
		}

		private void OnCustomerDelete()
		{
			int index = MessageBox.Query("Delete customer", "Are you sure?", "No", "Yes");
			if (index == 1)
			{
				deleted = true;
				Application.RequestStop();
			}
		}

		private void OnCustomerEdit()
		{
			EditCustomerDialog dialog = new EditCustomerDialog();
			dialog.SetCustomer(customer);

			Application.Run(dialog);
			if (!dialog.canceled)
			{				
				Customer updatedCustomer = dialog.GetCustomer();
				updated = true;
				SetCustomer(updatedCustomer);
			}
			nameInput.ReadOnly = true;
			addressInput.ReadOnly = true;
			passwordInput.ReadOnly = false;
			passwordInput.Visible = false;
		}

		private void OnOpenDialogCanceled()
		{
			Application.RequestStop();
		}

		public void SetCustomer(Customer customer)
		{
			this.customer = customer;
			idInput.Text = customer.id.ToString();
			nameInput.Text = customer.name;
			addressInput.Text = customer.address;
			
		}

		public Customer GetCustomer()
		{
			return customer;
		}
	}
}
