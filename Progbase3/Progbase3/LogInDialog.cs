using System;
using System.Collections.Generic;
using System.Text;
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
			
			this.Title = "Log In";

			Button submitBtn = new Button("OK!");
			submitBtn.Clicked += UserSubmit;
			this.AddButton(submitBtn);

			Button backBtn = new Button("Back");
			backBtn.Clicked += OnDialogCanceled;
			this.AddButton(backBtn);

			Label nameLbl = new Label(2, 2, "Enter your name:");
			nameInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(nameLbl),
				Width = 40
			};
			this.Add(nameLbl, nameInput);

			Label passwordLbl = new Label(2, 6, "Enter your password");
			passwordInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(passwordLbl),
				Width = 40,
				Secret = true
			};
			this.Add(passwordLbl, passwordInput);
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
			this.canceled = false;
			Application.RequestStop();
		}

		public void OnDialogCanceled()
		{
			canceled = true;
			Application.RequestStop();
		}
	}
}
