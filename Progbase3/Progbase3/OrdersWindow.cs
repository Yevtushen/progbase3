using System.Collections.Generic;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class OrdersWindow : Window
	{

		private OrdersRepository ordersRepository;
		private ProductsRepository productsRepository;
		private Customer customer;
		private ListView allOrdersListView;
		private Button prevPageBtn;
		private Button nextPageBtn;
		private Label pageLabel;
		private Label totalPagesLabel;
		private int pageSize = 5;
		private int pageNumber = 1;

		public OrdersWindow(Customer customer, OrdersRepository ordersRepository, ProductsRepository productsRepository)
		{
			this.customer = customer;
			this.ordersRepository = ordersRepository;
			this.productsRepository = productsRepository;

			this.Title = "Your orders";

			MenuBar menu = new MenuBar(new MenuBarItem[]
				{ new MenuBarItem("_File", new MenuItem[]
					{ new MenuItem("_Import", "Import", OnImport), new MenuItem("_Export", "Export", OnExport), new MenuItem("_Exit", "Exit program", OnExit) }),
				new MenuBarItem("_Help", new MenuItem[]
				{ new MenuItem("_About", "About program", OnTellAbout) })});
			this.Add(menu);

			Button createBtn = new Button(2, 2, "Create new order");
			createBtn.Clicked += CreateOrder;
			this.Add(createBtn);

			allOrdersListView = new ListView(new List<Order>())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			allOrdersListView.OpenSelectedItem += OnOpenOrder;

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

			FrameView frameView = new FrameView("Orders")
			{
				X = 2,
				Y = 8,
				Width = Dim.Fill() - 4,
				Height = pageSize + 2
			};

			Button backBtn = new Button(2, 15, "Back");
			backBtn.Clicked += CloseWin;
			this.Add(backBtn);

			frameView.Add(allOrdersListView);
			this.Add(frameView);
		}

		private void CloseWin()
		{
			Remove(this);
		}

		private void CreateOrder()
		{
			Toplevel top = new Toplevel();
			ProductsWindow window = new ProductsWindow(customer, productsRepository);
			top.Add(window);
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
			int totalPages = ordersRepository.GetTotalPages(pageSize, customer.id);
			if (pageNumber >= totalPages)
			{
				return;
			}

			this.pageNumber += 1;
			ShowCurrentPage();
		}

		public void SetRepository(OrdersRepository ordersRepository)
		{
			this.ordersRepository = ordersRepository;
			this.ShowCurrentPage();
		}

		private void ShowCurrentPage()
		{
			this.pageLabel.Text = pageNumber.ToString();
			this.totalPagesLabel.Text = ordersRepository.GetTotalPages(pageSize, customer.id).ToString();
			this.allOrdersListView.SetSource(ordersRepository.GetPage(pageNumber, pageSize, customer.id));
		}

		private void OnOpenOrder(ListViewItemEventArgs args)
		{
			Order order = (Order)args.Value;
			OpenOrderDialog dialog = new OpenOrderDialog(customer);
			dialog.SetOrder(order);
			Application.Run(dialog);
			if (dialog.deleted)
			{
				bool result = ordersRepository.Delete(order.id);
				if (result)
				{
					int pages = ordersRepository.GetTotalPages(pageSize, order.customer_id);
					if (pageNumber > pages && pageNumber > 1)
					{
						pageNumber -= 1;
						this.ShowCurrentPage();
					}

					allOrdersListView.SetSource(ordersRepository.GetPage(pageNumber, pageSize, order.customer_id));
				}
				else
				{
					MessageBox.ErrorQuery("Delete order", "Not able to delete the order", "OK");
				}
			}			
		}

		private void OnExport()
		{
			ExportWindow win = new ExportWindow(productsRepository);

			Application.Run(win);
		}

		private void OnImport()
		{
			ImportWindow win = new ImportWindow(productsRepository);

			Application.Run(win);
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
