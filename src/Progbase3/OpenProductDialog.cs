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
		protected TextField descriptionInput;
		public CheckBox inOrder;
		private Order order;

		public OpenProductDialog(Product product, Customer customer, Order order)
		{
			Title = "Open Product";

			this.product = product;
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
				Width = 40,
				ReadOnly = true
			};
			Add(leftLbl, leftInput);

			Label descriptionLbl = new Label(2, 10, "Description:");
			descriptionInput = new TextField()
			{
				X = rightColumnX,
				Y = Pos.Top(descriptionLbl),
				Width = 80,
				Text = "",
				ReadOnly = true
			};
			Add(descriptionLbl, descriptionInput);

			inOrder = new CheckBox(2, 12, "Put in order?") { Checked = false };
			Add(inOrder);

			if (product.left <= 0)
			{
				inOrder.Visible = false;
			}

			if (customer.moderator)
			{
				Button editBtn = new Button(2, 16, "Update");
				editBtn.Clicked += OnProductEdit;
				Add(editBtn);

				Button deleteBtn = new Button("Delete")
				{
					X = Pos.Right(editBtn) + 2,
					Y = Pos.Top(editBtn)
				};
				deleteBtn.Clicked += OnProductDelete;
				Add(deleteBtn);
			}
		}

		private void OnProductDelete()
		{
			int index = MessageBox.Query("Delete activity", "Are you sure?", "No", "Yes");
			if (index == 1)
			{
				deleted = true;
				Application.RequestStop();
			}
		}

		private void OnProductEdit()
		{
			EditProductDialog dialog = new EditProductDialog();
			try
			{
				dialog.SetProduct(product);
				Application.Run(dialog);
				if (!dialog.canceled)
				{
					nameInput.ReadOnly = false;
					priceInput.ReadOnly = false;
					leftInput.ReadOnly = false;
					descriptionInput.ReadOnly = false;
					Product updatedProduct = dialog.GetProduct();
					updated = true;
					SetProduct(updatedProduct);
					idInput.Text = product.id.ToString();
				}
			}
			catch (Exception ex)
			{
				MessageBox.ErrorQuery("Updating product", ex.Message, "OK");
			}
			nameInput.ReadOnly = true;
			priceInput.ReadOnly = true;
			leftInput.ReadOnly = true;
			descriptionInput.ReadOnly = true;
		}

		public void SetProduct(Product product)
		{
			this.product = product;
			idInput.Text = product.id.ToString();
			nameInput.Text = product.name;
			priceInput.Text = product.price.ToString();
			leftInput.Text = product.left.ToString();
			descriptionInput.Text = product.description;

			if (inOrder.Checked == true)
			{
				product.orders.Add(order);
			}
		}

		public Product GetProduct()
		{			
			return product;
		}

		private void OnOpenDialogCanceled()
		{
			Application.RequestStop();
		}
	}
}
