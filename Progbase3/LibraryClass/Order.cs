﻿using System.Collections.Generic;

namespace Progbase3
{
	public class Order
	{
		public int id;
		//public int custom;
		public Customer customer;
		public List<Product> product;

		public Order()
		{
			id = 0;
			/*
			customer = "";
			custom = 0;
			adress = "";*/
			customer = new Customer();
			product = new List<Product>();
		}

		public Order(int id, string adress, Customer customer, List<Product> product)
		{
			this.id = id;
			/*this.customer = customer;
			this.custom = custom;
			this.adress = adress;*/
			this.customer = customer;
			this.product = product;
		}

		public override string ToString()
		{
			return ""; //$"{customer}, your order #{id} {custom} will be delivered to {adress}";
		}
	}
}
