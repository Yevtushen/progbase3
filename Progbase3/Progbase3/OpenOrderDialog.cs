using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class OpenOrderDialog : Dialog
	{
		public bool deleted;
		private Order order;
		private TextField idInput;
		private TextField productsInput;

		public OpenOrderDialog(Order order)
		{
			Title = "Open order";
			this.order = order;

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

			Label productsLbl = new Label(2, 4, "Products:");
			productsInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(productsLbl),
				Width = 40,
				ReadOnly = true
			};
			Add(productsLbl, productsInput);

			Button deleteBtn = new Button(2, 16, "Delete");
			deleteBtn.Clicked += OnOrderDelete;
			Add(deleteBtn);
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
			return order;
		}
		
		public void SetOrder (Order order)
		{
			this.order = order;
			idInput.Text = order.id.ToString();
			string products = "";
			for (int i = 0; i < order.products.Count; i++)
			{
				products += order.products[i].name;
				if (i != order.products.Count - 1)
				{
					products += ", ";
				}
			}
			productsInput.Text = products;
		}

		private void OnOpenDialogCanceled()
		{

			Application.RequestStop();
		}
	}
}
