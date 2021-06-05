using System;
using System.Collections.Generic;
using System.Text;
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
		private Product product;
		private OrdersRepository ordersRepository;
		private Order order;
		private Window window;
		private int pageSize = 5;
		private int pageNumber = 1;

		public UserInterface(SqliteConnection connection)
		{
			this.customersRepository = new CustomersRepository(connection);
			this.productsRepository = new ProductsRepository(connection);
			this.ordersRepository = new OrdersRepository(connection);

			this.Title = "Authentication";

			MenuBar menu = new MenuBar(new MenuBarItem[]
				{ new MenuBarItem("_File", new MenuItem[]
					{ new MenuItem("_Import", "Import", OnImport), new MenuItem("_Export", "Export", OnExport), new MenuItem("_Exit", "Exit program", OnExit) }),
				new MenuBarItem("_Help", new MenuItem[]
				{ new MenuItem("_About", "About program", OnTellAbout) })});
			this.Add(menu);

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
			this.Add(logInBtn, registerBtn);

			Rect frame = new Rect(0, 0, this.Frame.Width, this.Frame.Height);
			window = new Window(frame, $"Hello, {customer.name}!");
			this.Add(window);

			Button backBtn = new Button("Log out");
			backBtn.Clicked += CloseNewWin;
			this.Add(backBtn);

			Button productsBtn = new Button("Go to the store")
			{ 
				X = Pos.Center(),
				Y = Pos.Center() + 2
			};
			productsBtn.Clicked += ShowProducts;

			Button ordersBtn = new Button("See your orders")
			{
				X = Pos.Center(),
				Y = Pos.Center()
			};
			ordersBtn.Clicked += ShowOrders;

			this.Add(productsBtn, ordersBtn);
			if (customer.moderator == true)
			{
				Button usersBtn = new Button("Look at customers")
				{ 
					X = Pos.Center(),
					Y = Pos.Center() - 2
				};
				usersBtn.Clicked += ShowUsers;
				this.Add(usersBtn);
			}
		}

		private void ShowUsers()
		{
			throw new NotImplementedException();
		}

		private void ShowOrders()
		{
			throw new NotImplementedException();
		}

		private void ShowProducts()
		{
			throw new NotImplementedException();
		}

		private void CloseNewWin()
		{
			Toplevel top = Application.Top;
			top.Remove(this.window);
		}

		private void RegisterClicked()
		{
			RegistrationDialog dialog = new RegistrationDialog();
			Application.Run(dialog);
			Authentication au = new Authentication();
			customer = au.Register(dialog.GetName(), dialog.GetPassword(), dialog.GetAdress());
			if (customer == null)
			{
				MessageBox.ErrorQuery("Something is wrong!", "We were unable to registrate you. User with such data alreade exists. Change your data or log in", "OK");
				dialog.OnDialogCanceled();
			}
		}

		private void LogInClicked()
		{
			LogInDialog dialog = new LogInDialog();
			Application.Run(dialog);
			Authentication au = new Authentication();
			customer = au.LogIn(dialog.GetName(), dialog.GetPassword());
			if (customer == null)
			{
				MessageBox.ErrorQuery("Something is wrong!", "Wrong username and/or password. Try again or registrate", "OK");
				dialog.OnDialogCanceled();
			}
		}

		private void OnExport()
		{
			throw new NotImplementedException();
		}

		private void OnImport()
		{
			throw new NotImplementedException();
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
