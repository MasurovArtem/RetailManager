using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Model;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndPoint _productEndPoint;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndPoint _saleEndPoint;
        private  IMapper _mapper;

        private int _itemQuantity = 1;
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private ProductDisplayModel _selectedProductDisplayModel;

        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, 
            ISaleEndPoint saleEndPoint, IMapper mapper)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;
            _saleEndPoint = saleEndPoint;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
             base.OnViewLoaded(view);
             await LoadProducts();
        }
        private async Task LoadProducts()
        {
            List<ProductModel> productList = await _productEndPoint.GetAll();
            List<ProductDisplayModel> products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }
        public BindingList<ProductDisplayModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }
        public BindingList<CartItemDisplayModel> Cart
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


        public string SubTotal => CalculateSubTotal().ToString("C");

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
            decimal taxRate = _configHelper.GetTaxRate() / 100;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

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
                bool output = ItemQuantity > 0 && SelectedProductDisplayModel?.QuantityInStock >= ItemQuantity;
                // Make sure something is selected
                // make sure there is an item quantity

                return output;
            }
        }


        public ProductDisplayModel SelectedProductDisplayModel
        {
            get => _selectedProductDisplayModel;
            set
            {
                _selectedProductDisplayModel = value;
                 NotifyOfPropertyChange(() => SelectedProductDisplayModel);
                 NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        public void AddToCart()
        {
            CartItemDisplayModel existingItemDisplay = Cart.FirstOrDefault(x => x.Product.Equals(SelectedProductDisplayModel));

            if (existingItemDisplay != null)
            {
                existingItemDisplay.QuantityInCart += ItemQuantity;
                //// HACK = There should be a better way of refreshing the display
                //Cart.Remove(existingItemDisplay);
                //Cart.Add(existingItemDisplay);

                // We did it using an auto mapper


            }
            else
            {
                var item = new CartItemDisplayModel
                {
                    Product = SelectedProductDisplayModel,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
            SelectedProductDisplayModel.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);

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
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = Cart.Count > 0;

                return output;
            }
        }

        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel()
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndPoint.PostSale(sale);
        }
    }
}