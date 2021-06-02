using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot;
using System.Linq;

namespace LibraryClass
{
	static class Graphic
	{
		public static void PLotImage()
		{
			var plt = new Plot(600, 800);
			//var products = 

			plt.Title("Distribution of prices for products");
			plt.YLabel("Price");
			plt.XLabel("Products");

			plt.SaveFig("Histogram.png");
		}
	}
}
