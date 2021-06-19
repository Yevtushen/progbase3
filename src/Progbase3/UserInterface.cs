using Terminal.Gui;
using Microsoft.Data.Sqlite;
using LibraryClass;

namespace Progbase3
{
	class UserInterface : Window
	{
		private CustomersRepository customersRepository;
		private Customer customer;
		private ProductsRepository productsRepository;
		private OrdersRepository ordersRepository;
		private Toplevel top = Application.Top;
		private MenuBar menu;
		private Window win;

		public UserInterface(SqliteConnection connection)
		{
			customersRepository = new CustomersRepository(connection);
			productsRepository = new ProductsRepository(connection);
			ordersRepository = new OrdersRepository(connection);

			Title = "Authentication";

			 menu = new MenuBar(new MenuBarItem[]
				{ new MenuBarItem("_File", new MenuItem[]
					{ new MenuItem("_Import", "Import", OnImport), new MenuItem("_Export", "Export", OnExport), new MenuItem("_Exit", "Exit program", OnExit) }),
				new MenuBarItem("_Help", new MenuItem[]
				{ new MenuItem("_About", "About program", OnTellAbout) })});
			Add(menu);

			Button logInBtn = new Button("Log In")
			{ 
				X = Pos.Center(),
				Y = Pos.Center() - 2
			};
			logInBtn.Clicked += LogInClicked;

			Button registerBtn = new Button("Register")
			{
				X = Pos.Center(),
				Y = Pos.Center() + 2
			};
			registerBtn.Clicked += RegisterClicked;
			Add(logInBtn, registerBtn);
		}

		private void CreateReport()
		{
			ReportWindow win = new ReportWindow(productsRepository);
			Application.Run(win);
		}

		private void ShowUsers()
		{
			CustomersWindow win = new CustomersWindow(customer, customersRepository);
			win.SetRepository(customersRepository);			
			Application.Run(win);
		}

		private void ShowOrders()
		{
			OrdersWindow win = new OrdersWindow(customer, ordersRepository, productsRepository);
			win.SetRepository(ordersRepository);			
			Application.Run(win);
		}

		private void ShowProducts()
		{
			ProductsWindow win = new ProductsWindow(customer, productsRepository, ordersRepository);
			win.SetRepository(productsRepository);
			Application.Run(win);
		}

		private void CloseNewWin()
		{
			top.Remove(win);
		}

		private void MainWindow()
		{
			Rect frame = new Rect(0, 0, Frame.Width, Frame.Height);
			win = new Window(frame, $"Hello, {customer.name}!");
			top.Add(win);

			win.Add(menu);

			Button backBtn = new Button("Log out")
			{
				X = Pos.Center(),
				Y = Pos.Center() + 4
			};
			backBtn.Clicked += CloseNewWin;
			win.Add(backBtn);

			Button productsBtn = new Button("Go to the store")
			{
				X = Pos.Center(),
				Y = Pos.Center() - 2
			};
			productsBtn.Clicked += ShowProducts;
			win.Add(productsBtn);

			Button ordersBtn = new Button("See your orders")
			{
				X = Pos.Center(),
				Y = Pos.Center()
			};
			ordersBtn.Clicked += ShowOrders;
			win.Add(ordersBtn);

			Button usersBtn = new Button("Customer")
			{
				X = Pos.Center(),
				Y = Pos.Center() + 2
			};
			usersBtn.Clicked += ShowUsers;
			win.Add(usersBtn);

			if (customer.moderator == true)
			{
				Button reportBtn = new Button("Create report")
				{
					X = Pos.Center(),
					Y = Pos.Center() - 4
				};
				reportBtn.Clicked += CreateReport;
				win.Add(reportBtn);				
			}
		}

		private void RegisterClicked()
		{
			RegistrationDialog dialog = new RegistrationDialog();
			Application.Run(dialog);
			Authentication au = new Authentication();
			if (!dialog.canceled && dialog.GetName() != null && dialog.GetPassword() != null && dialog.GetAdress() != null)
			{
				customer = au.Register(customersRepository, dialog.GetName(), dialog.GetPassword(), dialog.GetAdress());
				if (customer == null)
				{
					MessageBox.ErrorQuery("Something is wrong!", "We were unable to registrate you. User with such data already exists. Change your data or log in", "OK");
				}

				else
				{
					MainWindow();
				}
			}
		}

		private void LogInClicked()
		{
			LogInDialog dialog = new LogInDialog();
			Application.Run(dialog);
			if (!dialog.canceled && dialog.GetName() != null && dialog.GetPassword() != null)
			{				
				Authentication au = new Authentication();
				customer = au.LogIn(customersRepository, dialog.GetName(), dialog.GetPassword());
				if (customer == null)
				{
					MessageBox.ErrorQuery("Something is wrong!", "Wrong username and/or password. Try again or registrate", "OK");
				}

				else
				{
					MainWindow();
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
