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

			Title = "Your orders";

			MenuBar menu = new MenuBar(new MenuBarItem[]
				{ new MenuBarItem("_File", new MenuItem[]
				{ new MenuItem("_Exit", "Exit program", OnExit) }),
				new MenuBarItem("_Help", new MenuItem[]
				{ new MenuItem("_About", "About program", OnTellAbout) })});
			Add(menu);

			Button createBtn = new Button(2, 2, "Create new order");
			createBtn.Clicked += CreateOrder;
			Add(createBtn);

			allOrdersListView = new ListView(new List<Order>())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			allOrdersListView.OpenSelectedItem += OnOpenOrder;

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

			FrameView frameView = new FrameView("Orders")
			{
				X = 2,
				Y = 8,
				Width = Dim.Fill() - 4,
				Height = pageSize + 2
			};

			Button backBtn = new Button(2, 15, "Back");
			backBtn.Clicked += CloseWindow;
			Add(backBtn);

			frameView.Add(allOrdersListView);
			Add(frameView);
		}

		private void CloseWindow()
		{
			Application.RequestStop();
		}

		private void CreateOrder()
		{			
			ProductsWindow window = new ProductsWindow(customer, productsRepository, ordersRepository);
			window.SetRepository(productsRepository);
			Application.Run(window);
			ShowCurrentPage();
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
			int totalPages = ordersRepository.GetTotalPages(pageSize, customer.id);
			if (pageNumber >= totalPages)
			{
				return;
			}

			pageNumber += 1;
			ShowCurrentPage();
		}

		public void SetRepository(OrdersRepository ordersRepository)
		{
			this.ordersRepository = ordersRepository;
			ShowCurrentPage();
		}

		private void ShowCurrentPage()
		{
			pageLabel.Text = pageNumber.ToString();
			totalPagesLabel.Text = ordersRepository.GetTotalPages(pageSize, customer.id).ToString();
			allOrdersListView.SetSource(ordersRepository.GetPage(pageNumber, pageSize, customer.id));
		}

		private void OnOpenOrder(ListViewItemEventArgs args)
		{
			Order order = (Order)args.Value;
			OpenOrderDialog dialog = new OpenOrderDialog(order);
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
