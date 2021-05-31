using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace LibraryClass
{
	class Export
	{
		
	}
}
/*public static void Export(int n, string filePath, Document document, XmlSerializer serializer)
{
	Document doc = new Document();
	doc.Courses = document.Courses.OrderByDescending(c => c.Units).Take(n).ToList();
	using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
	{
		serializer.Serialize(fs, doc);
	}

}*/

/*XmlSerializer formatter = new XmlSerializer(typeof(List<VideoGame>));
using (FileStream fs = new FileStream("videogames.xml", FileMode.OpenOrCreate))
{
	formatter.Serialize(fs, rep.GetListGame());
}*/