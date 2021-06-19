using LibraryClass;

namespace Progbase3
{
	class EditProductDialog : CreateProductDialog
	{
		public EditProductDialog()
		{
			Title = "Edit product";
		}

		public void SetProduct(Product product)
		{
			nameInput.Text = product.name;
			priceInput.Text = product.price.ToString();
			leftInput.Text = product.left.ToString();
			descriptionInput.Text = product.description;
		}
	}
}
