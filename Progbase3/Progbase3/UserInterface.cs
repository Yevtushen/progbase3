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

		public UserInterface(SqliteConnection connection)
		{
			this.customersRepository = new CustomersRepository(connection);
			this.Title = "";

			MenuBar menu = new MenuBar(new MenuBarItem[]
				{ new MenuBarItem("_File", new MenuItem[]
					{ new MenuItem("_Import", "Import", OnImport), new MenuItem("_Export", "Export", OnExport), new MenuItem("_Exit", "Exit program", OnExit) }),
				new MenuBarItem("_Help", new MenuItem[]
				{ new MenuItem("_About", "About program", OnTellAbout) })});
			this.Add(menu);

			Button logInBtn = new Button(5, 2, "Log In");
			logInBtn.Clicked += LogInClicked;

			Button registerBtn = new Button(5, 4, "Register");
			registerBtn.Clicked += RegisterClicked;

			/*Button moderatorBtn = new Button("Moderator");
			moderatorBtn += moderatorBtn;*/
			this.Add(logInBtn, registerBtn);
		}

		private void RegisterClicked()
		{
			
		}

		private void LogInClicked()
		{
			throw new NotImplementedException();
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
			MessageBox.Query("About", "This is ", "OK");
		}
	}
}
