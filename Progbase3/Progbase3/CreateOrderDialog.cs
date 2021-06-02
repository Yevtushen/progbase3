using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using LibraryClass;

namespace Progbase3
{
	class CreateOrderDialog : Dialog
	{

		public bool canceled;
		protected OrdersRepository rep;
		protected Customer customer;
		protected TextView productsInOrder;

		public CreateOrderDialog(Customer customer)
		{
			this.customer = customer;
			this.Title = "Create order";

			Button okBtn = new Button("OK");
			Button canselBtn = new Button("Cancel");
			canselBtn.Clicked += OnCreatDialogCanceled;
			okBtn.Clicked += OnCreatDialogSubmitted;
			this.AddButton(canselBtn);
			this.AddButton(okBtn);

			int rightColumnX = 20;

			
		}

		public Order GetOrder()
		{
			return new Order()
			{
				customer_id = customer.id,
				products = rep.GetProductsInOrder(customer.id)
			};
		}



		private void OnCreatDialogSubmitted()
		{
			this.canceled = false;
			Application.RequestStop();
		}

		private void OnCreatDialogCanceled()
		{
			this.canceled = true;
			Application.RequestStop();
		}
	}
}
