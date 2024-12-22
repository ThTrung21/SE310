using BookMK.Commands.UpdateCommand;
using BookMK.Models;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewCustomerViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ViewCustomerViewModel));
        private Customer _currentcustomer = new Customer();
        public Customer CurrentCustomer
        {
            get => _currentcustomer;
            set { _currentcustomer = value; OnPropertyChanged(nameof(CurrentCustomer)); }
        }
        public ObservableCollection<Borrow> _borrowList;
        public ObservableCollection<Borrow> BorrowList
        {
            get => _borrowList;
            set { _borrowList = value; OnPropertyChanged(nameof(BorrowList)); }
        }
        private string _fullname;
        public string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        private string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        //private string _purchasepoint;
        //public string PurchasePoint
        //{
        //    get
        //    {
        //        return _purchasepoint;
        //    }
        //    set
        //    {
        //        _purchasepoint= value;
        //        OnPropertyChanged(nameof(PurchasePoint));
        //    }
        //}
        public ICommand UpdateCustomer { get; set; }

        public ViewCustomerViewModel()
        {
            _logger.Information("ViewCustomerViewModel constructor called.");

        }
        public ViewCustomerViewModel(Customer c)
        {

            _logger.Information("ViewAuthorViewModel constructor with Customer {a} parameter called.",c.ID);
            this.CurrentCustomer = c;



            DataProvider<Borrow> db = new DataProvider<Borrow>(Borrow.Collection);
            FilterDefinition<Borrow> filter = Builders<Borrow>.Filter.Where(a => a.CustomerID == c.ID);
            List<Borrow> allborrows = db.ReadFiltered(filter);



            this.BorrowList = new ObservableCollection<Borrow>(allborrows);
            UpdateCustomer = new UpdateCustomerCommand(this);
        }

    }
}
