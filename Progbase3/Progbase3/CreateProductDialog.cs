using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	class CreateProductDialog : Dialog
	{
		public bool canceled;
		protected TextField nameInput;
		protected TextField priceInput;
		protected TextField leftInput;
		protected TextField descriptionInput;

		public CreateProductDialog()
		{
			Title = "Create product";

			Button okBtn = new Button("OK");
			Button canselBtn = new Button("Cancel");
			canselBtn.Clicked += OnCreateDialogCanceled;
			okBtn.Clicked += OnCreateDialogSubmitted;
			AddButton(canselBtn);
			AddButton(okBtn);

			int rightColumnX = 20;

			Label nameLbl = new Label(2, 4, "Name:");
			nameInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(nameLbl),
				Width = 40,
				ReadOnly = true
			};
			Add(nameLbl, nameInput);

			Label priceLbl = new Label(2, 6, "Price:");
			priceInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(priceLbl),
				Width = 40,
				ReadOnly = true
			};
			Add(priceLbl, priceInput);

			Label leftLbl = new Label(2, 8, "Left:");
			leftInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(leftLbl),
				Width = 60,
				ReadOnly = true
			};
			Add(leftLbl, leftInput);

			Label descriptionLbl = new Label(2, 10, "Description:");
			descriptionInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(descriptionLbl),
				Width = 40,
				ReadOnly = true
			};
			Add(descriptionLbl, descriptionInput);
		}

		public Product GetProduct()
		{
			return new Product()
			{
				name = nameInput.Text.ToString(),
				price = int.Parse(priceInput.Text.ToString()),
				left = int.Parse(leftInput.Text.ToString()),
				description = descriptionInput.Text.ToString()
				
			};
		}

		private void OnCreateDialogSubmitted()
		{
			canceled = false;
			Application.RequestStop();
		}

		private void OnCreateDialogCanceled()
		{
			canceled = true;
			Application.RequestStop();
		}
	}
}
