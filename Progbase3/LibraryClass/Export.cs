using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.IO.Compression;

namespace LibraryClass
{
	class Export
	{
		private ProductsRepository rep;

		public void ExportProducts(string value, string xmlFilePathExp, string sourceFolder, string zipFile)
		{
			XmlSerializer formatter = new XmlSerializer(typeof(List<Product>));
			using (FileStream fs = new FileStream(xmlFilePathExp, FileMode.OpenOrCreate))
			{
				formatter.Serialize(fs, rep.GetExport(value));
			}

			ZipFile.CreateFromDirectory(sourceFolder, zipFile);
		}
	}
}
