﻿using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndPoint _productEndPoint;
        private readonly IConfigHelper _configHelper;

        private int _itemQuantity = 1;
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private ProductModel _selectedProduct;

        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;
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
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }

            return subTotal;
        }
        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal textRate = _configHelper.GetTaxRate() / 100;
            foreach (var item in Cart)
            {
                if (item.Product.IsTaxable)
                {
                    taxAmount += (item.Product.RetailPrice * item.QuantityInCart * textRate);
                }
            }

            return taxAmount;
        }
        public string Tax
        {
            get
            {
                //Replace with calculation
                return CalculateTax().ToString("C");
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
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
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
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);

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
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
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