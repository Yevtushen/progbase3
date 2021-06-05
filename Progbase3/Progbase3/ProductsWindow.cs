using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class ProductsWindow : Window
	{

		protected ProductsRepository rep;
		private Customer c;
		protected Order order;
		private TextField searchField;
		private string filterValue = "";
		protected ListView allProductsListView;
		private Button prevPageBtn;
		private Button nextPageBtn;
		private Label pageLabel;
		private Label totalPagesLabel;
		private int pageSize = 5;
		private int pageNumber = 1;

		public ProductsWindow(Customer c)
		{
			this.c = c;

			searchField = new TextField(2, 1, 20, "");
			searchField.KeyPress += OnSearchPress;
			this.Add(searchField);

			allProductsListView = new ListView(new List<Product>())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			allProductsListView.OpenSelectedItem += OnOpenProduct;

			prevPageBtn = new Button(2, 6, "Previous");
			prevPageBtn.Clicked += OnPrevPage;
			this.Add(prevPageBtn);

			pageLabel = new Label("?")
			{
				X = Pos.Right(prevPageBtn) + 2,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			this.Add(pageLabel);

			Label pagesSeparatorLabel = new Label("/")
			{
				X = Pos.Right(pageLabel) + 1,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			this.Add(pagesSeparatorLabel);
			totalPagesLabel = new Label("?")
			{
				X = Pos.Right(pagesSeparatorLabel) + 1,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			this.Add(totalPagesLabel);

			nextPageBtn = new Button("Next")
			{
				X = Pos.Right(totalPagesLabel) + 2,
				Y = Pos.Top(prevPageBtn),
			};
			nextPageBtn.Clicked += OnNextPage;
			this.Add(nextPageBtn);

			if (c.moderator)
			{
				Button createNewBtn = new Button(2, 4, "Add new");
				createNewBtn.Clicked += OnCreateButtonClicked;
				this.Add(createNewBtn);
			}

			FrameView frameView = new FrameView("Products")
			{
				X = 2,
				Y = 8,
				Width = Dim.Fill() - 4,
				Height = pageSize + 2
			};

			Button submitOrderBtn = new Button(2, 2, "Submit your order");
			if (order.products == null)
			{
				submitOrderBtn.Visible = false;
			}
			submitOrderBtn.Clicked += OnSubmitOrder;

			Button backBtn = new Button(2, 15, "Back");
			backBtn.Clicked += CloseWin;
			this.Add(backBtn);

			frameView.Add(allProductsListView);
			this.Add(frameView);
		}

		private void CloseWin()
		{
			Remove(this);
		}

		private void OnSearchPress(KeyEventEventArgs args)
		{
			if (args.KeyEvent.Key == Key.Enter)
			{
				filterValue = searchField.Text.ToString();
				ShowCurrentPage();
			}
		}

		private void OnSubmitOrder()
		{
			Remove(this);
		}

		private void OnCreateButtonClicked()
		{
			CreateProductDialog dialog = new CreateProductDialog();
			Application.Run(dialog);
			if (!dialog.canceled)
			{
				Product p = dialog.GetProduct();
				long activid = rep.Insert(p);
				p.id = activid;
				allProductsListView.SetSource(rep.GetSearchPage(filterValue, pageNumber, pageSize));
			}
		}

		private void OnPrevPage()
		{
			if (pageNumber == 1)
			{
				return;
			}

			this.pageNumber -= 1;
			ShowCurrentPage();
		}

		private void OnNextPage()
		{
			int totalPages = rep.GetSearchPagesCount(pageSize, filterValue);
			if (pageNumber >= totalPages)
			{
				return;
			}

			this.pageNumber += 1;
			ShowCurrentPage();
		}

		private void ShowCurrentPage()
		{
			this.pageLabel.Text = pageNumber.ToString();
			this.totalPagesLabel.Text = rep.GetSearchPagesCount(pageSize, filterValue).ToString();
			this.allProductsListView.SetSource(rep.GetSearchPage(filterValue, pageNumber, pageSize));
		}

		private void OnOpenProduct(ListViewItemEventArgs args)
		{
			Product p = (Product)args.Value;
			OpenProductDialog dialog = new OpenProductDialog(c, order);
			dialog.SetProduct(p);
			Application.Run(dialog);
			if (dialog.inOrder.Checked)
			{
				order.products.Add(p);
			}
			if (dialog.deleted)
			{
				bool result = rep.Delete(c.id);
				if (result)
				{
					int pages = rep.GetTotalPages(pageSize);
					if (pageNumber > pages && pageNumber > 1)
					{
						pageNumber -= 1;
						this.ShowCurrentPage();
					}

					allProductsListView.SetSource(rep.GetPage(pageNumber, pageSize));
				}
				else
				{
					MessageBox.ErrorQuery("Delete product", "Not able to delete the product", "OK");
				}
			}
			else if (dialog.updated)
			{
				bool result = rep.Update(c.id, dialog.GetProduct());
				if (result)
				{
					allProductsListView.SetSource(rep.GetPage(pageNumber, pageSize));
				}
				else
				{
					MessageBox.ErrorQuery("Edit product", "Not able to edit the product", "OK");
				}
			}
		}
	}
}
