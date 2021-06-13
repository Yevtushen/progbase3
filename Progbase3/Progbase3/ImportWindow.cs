using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class ImportWindow : Window
	{
		private ProductsRepository productsRepository;
		private Label targetFolderLbl;
		private Label zipFileLbl;
		private Label xmlFilePathLbl;
		public bool closed;

		public ImportWindow(ProductsRepository productsRepository)
		{
			this.productsRepository = productsRepository;

			Button targetBtn = new Button(2, 4, "Choose target folder");
			targetFolderLbl = new Label("Not selected")
			{
				X = Pos.Right(targetBtn) + 2,
				Y = Pos.Top(targetBtn),
				Width = Dim.Fill(),
			};
			targetBtn.Clicked += SelectDirectory;
			this.Add(targetBtn, targetFolderLbl);

			Button zipBtn = new Button(2, 6, "Choose archive");
			zipFileLbl = new Label("Not selected")
			{
				X = Pos.Right(zipBtn) + 2,
				Y = Pos.Top(zipBtn),
				Width = Dim.Fill(),
			};
			zipBtn.Clicked += SelectZipFile;			
			this.Add(zipBtn, zipFileLbl);

			Button xmlBtn = new Button(2, 8, "Choose xml");
			xmlFilePathLbl = new Label("Not selected")
			{
				X = Pos.Right(xmlBtn) + 2,
				Y = Pos.Top(xmlBtn),
				Width = Dim.Fill(),
			};
			xmlBtn.Clicked += SelectXMLFile;
			this.Add(xmlBtn, xmlFilePathLbl);

			Button importBtn = new Button(2, 10, "Import");
			importBtn.Clicked += ImportData;
			this.Add(importBtn);

			Button backBtn = new Button(2, 2, "Back");
			backBtn.Clicked += GoBack;
			this.Add(backBtn);
		}

		private void GoBack()
		{
			closed = true;
		}

		private void ImportData()
		{
			Import import = new Import();
			import.ImportProduct(productsRepository, targetFolderLbl.Text.ToString(), zipFileLbl.Text.ToString(), xmlFilePathLbl.Text.ToString());

		}

		private void SelectZipFile()
		{
			OpenDialog dialog = new OpenDialog("Open ZIP file", "Open?");
			// dialog.DirectoryPath = ...

			Application.Run(dialog);

			if (!dialog.Canceled)
			{
				NStack.ustring filePath = dialog.FilePath;
				zipFileLbl.Text = filePath;
			}
			else
			{
				zipFileLbl.Text = "Not selected";
			}
		}

		private void SelectXMLFile()
		{
			OpenDialog dialog = new OpenDialog("Open XML file", "Open?");
			// dialog.DirectoryPath = ...

			Application.Run(dialog);

			if (!dialog.Canceled)
			{
				NStack.ustring filePath = dialog.FilePath;
				xmlFilePathLbl.Text = filePath;
			}
			else
			{
				xmlFilePathLbl.Text = "Not selected";
			}
		}

		private void SelectDirectory()
		{
			OpenDialog dialog = new OpenDialog("Open directory", "Open?");
			dialog.CanChooseDirectories = true;
			dialog.CanChooseFiles = false;

			Application.Run(dialog);

			if (!dialog.Canceled)
			{
				NStack.ustring filePath = dialog.FilePath;
				targetFolderLbl.Text = filePath;
			}
			else
			{
				targetFolderLbl.Text = "Not selected";
			}
		}
	}
}
