using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	class OpenProductDialog : Dialog
	{
		public bool deleted;
		public bool updated;
		protected Product product;
		protected TextField idInput;
		protected TextField nameInput;
		protected TextField priceInput;
		protected TextField leftInput;
		protected TextView descriptionInput;

		public OpenProductDialog(Customer c)
		{
			this.Title = "Open Product";

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

			Label nameLbl = new Label(2, 4, "Name:");
			nameInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(nameLbl),
				Width = 40,
				ReadOnly = true
			};
			this.Add(nameLbl, nameInput);

			Label priceLbl = new Label(2, 6, "Price:");
			priceInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(priceLbl),
				Width = 40,
				ReadOnly = true
			};
			this.Add(priceLbl, priceInput);

			Label leftLbl = new Label(2, 8, "Left:");
			leftInput = new TextField("")
			{
				X = rightColumnX,
				Y = Pos.Top(leftLbl),
				Width = 40,
				ReadOnly = true
			};
			this.Add(leftLbl, leftInput);

			Label descriptionLbl = new Label(2, 10, "Description:");
			descriptionInput = new TextView()
			{
				X = rightColumnX,
				Y = Pos.Top(descriptionLbl),
				Width = 40,
				Text = "",
				ReadOnly = true
			};
			this.Add(descriptionLbl, descriptionInput);

			if (c.moderator)
			{
				Button editBtn = new Button(2, 16, "Update");
				editBtn.Clicked += OnProductEdit;
				this.Add(editBtn);

				Button deleteBtn = new Button("Delete")
				{
					X = Pos.Right(editBtn) + 2,
					Y = Pos.Top(editBtn)
				};
				deleteBtn.Clicked += OnProductDelete;
				this.Add(deleteBtn);
			}
		}

		private void OnProductDelete()
		{
			int index = MessageBox.Query("Delete activity", "Are you sure?", "No", "Yes");
			if (index == 1)
			{
				this.deleted = true;
				Application.RequestStop();
			}
		}

		private void OnProductEdit()
		{
			EditProductDialog dialog = new EditProductDialog();
			dialog.SetProduct(this.product);
			Application.Run(dialog);
			if (!dialog.canceled)
			{
				nameInput.ReadOnly = false;
				priceInput.ReadOnly = false;
				leftInput.ReadOnly = false;
				descriptionInput.ReadOnly = false;
				Product updatedProduct = dialog.GetProduct();
				this.updated = true;
				this.SetProduct(updatedProduct);
			}
			nameInput.ReadOnly = true;
			priceInput.ReadOnly = true;
			leftInput.ReadOnly = true;
			descriptionInput.ReadOnly = true;
		}

		public void SetProduct(Product product)
		{
			this.product = product;
			this.idInput.Text = product.id.ToString();
			this.nameInput.Text = product.name;
			this.priceInput.Text = product.price.ToString();
			this.leftInput.Text = product.left.ToString();
			this.descriptionInput.Text = product.description;

		}

		public Product GetProduct()
		{
			return this.product;
		}

		private void OnOpenDialogCanceled()
		{
			Application.RequestStop();
		}

	}
}
