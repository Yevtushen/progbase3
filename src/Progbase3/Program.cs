using Microsoft.Data.Sqlite;
using Terminal.Gui;

namespace Progbase3
{
	class Program
	{
		static void Main(string[] args)
		{
			string databaseString = "../../../../../data/base.db";
			SqliteConnection connection = new SqliteConnection($"Data Source={databaseString}");
			Application.Init();
			Toplevel top = Application.Top;
			UserInterface win = new UserInterface(connection);
			top.Add(win);
			Application.Run();
		}		
	}
}
