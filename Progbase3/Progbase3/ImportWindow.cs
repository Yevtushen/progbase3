using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	class ImportWindow : Window
	{
		private ProductsRepository rep;
		private Label targetFolderLbl;
		private Label zipFileLbl;
		private Label xmlFilePathLbl;

		ImportWindow(ProductsRepository rep)
		{
			this.rep = rep;

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
			zipBtn.Clicked += SelectFile;			
			this.Add(zipBtn, zipFileLbl);

			Button xmlBtn = new Button(2, 8, "Choose xml");
			xmlFilePathLbl = new Label("Not selected")
			{
				X = Pos.Right(xmlBtn) + 2,
				Y = Pos.Top(xmlBtn),
				Width = Dim.Fill(),
			};
			xmlBtn.Clicked += SelectFile;
			this.Add(xmlBtn, xmlFilePathLbl);

			Button importBtn = new Button(2, 10, "Import");
			importBtn.Clicked += ImportData;
			this.Add(importBtn);
		}

		private void ImportData()
		{
			Import import = new Import();
			import.ImportProduct(rep, targetFolderLbl.Text.ToString(), zipFileLbl.Text.ToString(), xmlFilePathLbl.Text.ToString());

		}

		private void SelectFile(Label fileLbl)
		{
			OpenDialog dialog = new OpenDialog("Open ZIP file", "Open?");
			// dialog.DirectoryPath = ...

			Application.Run(dialog);

			if (!dialog.Canceled)
			{
				NStack.ustring filePath = dialog.FilePath;
				fileLbl.Text = filePath;
			}
			else
			{
				fileLbl.Text = "Not selected";
			}
		}

		private void SelectDirectory(Label dirLbl)
		{
			OpenDialog dialog = new OpenDialog("Open directory", "Open?");
			dialog.CanChooseDirectories = true;
			dialog.CanChooseFiles = false;

			Application.Run(dialog);

			if (!dialog.Canceled)
			{
				NStack.ustring filePath = dialog.FilePath;
				dirLbl.Text = filePath;
			}
			else
			{
				dirLbl.Text = "Not selected";
			}
		}
	}
}
