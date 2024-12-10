using BookMK.Commands;
using BookMK.Commands.InsertCommand;
using BookMK.Models;
using MongoDB.Driver;
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
        private BookCopy _selectedCopy;
        public BookCopy SelectedCopy
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
            if (SelectedBook == null || CopyIDInput == null)
            {
                MessageBox.Show("Error! Please check your copy id", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SelectedCopy= 
            if (SelectedBook.Stock < 1)
            {
                MessageBox.Show($"{SelectedBook.Title} is out of stock. Cannot add to order.", "Stock Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CopyIDInput > SelectedBook.Stock)
            {
                MessageBox.Show("The amount of item is too much! Cannot add to order.", "Stock Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }




            // Create a new bookcopy instance and add it to the ObservableCollection
            BookCopy newItem = new BookCopy
            {
                //SellBook = this.SelectedBook.Title,
                BookID = this.SelectedBook.ID,
                //isGifted = false,
                //Quantity = this.CopyIDInput,


            };



            //checkstock
            {
                
            }





            OrderItemList.Add(newItem);
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

                // Check if the next item is marked as gifted
                //if (nextItem.isGifted)
                //{
                //    // Remove the next item
                //    OrderItemList.Remove(nextItem);
                //}
                //else
                //{
                //    // If the next item is not marked as gifted, break the loop
                //    break;
                //}
            }

            // Remove the original item

            OrderItemList.Remove(item);
            _logger.Information("Item removed successfully.");
        }
        //--------------------------------------------------------------------------------------------------------------

        

        
       

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
        public bool loyaldiscountflag;

        
       

        
        



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
            
            
            loyaldiscountflag=false;
            this.OrderItemList = new ObservableCollection<BookCopy>();
           // this.InsertOrder = new InsertOrderCommand(this, Cashier);
        }
        public ICommand InsertOrder { get;set; }
        //public static async Task<InsertOrderViewModel> Initialize()
        //{
        //    InsertOrderViewModel viewModel = new InsertOrderViewModel();
        //    await viewModel.IntializeAsync();
        //    return viewModel;
        //}
        //private async Task IntializeAsync()
        //{
        //    await Task.Run(async () =>
        //    {
        //        // Simulate an asynchronous operation
        //        await Task.Delay(1000);
                
                
        //    });

        //}

    }
}
