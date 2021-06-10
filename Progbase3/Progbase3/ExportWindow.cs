using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class ExportWindow : Window
	{
		private ProductsRepository rep;
		private Label sourceFolderLbl;
		private Label zipFileLbl;
		private Label xmlFilePathLbl;
		private TextField value;

		public ExportWindow(ProductsRepository rep)
		{
			this.rep = rep;
			Button sourceBtn = new Button(2, 4, "Choose source folder");
			sourceFolderLbl = new Label("Not selected")
			{
				X = Pos.Right(sourceBtn) + 2,
				Y = Pos.Top(sourceBtn),
				Width = Dim.Fill(),
			};
			sourceBtn.Clicked += SelectDirectory;
			this.Add(sourceBtn, sourceFolderLbl);

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

			Label valuelbl = new Label(2, 10, "Enter value");
			value = new TextField("")
			{
				X = Pos.Right(valuelbl) + 2,
				Y = Pos.Top(valuelbl),
				Width = 20
			};
			this.Add(value, valuelbl);

			Button exportBtn = new Button(2, 10, "Import");
			exportBtn.Clicked += ExportData;
			this.Add(exportBtn);
		}

		private void ExportData()
		{
			Export export = new Export();

			export.ExportProducts(rep, value.Text.ToString(), xmlFilePathLbl.Text.ToString(), sourceFolderLbl.Text.ToString(), zipFileLbl.Text.ToString());
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
				sourceFolderLbl.Text = filePath;
			}
			else
			{
				sourceFolderLbl.Text = "Not selected";
			}
		}
	}
}
