using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Terminal.Gui;

namespace Progbase3
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}


		private static void DBProcessing()
		{
			string databaseString = "D:\\victo\\kpi\\progbase projects\\progbase3\\data\\newbase.db";
			SqliteConnection connection = new SqliteConnection($"Data Source={databaseString}");
		}

		private static void UserInterface()
		{
			Application.Init();

			Application.Run();
		}
		
	}
}
