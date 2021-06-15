using System.Collections.Generic;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class ProductsWindow : Window
	{

		private ProductsRepository productsRepository;
		private OrdersRepository ordersRepository;
		private Customer customer;
		private Order order;
		private TextField searchField;
		private string filterValue = "";
		protected ListView allProductsListView;
		private Button prevPageBtn;
		private Button nextPageBtn;
		private Label pageLabel;
		private Label totalPagesLabel;
		private int pageSize = 5;
		private int pageNumber = 1;
		private Button submitOrderBtn;

		public ProductsWindow(Customer customer, ProductsRepository productsRepository, OrdersRepository ordersRepository)
		{
			this.customer = customer;
			this.productsRepository = productsRepository;
			this.ordersRepository = ordersRepository;
			order = new Order()
			{
				customer_id = customer.id
			};

			Title = "Store";

			MenuBar menu = new MenuBar(new MenuBarItem[]
				{ new MenuBarItem("_File", new MenuItem[]
				{ new MenuItem("_Exit", "Exit program", OnExit) }),
				new MenuBarItem("_Help", new MenuItem[]
				{ new MenuItem("_About", "About program", OnTellAbout) })});
			Add(menu);

			searchField = new TextField(2, 1, 20, "");
			searchField.KeyPress += OnSearchPress;
			Add(searchField);

			allProductsListView = new ListView(new List<Product>())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			allProductsListView.OpenSelectedItem += OnOpenProduct;

			prevPageBtn = new Button(2, 6, "Previous");
			prevPageBtn.Clicked += OnPrevPage;
			Add(prevPageBtn);

			pageLabel = new Label("?")
			{
				X = Pos.Right(prevPageBtn) + 2,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			Add(pageLabel);

			Label pagesSeparatorLabel = new Label("/")
			{
				X = Pos.Right(pageLabel) + 1,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			Add(pagesSeparatorLabel);

			totalPagesLabel = new Label("?")
			{
				X = Pos.Right(pagesSeparatorLabel) + 1,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			Add(totalPagesLabel);

			nextPageBtn = new Button("Next")
			{
				X = Pos.Right(totalPagesLabel) + 2,
				Y = Pos.Top(prevPageBtn),
			};
			nextPageBtn.Clicked += OnNextPage;
			Add(nextPageBtn);

			if (customer.moderator)
			{
				Button createNewBtn = new Button(2, 4, "Add new");
				createNewBtn.Clicked += OnCreateButtonClicked;
				Add(createNewBtn);
			}

			FrameView frameView = new FrameView("Products")
			{
				X = 2,
				Y = 8,
				Width = Dim.Fill() - 4,
				Height = pageSize + 2
			};

			submitOrderBtn = new Button(2, 3, "Submit your order")
			{
				Visible = false
			};
			
			submitOrderBtn.Clicked += OnSubmitOrder;
			Add(submitOrderBtn);

			Button backBtn = new Button(2, 15, "Back");
			backBtn.Clicked += CloseWindow;
			Add(backBtn);

			frameView.Add(allProductsListView);
			Add(frameView);
		}

		internal void SetRepository(ProductsRepository productsRepository)
		{
			this.productsRepository = productsRepository;
			ShowCurrentPage();
		}

		private void CloseWindow()
		{			
			Application.RequestStop();
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
			foreach (var p in order.products)
			{
				p.left--;
			}
			ordersRepository.Insert(order);
			Application.RequestStop();
		}

		private void OnCreateButtonClicked()
		{
			CreateProductDialog dialog = new CreateProductDialog();
			Application.Run(dialog);
			if (!dialog.canceled)
			{
				Product p = dialog.GetProduct();
				long productId = productsRepository.Insert(p);
				p.id = productId;
				allProductsListView.SetSource(productsRepository.GetSearchPage(filterValue, pageNumber, pageSize));
			}
		}

		private void OnPrevPage()
		{
			if (pageNumber == 1)
			{
				return;
			}

			pageNumber -= 1;
			ShowCurrentPage();
		}

		private void OnNextPage()
		{
			int totalPages = productsRepository.GetSearchPagesCount(pageSize, filterValue);
			if (pageNumber >= totalPages)
			{
				return;
			}

			pageNumber += 1;
			ShowCurrentPage();
		}

		private void ShowCurrentPage()
		{
			pageLabel.Text = pageNumber.ToString();
			totalPagesLabel.Text = productsRepository.GetSearchPagesCount(pageSize, filterValue).ToString();
			allProductsListView.SetSource(productsRepository.GetSearchPage(filterValue, pageNumber, pageSize));
		}

		private void OnOpenProduct(ListViewItemEventArgs args)
		{
			Product product = (Product)args.Value;
			OpenProductDialog dialog = new OpenProductDialog(product, customer, order);
			dialog.SetProduct(product);
			Application.Run(dialog);
			
			if (dialog.inOrder.Checked)
			{
				submitOrderBtn.Visible = true;
				order.products.Add(product);
			}

			if (dialog.deleted)
			{
				bool result = productsRepository.Delete(customer.id);
				if (result)
				{
					int pages = productsRepository.GetTotalPages(pageSize);
					if (pageNumber > pages && pageNumber > 1)
					{
						pageNumber -= 1;
						ShowCurrentPage();
					}

					allProductsListView.SetSource(productsRepository.GetPage(pageNumber, pageSize));
				}
				else
				{
					MessageBox.ErrorQuery("Delete product", "Not able to delete the product", "OK");
				}
			}

			else if (dialog.updated)
			{
				bool result = productsRepository.Update(customer.id, dialog.GetProduct());
				if (result)
				{
					allProductsListView.SetSource(productsRepository.GetPage(pageNumber, pageSize));
				}
				else
				{
					MessageBox.ErrorQuery("Edit product", "Not able to edit the product", "OK");
				}
			}
		}
		
		private void OnExit()
		{
			Application.RequestStop();
		}

		private void OnTellAbout()
		{
			MessageBox.Query("About", "This is program presentation of an e-store done by KP-03 student, Yevtushenko Victoria, as a course project", "OK");
		}
	}
}
