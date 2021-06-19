using System.Text;
using Terminal.Gui;
using LibraryClass;
using System.Security.Cryptography;

namespace Progbase3
{
	public class EditCustomerDialog : Dialog
	{
		public bool canceled;
		private TextField nameInput;
		private TextField addressInput;
		private TextField passwordInput;
		private SHA256 sha256Hash = SHA256.Create();

		public EditCustomerDialog()
		{
			Title = "Edit customer";
			int rightColumnX = 20;

			Button okBtn = new Button("OK");
			Button canselBtn = new Button("Cancel");
			canselBtn.Clicked += OnEditDialogCanceled;
			okBtn.Clicked += OnEditDialogSubmitted;
			AddButton(canselBtn);
			AddButton(okBtn);

			Label nameLbl = new Label(2, 4, "Name:");
			nameInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(nameLbl),
				Width = 40,
			};
			Add(nameLbl, nameInput);

			Label adressLbl = new Label(2, 6, "Adress:");
			addressInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(adressLbl),
				Width = 40,
			};
			Add(adressLbl, addressInput);

			Label passwordLbl = new Label(2, 8, "Password:");
			passwordInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(passwordLbl),
				Width = 60,
			};
			Add(passwordLbl, passwordInput);
		}

		private void OnEditDialogSubmitted()
		{
			canceled = false;
			Application.RequestStop();
		}

		private void OnEditDialogCanceled()
		{
			canceled = true;
			Application.RequestStop();
		}

		public void SetCustomer(Customer customer)
		{
			nameInput.Text = customer.name;
			addressInput.Text = customer.address;
		}

		public Customer GetCustomer()
		{
			return new Customer()
			{
				name = nameInput.Text.ToString(),
				address = addressInput.Text.ToString(),
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
