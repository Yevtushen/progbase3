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
				var products = rep.GetExport(value);
				formatter.Serialize(fs, products);
			}
			
			ZipFile.CreateFromDirectory(sourceFolder, zipFile);
		}
	}
}
