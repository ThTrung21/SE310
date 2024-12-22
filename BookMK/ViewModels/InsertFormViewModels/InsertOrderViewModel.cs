using BookMK.Commands;
using BookMK.Commands.InsertCommand;
using BookMK.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace BookMK.ViewModels.InsertFormViewModels
{
    public class InsertOrderViewModel: ViewModelBase
    {
       
        private static readonly ILogger _logger = Log.ForContext(typeof(InsertOrderViewModel));
        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value; OnPropertyChanged(nameof(SelectedBook));
            }
        }
        private Book _selectedTitleForComboBox = null;
		private BookCopy _selectedCopy;
        public BookCopy SelectedCopy;
        
        private Staff _cashier;
        public Staff Cashier
        {
            get { return _cashier; }
            set
            {
                _cashier=value; OnPropertyChanged(nameof(Cashier));
            }
        }

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private int _copyidinput;
        public int CopyIDInput
        {
            get { return _copyidinput; }
            set { _copyidinput = value; OnPropertyChanged(nameof(CopyIDInput)); }
        }
		

		public ObservableCollection<Book> ComboBoxBooks { get; set; } = new ObservableCollection<Book>(Book.GetBooksList());
        
		//public ObservableCollection<BookCopy> ComboBoxBookCopy { get; set; } = new ObservableCollection<BookCopy>(BookCopy.GetCopies(_selectedTitleForComboBox.ID));
		//private int _copyId;
		//public int CopyID
		//{
		//    get { return _copyId; }
		//    set { _copyId = value; OnPropertyChanged(nameof(CopyID)); }
		//}

		private ObservableCollection<BookCopy> _orderitemlist;
        public ObservableCollection<BookCopy> OrderItemList
        {
            get { return _orderitemlist; }
            set
            {
                _orderitemlist = value;
                OnPropertyChanged(nameof(OrderItemList));
            }
        }
        

        

       
        

        //adding item to list
        public ICommand AddItemCommand => new RelayCommand(AddItem);
        private void AddItem()
        {
            _logger.Information("AddItem method called.");
            //check null
            if (SelectedBook == null || CopyIDInput == -1)
            {
                MessageBox.Show("Error! Please check your copy id", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedCopy = BookCopy.GetBookCopy(SelectedBook.ID, CopyIDInput);

            //check availability
            if (SelectedCopy == null)
            { MessageBox.Show($"{SelectedBook.Title} copy number {CopyIDInput} is not available. Cannot add to order.", "Availability Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return; }
            if (SelectedCopy.IsRetire==true||SelectedCopy.Availability!=STATUS.Available ||  !BookCopy.IsExisted(SelectedCopy.ID))    
            {
                MessageBox.Show($"{SelectedBook.Title } copy number {SelectedCopy.CopyID} is not available. Cannot add to order.", "Availability Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //if (CopyIDInput > SelectedBook.Stock)
            //{
            //    MessageBox.Show("The amount of item is too much! Cannot add to order.", "Stock Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}
            

            
                OrderItemList.Add(SelectedCopy);
        }
            
           

        public ICommand RemoveItemCommand => new RelayCommand<BookCopy>(RemoveItem);
        private void RemoveItem(BookCopy item)
        {
            _logger.Information("RemoveItem method called.");


            int index = OrderItemList.IndexOf(item);

            // Check if the removed item is not the last one
            while (index < OrderItemList.Count - 1)
            {
                // Get the item right behind the removed item
                BookCopy nextItem = OrderItemList[index + 1];

                
            }

            // Remove the original item

            OrderItemList.Remove(item);
            _logger.Information("Item removed successfully.");
        }



        //--------------------------------------------------------------------------------------------------------------



        private DateTime _returndate= DateTime.Now;
        public DateTime ReturnDate
        {
            get { return _returndate; }
            set
            {
                _returndate = value;
                OnPropertyChanged(nameof(ReturnDate));
            }
        }

       

        public ObservableCollection<Customer> ComboBoxCustomer { get; set; } = new ObservableCollection<Customer>(Customer.GetCustomerList());
        
        
     
        private Customer _selectedcustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedcustomer; }
            set
            {
                _selectedcustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        

        
       

        
        



        public void UpdateListItem(ObservableCollection<BookCopy> results)
        {
            _logger.Information("UpdateListItem method called.");
            this.OrderItemList.Clear();
            foreach (BookCopy s in results)
            {
                OrderItemList.Add(s);
            }
        }


        public InsertOrderViewModel()
        {
            

        }
        public InsertOrderViewModel(Staff s)
        {
            _logger.Information("InsertOrderViewModel initialized");
            this.Cashier = s;
            
            
            
            this.OrderItemList = new ObservableCollection<BookCopy>();
            this.InsertOrder = new InsertOrderCommand(this, Cashier);
        }
        public ICommand InsertOrder { get;set; }
        public static async Task<InsertOrderViewModel> Initialize()
        {
            InsertOrderViewModel viewModel = new InsertOrderViewModel();
            await viewModel.IntializeAsync();
            return viewModel;
        }
        private async Task IntializeAsync()
        {
            await Task.Run(async () =>
            {
                // Simulate an asynchronous operation
                await Task.Delay(1000);


            });

        }

    }
}
