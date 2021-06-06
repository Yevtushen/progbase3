﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace LibraryClass
{
	public class Import
	{

		public void ImportProduct(ProductsRepository rep, string targetFolder, string zipFile, string xmlFilePath)
		{
			ZipFile.ExtractToDirectory(zipFile, targetFolder);

			XmlSerializer formatter = new XmlSerializer(typeof(List<Product>));
			using (FileStream fs = new FileStream(xmlFilePath, FileMode.OpenOrCreate))
			{
				Product product = (Product)formatter.Deserialize(fs);
				rep.Insert(product);
			}
		}
	}
}
