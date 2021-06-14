using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Progbase3
{
	public class RegistrationDialog : Dialog
	{
		public bool canceled;
		private TextField nameInput;
		private TextField adressInput;
		private TextField passwordInput;

		public RegistrationDialog()
		{
			int rightColumnX = 20;

			Title = "Registration";

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

			Label adressLbl = new Label(2, 4, "Enter your adress:");
			 adressInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(adressLbl),
				Width = 40
			};
			Add(adressLbl, adressInput);

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

		public string GetAdress()
		{
			return adressInput.Text.ToString();
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
