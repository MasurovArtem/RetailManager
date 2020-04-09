using System.ComponentModel;
using System.Runtime.CompilerServices;
using RMDesktopUI.Annotations;

namespace RMDesktopUI.Model
{
    public class ProductDisplayModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }

        private int _quantityInStock;
        public int QuantityInStock
        {
            get => _quantityInStock;
            set
            {
                _quantityInStock = value;
                CallPropertyChanged(nameof(QuantityInStock));
            }
        }

        public bool IsTaxable { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void CallPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}