using BaByBoi.Domain;

namespace BaByBoi_Project.Common
{
    public class ProductSizeHelper
    {
        public static SelectedPurchaseDTO cutID(string selectedProduct)
        {
            var productInfo = selectedProduct.Split('-');
            var selected = new SelectedPurchaseDTO();
            selected.ProductId = int.Parse(productInfo[0]);
            selected.SizeId = int.Parse(productInfo[1]);
            return selected;
        }

    }
}
