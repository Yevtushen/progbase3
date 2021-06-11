using System.Collections.Generic;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class OpenOrderDialog : Dialog
	{
		public bool deleted;
		private Order order;
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
			int index = MessageBox.Query("Delete order", "Are you sure?", "No", "Yes");
			if (index == 1)
			{
				deleted = true;
				foreach (var p in order.products)
				{
					p.left ++;
				}
				Application.RequestStop();
			}
		}

		public Order GetOrder()
		{
			return this.order;
		}
		
		public void SetOrder (Order order)
		{
			this.order = order;
			idInput.Text = order.id.ToString();
			string products = "";
			foreach (var p in order.products)
			{
				products += p.name + " ";
			}
			productsInput.Text = products;
		}

		private void OnOpenDialogCanceled()
		{

			Application.RequestStop();
		}
	}
}
