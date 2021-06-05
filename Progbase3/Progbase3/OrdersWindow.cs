using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	public class OrdersWindow : Window
	{

		protected OrdersRepository rep;
		protected Customer customer;
		protected ListView allOrdersListView;
		private Button prevPageBtn;
		private Button nextPageBtn;
		private Label pageLabel;
		private Label totalPagesLabel;
		private int pageSize = 5;
		private int pageNumber = 1;

		public OrdersWindow(Customer customer)
		{
			this.customer = customer;

			Button createBtn = new Button(2, 2, "Create new order");
			createBtn.Clicked += CreateOrder;

			allOrdersListView = new ListView(new List<Order>())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			allOrdersListView.OpenSelectedItem += OnOpenOrder;

			prevPageBtn = new Button(2, 6, "Previous");
			prevPageBtn.Clicked += OnPrevPage;
			this.Add(prevPageBtn);

			pageLabel = new Label("?")
			{
				X = Pos.Right(prevPageBtn) + 2,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			this.Add(pageLabel);

			Label pagesSeparatorLabel = new Label("/")
			{
				X = Pos.Right(pageLabel) + 1,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			this.Add(pagesSeparatorLabel);
			totalPagesLabel = new Label("?")
			{
				X = Pos.Right(pagesSeparatorLabel) + 1,
				Y = Pos.Top(prevPageBtn),
				Width = 5
			};
			this.Add(totalPagesLabel);

			nextPageBtn = new Button("Next")
			{
				X = Pos.Right(totalPagesLabel) + 2,
				Y = Pos.Top(prevPageBtn),
			};
			nextPageBtn.Clicked += OnNextPage;
			this.Add(nextPageBtn);

			FrameView frameView = new FrameView("Activities")
			{
				X = 2,
				Y = 8,
				Width = Dim.Fill() - 4,
				Height = pageSize + 2
			};

			Button backBtn = new Button(2, 15, "Back");
			backBtn.Clicked += CloseWin;
			this.Add(backBtn);

			frameView.Add(allOrdersListView);
			this.Add(frameView);
		}

		private void CloseWin()
		{
			Remove(this);
		}

		private void CreateOrder()
		{
			ProductsWindow window = new ProductsWindow(customer);
		}

		private void OnPrevPage()
		{
			if (pageNumber == 1)
			{
				return;
			}

			this.pageNumber -= 1;
			ShowCurrentPage();
		}

		private void OnNextPage()
		{
			int totalPages = rep.GetTotalPages(pageSize, customer.id);
			if (pageNumber >= totalPages)
			{
				return;
			}

			this.pageNumber += 1;
			ShowCurrentPage();
		}

		private void ShowCurrentPage()
		{
			this.pageLabel.Text = pageNumber.ToString();
			this.totalPagesLabel.Text = rep.GetTotalPages(pageSize, customer.id).ToString();
			this.allOrdersListView.SetSource(rep.GetPage(pageNumber, pageSize, customer.id));
		}

		private void OnOpenOrder(ListViewItemEventArgs args)
		{
			OpenOrderDialog dialog = new OpenOrderDialog(customer);

		}
	}
}
