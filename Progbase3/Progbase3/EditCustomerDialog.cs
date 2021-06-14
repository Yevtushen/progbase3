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
		private TextField adressInput;
		private CheckBox isModerator;
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
				ReadOnly = true
			};
			Add(nameLbl, nameInput);

			Label adressLbl = new Label(2, 6, "Adress:");
			adressInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(adressLbl),
				Width = 40,
				ReadOnly = true
			};
			Add(adressLbl, adressInput);

			Label passwordLbl = new Label(2, 8, "Password:");
			passwordInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(passwordLbl),
				Width = 60,
				ReadOnly = true
			};
			Add(passwordLbl, passwordInput);

			isModerator = new CheckBox(2, 10, "Is moderator");
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
			adressInput.Text = customer.adress;
			isModerator.Checked = bool.Parse(customer.moderator.ToString());
		}

		public Customer GetCustomer()
		{
			return new Customer()
			{
				name = nameInput.Text.ToString(),
				adress = adressInput.Text.ToString(),
				moderator = bool.Parse(isModerator.Checked.ToString()),
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
