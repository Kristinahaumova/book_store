using Book_store.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Book_store.Model
{
    public class CartItem : INotifyPropertyChanged
    {
        private OrderItemContext _item;
        public OrderItemContext Item
        {
            get => _item;
            set
            {
                if (_item != value)
                {
                    if (_item != null) _item.PropertyChanged -= Item_PropertyChanged;  
                    _item = value;
                    if (_item != null) _item.PropertyChanged += Item_PropertyChanged; 
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        private BookContext _book;
        public BookContext Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged();
            }
        }

        public int Total => Item?.Quantity * Item?.Price ?? 0;
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Item.Quantity))
            {
                OnPropertyChanged(nameof(Total));  
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
