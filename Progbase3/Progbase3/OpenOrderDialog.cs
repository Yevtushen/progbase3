using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class OpenOrderDialog : Dialog
	{
		public bool deleted;
		public bool updated;
		private TextField idInput;
		private TextView productsInput;

		public OpenOrderDialog(Customer c)
		{
			this.Title = "Open order";

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

			Label productsLbl = new Label(2, 4, "Products:");
			productsInput = new TextView()
			{
				X = rightColumnX,
				Y = Pos.Top(productsLbl),
				Width = 40,
				Text = "",
				ReadOnly = true
			};
			this.Add(productsLbl, productsInput);

			Button deleteBtn = new Button(2, 16, "Delete");
			deleteBtn.Clicked += OnOrderDelete;
			this.Add(deleteBtn);
		}

		private void OnOrderDelete()
		{
			int index = MessageBox.Query("Delete activity", "Are you sure?", "No", "Yes");
			if (index == 1)
			{
				this.deleted = true;
				Application.RequestStop();
			}
		}

		private void OnOpenDialogCanceled()
		{
			Application.RequestStop();
		}
	}
}
