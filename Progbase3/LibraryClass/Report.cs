using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace LibraryClass
{
	class Report
	{
		public void PLotImage(ProductsRepository rep)
		{
			List<Product> products = rep.GetProducts();

			var plt = new Plot(600, 400);

			double[] xs = Enumerable.Range(1, products.Count)
				.Select(i => (double)i).ToArray();
			double[] ys = products.Select(p => (double)p.price).ToArray();

			plt.PlotBar(xs, ys);
			//	plt.PlotScatter(xs, ys, markerSize: 0, lineWidth: 2, color: Color.Black);
			plt.Title("Product-price relation");
			plt.YLabel("Price");
			string[] labels = products.Select(p => p.name.Replace(' ', '\n')).ToArray();
			plt.XTicks(xs, labels);
			plt.Axis(null, null, 0, null);
			plt.Grid(lineStyle: LineStyle.Dot);

			plt.SaveFig("Histogram.png");
		}
	}
}
