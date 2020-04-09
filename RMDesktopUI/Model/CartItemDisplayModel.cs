using System.ComponentModel;
using System.Runtime.CompilerServices;
using RMDesktopUI.Annotations;

namespace RMDesktopUI.Model
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        public ProductDisplayModel Product { get; set; }
        public string DisplayText => $@"{Product.ProductName} ({QuantityInCart})";

        private int _quantityInCart;
        public int QuantityInCart
        {
            get => _quantityInCart;
            set
            {
                _quantityInCart = value;
                CallPropertyChanged(nameof(QuantityInCart));
                CallPropertyChanged(nameof(DisplayText));
            }
        }
        public void CallPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    } 
}