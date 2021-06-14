using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.IO.Compression;

namespace LibraryClass
{
	public class Export
	{
		public void ExportProducts(ProductsRepository rep, string value, string xmlFilePath, string sourceFolder, string zipFile)
		{
			XmlSerializer formatter = new XmlSerializer(typeof(List<Product>));
			using (FileStream fs = new FileStream(xmlFilePath, FileMode.OpenOrCreate))
			{
				formatter.Serialize(fs, rep.GetExport(value));
			}

			ZipFile.CreateFromDirectory(sourceFolder, zipFile);
		}
	}
}
