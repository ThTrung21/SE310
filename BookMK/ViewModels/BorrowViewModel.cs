using BookMK.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookMK.ViewModels
{
    public class BorrowViewModel:ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(BorrowViewModel));
        private ObservableCollection<Borrow> _borrows;
        public ObservableCollection<Borrow> Borrows
        {
            get { return _borrows; }
            set { _borrows = value; OnPropertyChanged(nameof(Borrows)); }
        }


        public ObservableCollection<string> ComboBoxItems { get; set; } = new ObservableCollection<string>(new List<string>() { "Customer", "Phone","Staff" });
        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value;/* Search()*/; OnPropertyChanged(nameof(SelectedIndex)); }
        }
        private DateTime _borrowdate;
        public DateTime BorrowDate
        {
            get
            {
                return _borrowdate;
            }
            set
            {
                _borrowdate = value;
                OnPropertyChanged(nameof(BorrowDate));
            }
        }
        private Customer _borrower;
        public Customer Borrower
        {
            get
            {
                return _borrower;
            }
            set
            {
                _borrower = value;
                OnPropertyChanged(nameof(Borrower));
            }
        }
        private Staff _processstaff;
        public Staff ProcessStaff
        {
            get
            {
                return _processstaff;
            }
            set
            {
                _processstaff = value;
                OnPropertyChanged(nameof(ProcessStaff));
            }
        }
        private string _searchString = "";
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                Search();
                OnPropertyChanged(nameof(SearchString));
            }
        }

        //private double _value;
        //public double Value
        //{
        //    get
        //    {
        //        return _value;
        //    }
        //    set
        //    {
        //        _value = value;
        //        OnPropertyChanged(nameof(Value));
        //    }
        //}









        public static async Task<BorrowViewModel> Initialize()
        {
            BorrowViewModel viewModel = new BorrowViewModel();
            await viewModel.InitializeAsync(); _logger.Information("OrderViewModel initialized");
            return viewModel;
        }
        private async Task InitializeAsync()
        {
            DataProvider<Borrow> db = new DataProvider<Borrow>(Borrow.Collection);
            List<Borrow> allorder = await db.ReadAllAsync();
            this._borrows = new ObservableCollection<Borrow>(allorder);

        }
        public void UpdateOrderList(List<Borrow> i)
        {
            this.Borrows.Clear();
            foreach (Borrow c in i)
            {
                Borrows.Add(c);
            }
        }


        private async void Search()
        {
            await Task.Run(async () =>
            {
                DataProvider<Borrow> db = new DataProvider<Borrow>(Borrow.Collection);
                string searchInput = SearchString.Trim();
                List<Borrow> results = new List<Borrow>();
                switch (_selectedIndex)
                {
                    //buyer name
                    case 0:
                        {
                            FilterDefinition<Borrow> filter = Builders<Borrow>.Filter.Regex("CustomerName", new BsonRegularExpression(searchInput, "i"));
                            results = db.ReadFiltered(filter);
                        }
                        break;

                    //buyer phone
                    case 1:
                        {
                            FilterDefinition<Borrow> filter = Builders<Borrow>.Filter.Regex("CustomerPhone", new BsonRegularExpression(searchInput, "i"));
                            results = db.ReadFiltered(filter);
                        }
                        break;
                   
                    case 2:
                        {
                            FilterDefinition<Borrow> filter = Builders<Borrow>.Filter.Regex("StaffName", new BsonRegularExpression(searchInput, "i"));
                            results = db.ReadFiltered(filter);
                        }
                        break;
                    default:
                        return;
                }
                Application.Current.Dispatcher.Invoke(() => {
                    Borrows.Clear();
                    foreach (Borrow c in results)
                    {
                        Borrows.Add(c);
                    }
                });
            });
        }


    }
}
