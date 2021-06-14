using Terminal.Gui;

namespace Progbase3
{
	public class LogInDialog : Dialog
	{
		public bool canceled;
		private TextField nameInput;
		private TextField passwordInput;

		public LogInDialog()
		{
			int rightColumnX = 20;
			
			Title = "Log In";

			Button submitBtn = new Button("OK!");
			submitBtn.Clicked += UserSubmit;
			AddButton(submitBtn);

			Button backBtn = new Button("Back");
			backBtn.Clicked += OnDialogCanceled;
			AddButton(backBtn);

			Label nameLbl = new Label(2, 2, "Enter your name:");
			nameInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(nameLbl),
				Width = 40
			};
			Add(nameLbl, nameInput);

			Label passwordLbl = new Label(2, 6, "Enter your password");
			passwordInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(passwordLbl),
				Width = 40,
				Secret = true
			};
			Add(passwordLbl, passwordInput);
		}

		public string GetName()
		{
			return nameInput.Text.ToString();
		}

		public string GetPassword()
		{
			return passwordInput.Text.ToString();
		}

		private void UserSubmit()
		{
			canceled = false;
			Application.RequestStop();
		}

		public void OnDialogCanceled()
		{
			canceled = true;
			Application.RequestStop();
		}
	}
}
