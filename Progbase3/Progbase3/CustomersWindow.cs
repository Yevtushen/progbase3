using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class CustomersWindow : Window
	{
		private Window window;
		protected CustomersRepository rep;
		private Customer customer;
		private TextField searchField;
		private string filterValue = "";
		protected ListView allCustomersListView;
		private Button prevPageBtn;
		private Button nextPageBtn;
		private Label pageLabel;
		private Label totalPagesLabel;
		private int pageSize = 5;
		private int pageNumber = 1;

		public CustomersWindow(Customer customer)
		{
			this.customer = customer;

			Button yourselfBtn = new Button(2, 4, "Your info");
			yourselfBtn.Clicked += OnYouOpen;			

			if (customer.moderator)
			{
				searchField = new TextField(2, 1, 20, "");
				searchField.KeyPress += OnSearchPress;
				this.Add(searchField);

				allCustomersListView = new ListView(new List<Customer>())
				{
					Width = Dim.Fill(),
					Height = Dim.Fill()
				};

				allCustomersListView.OpenSelectedItem += OnOpenCustomer;

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

				FrameView frameView = new FrameView("Customers")
				{
					X = 2,
					Y = 8,
					Width = Dim.Fill() - 4,
					Height = pageSize + 2
				};

				Button backBtn = new Button(2, 15, "Back");
				backBtn.Clicked += CloseWin;
				this.Add(backBtn);

				frameView.Add(allCustomersListView);
				this.Add(frameView);
			}
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

		private void OnYouOpen()
		{
			OpenCustomerDialog dialog = new OpenCustomerDialog(customer);
			dialog.SetCustomer(customer);
			Application.Run(dialog);
			if (dialog.deleted)
			{
				bool result = rep.Delete(customer.id);
				if (result)
				{
					this.Remove(this.window);
				}
				else
				{
					MessageBox.ErrorQuery("Delete user", "Not able to delete the user", "OK");
				}
			}
			else if (dialog.updated)
			{
				bool result = rep.Update(customer.id, dialog.GetCustomer());
				if (!result)
				{
					MessageBox.ErrorQuery("Edit customer", "Not able to edit the customer", "OK");
				}
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
			int totalPages = rep.GetTotalPages(pageSize);
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
			this.allCustomersListView.SetSource(rep.GetSearchPage(filterValue, pageNumber, pageSize));
		}

		private void OnOpenCustomer(ListViewItemEventArgs args)
		{
			Customer c = (Customer)args.Value;
			OpenCustomerDialog dialog = new OpenCustomerDialog(c);
			dialog.SetCustomer(c);
			Application.Run(dialog);
			if (dialog.deleted)
			{
				bool result = rep.Delete(c.id);
				if (result)
				{
					int pages = rep.GetSearchPagesCount(pageSize, filterValue);
					if (pageNumber > pages && pageNumber > 1)
					{
						pageNumber -= 1;
						this.ShowCurrentPage();
					}

					allCustomersListView.SetSource(rep.GetPage(pageNumber, pageSize));
				}
				else
				{
					MessageBox.ErrorQuery("Delete user", "Not able to delete the user", "OK");
				}
			}
			else if (dialog.updated)
			{
				bool result = rep.Update(c.id, dialog.GetCustomer());
				if (result)
				{
					allCustomersListView.SetSource(rep.GetPage(pageNumber, pageSize));
				}
				else
				{
					MessageBox.ErrorQuery("Edit customer", "Not able to edit the customer", "OK");
				}
			}
		}
	}
}
