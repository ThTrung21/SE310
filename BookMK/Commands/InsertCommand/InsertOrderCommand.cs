using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertOrderCommand : AsyncCommandBase
    {
        private readonly InsertOrderViewModel vm;
        private readonly Staff Cashier;
        bool operationSucceeded = false;
        public InsertOrderCommand(InsertOrderViewModel vm, Staff c)
        {
            this.vm = vm;
            this.Cashier = c;
        }



        public override async Task ExecuteAsync(object parameter)
        {
            int _ID = vm.ID;
            ObservableCollection<BookCopy> list = vm.OrderItemList;

            if (vm.SelectedCustomer == null)
            {
                MessageBox.Show("Please choose a member first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Borrow o = new Borrow()
            {
                ID = Borrow.CreateID(),
                BorrowedCopies = list,
                BorrowStatus = BORROWSTATUS.Borrowing,
                // Customer info
                CustomerID = vm.SelectedCustomer.ID,
                CustomerName = vm.SelectedCustomer.FullName,
                CustomerPhone = vm.SelectedCustomer.Phone,

                // Cashier info
                StaffID = Cashier.ID,
                StaffName = Cashier.FullName,

                BorrowDate = DateTime.Now,
                ReturnDate = vm.ReturnDate,

            };
            try
            {
                // Add to cache before attempting to insert
                SimpleCache.AddOrUpdate($"order_{_ID}", o);
                // Define retry policy with exponential backoff
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of inserting author failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    DataProvider<Borrow> db = new DataProvider<Borrow>(Borrow.Collection);
                    await db.InsertOneAsync(o); operationSucceeded = true;
                });

                // Update book's copy info
                foreach (var item in vm.OrderItemList)
                {

                    FilterDefinition<BookCopy> filter = Builders<BookCopy>.Filter.Eq(x => x.ID, item.ID);
                    UpdateDefinition<BookCopy> update = Builders<BookCopy>.Update
                        .Set(x => x.BorrowerID, o.CustomerID)
                        .Set(x => x.Availability, STATUS.Borrowing);
                    DataProvider<BookCopy> dbb = new DataProvider<BookCopy>(BookCopy.Collection);
                    dbb.Update(filter, update);
                }

                // Update customer points and loyal discount status (if any)
                {
                    
                        FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.ID, o.CustomerID);
                        UpdateDefinition<Customer> update = Builders<Customer>.Update
                            .AddToSet(x => x.BorrowedIDList, _ID);
                        DataProvider<Customer> dbc = new DataProvider<Customer>(Customer.Collection);
                        dbc.Update(filter, update);
                    operationSucceeded = true;
                }
                if (operationSucceeded)
                {
                    MessageBox.Show("A new import has been recorded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window f = parameter as Window;
                    f?.Close();


                    // Remove from cache after successful insert
                    SimpleCache.Remove($"import_ {_ID}");

                    // Log success
                    Log.Information("A new borrow has been recorded: ImportID - {ImportID}, Time - {ImportTime}", _ID, DateTime.Now);
                }
                else
                {
                    // Notify user after all retries failed
                    MessageBox.Show("Failed to create the import after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Window f = parameter as Window;
                    f?.Close();
                }

            }
            catch (Exception ex)
            {
                var cachedAuthor = SimpleCache.Get<Borrow>($"borrow_{_ID}");
    //               

                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new order.");


            }
        }
    }
}
