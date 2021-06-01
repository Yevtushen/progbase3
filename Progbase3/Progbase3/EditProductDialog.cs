using LibraryClass;

namespace Progbase3
{
	class EditProductDialog : CreateProductDialog
	{
		public EditProductDialog()
		{
			this.Title = "Edit product";
		}

		public void SetProduct(Product product)
		{
			this.nameInput.Text = product.name;
			this.priceInput.Text = product.price.ToString();
			this.leftInput.Text = product.left.ToString();
			this.descriptionInput.Text = product.description;
		}
	}
}
