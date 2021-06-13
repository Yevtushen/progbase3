using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace LibraryClass
{
	class DocxReportCreator
	{
		private string name;
		private string destination;

		public DocxReportCreator(string fileName)
		{
			destination = fileName.Substring(0, fileName.LastIndexOf("\\"));
			name = Path.GetFileNameWithoutExtension(fileName);
		}

		public XDocument GetXmlTemplate()
		{
			var folderName = Path.Combine(destination, name);

			var templateDir = new DirectoryInfo("D:\\private\\ReportTemplate");
			CopyDirectory(templateDir, folderName);

			var xmlDocument = ReadXml(folderName);
			return xmlDocument;
		}

		public void SaveDocx(XDocument document)
		{
			var folderName = Path.Combine(destination, name);
			document.Save(Path.Combine(folderName, "word\\document.xml"));
			ZipFile.CreateFromDirectory(folderName, $"{folderName}.docx");
			DeleteDirectory(folderName);
		}

		public string GetImageFileName()
		{
			return Path.Combine(destination, name, "media\\image1.png");
		}


		private void CopyDirectory(DirectoryInfo dir, string destination)
		{
			Directory.CreateDirectory(destination);
			var subDirectories = dir.GetDirectories();
			foreach (var subDir in subDirectories)
			{
				CopyDirectory(subDir, Path.Combine(destination, subDir.Name));
			}
			var files = dir.GetFiles();
			foreach (var file in files)
			{
				file.CopyTo(Path.Combine(destination, file.Name), true);
			}
		}

		private void DeleteDirectory(string path)
		{
			var dir = new DirectoryInfo(path);
			var files = dir.GetFiles();
			foreach(var file in files)
			{
				File.Delete(file.FullName);
			}
			var subDirectories = dir.GetDirectories();
			foreach (var subDir in subDirectories)
			{
				DeleteDirectory(subDir.FullName);
			}

			Directory.Delete(path, false);

		}

		private XDocument ReadXml(string filePath)
		{
			var xmlFilePath = Path.Combine(filePath, "word\\document.xml");
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.IgnoreWhitespace = true;

			using (StreamReader fileStream = File.OpenText(xmlFilePath))
			{
				XmlReader reader = XmlReader.Create(fileStream, settings);
				XDocument doc = XDocument.Load(reader);
				reader.Close();
				return doc;
			}
		}

	}
}
