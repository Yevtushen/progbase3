using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace LibraryClass
{
	class Import
	{
		private ProductsRepository rep;

		public void ImportProduct(string targetFolder, string zipFile, string xmlFilePathImp, bool add)
		{
			ZipFile.ExtractToDirectory(zipFile, targetFolder);

			XmlSerializer formatter = new XmlSerializer(typeof(List<Product>));
			using (FileStream fs = new FileStream(xmlFilePathImp, FileMode.OpenOrCreate))
			{
				Product product = (Product)formatter.Deserialize(fs);
				if (add)
				{
					rep.Insert(product);
				}
			}

		}
	}
}
