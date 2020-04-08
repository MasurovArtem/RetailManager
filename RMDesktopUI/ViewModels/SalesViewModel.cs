using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndPoint _productEndPoint;
        private int _itemQuantity = 1;
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private ProductModel _selectedProduct;

        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndPoint = productEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
             base.OnViewLoaded(view);
             await LoadProducts();
        }
        private async Task LoadProducts()
        {
            var productList = await _productEndPoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }
        public BindingList<ProductModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }
        public BindingList<CartItemModel> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }
        public int ItemQuantity
        {
            get => _itemQuantity;
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        public string SubTotal
        {
            get
            {
                //Replace with calculation
                decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }
                return subTotal.ToString("C");
            }
        }
        public string Total
        {
            get
            {
                //Replace with calculation
                return $@"$0.00";
            }
        }

        public string Tax
        {
            get
            {
                //Replace with calculation
                return $@"$0.00";
            }
        }
        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                // Make sure something is selected
                // make sure there is an item quantity
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }


        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                 NotifyOfPropertyChange(() => SelectedProduct);
                 NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product.Equals(SelectedProduct));

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                //// HACK = There should be a better way of refreshing the display
                //Cart.Remove(existingItem);
                //Cart.Add(existingItem);
            }
            else
            {
                var item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => existingItem.DisplayText);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                // Make sure something is selected
                // make sure there is an item quantity

                return output;
            }
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                
                // make sure there is something in the cart

                return output;
            }
        }

        public void CheckOut()
        {

        }
    }
}