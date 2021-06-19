using System.Collections.Generic;
using ScottPlot;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LibraryClass
{
	public class ReportCreator
	{		
		private ProductsRepository productsRepository;

		public ReportCreator(ProductsRepository productsRepository)
		{
			this.productsRepository = productsRepository;
		}

		public void SaveReport()
		{
			var docxReportCreator = new DocxReportCreator();
			var xmlDocument = docxReportCreator.GetXmlTemplate();

			FindAndReplace(xmlDocument.Root);
			ReplaceImage(docxReportCreator.GetImageFileName());

			docxReportCreator.SaveDocx(xmlDocument);
			
		}

		private void ReplaceImage(string filePath)
		{
			PlotImage(productsRepository, filePath);
		}

		public void FindAndReplace(XElement node)
		{
			if (node.FirstNode != null && node.FirstNode.NodeType == XmlNodeType.Text)
			{
				switch (node.Value)
				{
					case "{{totalCount}}": node.Value = GetTotalCount(); break;
					case "{{avgPrice}}": node.Value = GetAvgPrice(); break;
					case "{{maxPriceProduct}}": node.Value = GetMaxPriceProduct(); break;
					case "{{minPriceProduct}}": node.Value = GetMinPriceProduct(); break;
					case "{{mostPopularProduct}}": node.Value = GetMostPopularProduct(); break;
					case "{{maxRevenueProduct}}": node.Value = GetMaxRevenueProduct(); break;
				}
			}

			foreach (var el in node.Elements())
			{
				FindAndReplace(el);
			}

		}

		private string GetMostPopularProduct()
		{
			var productsInOrders = productsRepository.GetProductsInOrders();
			var mostPopularProduct = productsInOrders.Values.SelectMany(p => p)
				.GroupBy(p => p)
				.OrderByDescending(grp => grp.Count())
				.FirstOrDefault()
				.Key;
			return mostPopularProduct.ToString();
		}

		private string GetMaxPriceProduct()
		{
			var products = productsRepository.GetProducts();
			var maxPriceProduct = products.OrderByDescending(p => p.price).FirstOrDefault();
			return maxPriceProduct?.ToString() ?? "";
		}

		private string GetMinPriceProduct()
		{
			var products = productsRepository.GetProducts();
			var minPriceProduct = products.OrderByDescending(p => p.price).FirstOrDefault();
			return minPriceProduct?.ToString() ?? "";
		}

		private string GetMaxRevenueProduct()
		{
			var productsInOrders = productsRepository.GetProductsInOrders();
			var mostPopularProduct = productsInOrders.Values.SelectMany(p => p)
				.GroupBy(p => p)
				.OrderByDescending(grp => grp.Count()*grp.Key.price)
				.FirstOrDefault()
				.Key;
			return mostPopularProduct.ToString();
		}

		private string GetAvgPrice()
		{
			var products = productsRepository.GetProducts();
			var avgPrice = products.Average(p => p.price);
			return avgPrice.ToString();
		}

		private string GetTotalCount()
		{
			var products = productsRepository.GetProducts();
			return products.Count().ToString();
		}

		public void PlotImage(ProductsRepository rep, string filePath)
		{
			List<Product> products = rep.GetProducts();

			var plt = new Plot(600, 400);

			double[] xs = Enumerable.Range(1, products.Count)
				.Select(i => (double)i).ToArray();
			double[] ys = products.Select(p => (double)p.price).ToArray();

			plt.PlotBar(xs, ys);

			plt.Title("Product-price relation");
			plt.YLabel("Price");
			string[] labels = products.Select(p => p.name.Replace(' ', '\n')).ToArray();
			plt.XTicks(xs, labels);
			plt.Axis(null, null, 0, null);
			plt.Grid(lineStyle: LineStyle.Dot);

			plt.SaveFig(filePath);
		}
	}
}
