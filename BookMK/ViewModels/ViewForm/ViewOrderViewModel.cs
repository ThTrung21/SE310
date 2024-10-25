using BookMK.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewOrderViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ViewCustomerViewModel));
        private Borrow _currentorder = new Borrow();
        public Borrow CurrentOrder
        {
            get => _currentorder;
            set { _currentorder = value; OnPropertyChanged(nameof(CurrentOrder)); }
        }
        private ObservableCollection<BookCopy> _orderitemlist;
        public ObservableCollection<BookCopy> OrderItemList
        {
            get { return _orderitemlist; }
            set { _orderitemlist = value; OnPropertyChanged(nameof(OrderItemList)); }
        }
        public ViewOrderViewModel(Borrow i)
        {
            _logger.Information("ViewAuthorViewModel constructor with Borrow {a} parameter called.", i.ID);
            CurrentOrder = i;

        }
    }
}
