using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Progbase3
{
	public class LogInDialog : Dialog
	{
		private TextField nameInput;
		private TextField passwordInput;

		public LogInDialog()
		{
			int rightColumnX = 20;
			
			this.Title = "Log In";

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

		public void OnDialogCanceled()
		{
			Application.RequestStop();
		}
	}
}
