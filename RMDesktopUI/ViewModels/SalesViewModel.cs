using System.ComponentModel;
using Caliburn.Micro;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private string _itemQuantity;
        private BindingList<string> _products;
        private BindingList<string> _cart;

        public BindingList<string> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }
        public BindingList<string> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }
        public string ItemQuantity
        {
            get => _itemQuantity;
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }


        public string SubTotal
        {
            get
            {
                //Replace with calculation
                return $@"$0.00";
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
        private bool CanAddToCart
        {
            get
            {
                bool output = false;
                // Make sure something is selected
                // make sure there is an item quantity

                return output;
            }
        }

        public void AddToCart()
        {

        }

        private bool CanRemoveFromCart
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

        }

        private bool CanCheckOut
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