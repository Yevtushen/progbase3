using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class ReportWindow : Window
	{
		private ProductsRepository productsRepository;


		public ReportWindow(ProductsRepository productsRepository)
		{
			this.productsRepository = productsRepository;

			Button createReportBtn = new Button("Create report")
			{
				X = Pos.Center(),
				Y = Pos.Center()
			};
			createReportBtn.Clicked += CreateReport;
			Add(createReportBtn);

			Button backBtn = new Button("Back")
			{
				X = Pos.Center(),
				Y = Pos.Center() + 2
			};
			backBtn.Clicked += CloseWindow;
			Add(backBtn);
		}

		private void CloseWindow()
		{
			Application.RequestStop();
		}

		private void CreateReport()
		{
			ReportCreator reportCreator = new ReportCreator(productsRepository);
			reportCreator.SaveReport();
			MessageBox.Query("Report creation", "Report created", "OK!");
		}
	}
}
