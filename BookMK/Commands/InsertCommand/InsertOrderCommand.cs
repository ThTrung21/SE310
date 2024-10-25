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
    //public class InsertOrderCommand : AsyncCommandBase
    //{
    //    private readonly InsertOrderViewModel vm;
    //    private readonly Staff Cashier;
    //    bool operationSucceeded=false;
    //    public InsertOrderCommand(InsertOrderViewModel vm, Staff c)
    //    {
    //        this.vm = vm;
    //        this.Cashier = c;
    //    }

       

    //    public override async Task ExecuteAsync(object parameter)
    //    {
    //        int _ID = vm.ID;
    //        ObservableCollection<BookCopy> list = vm.OrderItemList;

    //        if (vm.SelectedCustomer == null)
    //        {
    //            MessageBox.Show("Please choose a buyer first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    //            return;
    //        }

    //        Borrow o = new Borrow()
    //        {
    //            ID = Borrow.CreateID(),
    //            Items = list,
    //            // Customer info
    //            CustomerID = vm.SelectedCustomer.ID,
    //            CustomerName = vm.SelectedCustomer.FullName,
    //            CustomerPhone = vm.SelectedCustomer.Phone,
    //            // Cashier info
    //            StaffID = Cashier.ID,
    //            StaffName = Cashier.FullName,
    //            Time = DateTime.Now,
               
    //        };
    //        try
    //        {
    //            // Add to cache before attempting to insert
    //            SimpleCache.AddOrUpdate($"order_{_ID}", o);
    //            // Define retry policy with exponential backoff
    //            var retryPolicy = Policy
    //                .Handle<Exception>()
    //                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
    //                    (exception, timeSpan, retryCount, context) =>
    //                    {
    //                        Log.Warning("Retry {RetryCount} of inserting author failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
    //                    });

    //            await retryPolicy.ExecuteAsync(async () =>
    //            {
    //                DataProvider<Borrow> db = new DataProvider<Borrow>(Borrow.Collection);
    //                await db.InsertOneAsync(o); operationSucceeded = true;
    //            });

    //            // Update books stock
    //            foreach (var item in vm.OrderItemList)
    //            {
    //                FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, item.BookID);
    //                UpdateDefinition<Book> update = Builders<Book>.Update.Inc(x => x.Stock, -item.Quantity);
    //                DataProvider<Book> dbb = new DataProvider<Book>(Book.Collection);
    //                dbb.Update(filter, update);
    //            }

    //            // Update customer points and loyal discount status (if any)
    //            {
    //                if (vm.SelectedCustomer.ID != 0)
    //                {
                       
                      

    //                    //FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.ID, vm.SelectedCustomer.ID);
    //                    //UpdateDefinition<Customer> update = Builders<Customer>.Update
    //                    //    .Set(x => x.PurchasePoint, vm.SelectedCustomer.PurchasePoint)
    //                    //    .Set(x => x.IsLoyalDiscountReady, vm.SelectedCustomer.IsLoyalDiscountReady);

    //                    //DataProvider<Customer> dbc = new DataProvider<Customer>(Customer.Collection);
    //                    //dbc.Update(filter, update);
    //                }
    //                else
    //                {
    //                    FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.ID, 0);
    //                    UpdateDefinition<Customer> update = Builders<Customer>.Update
    //                        .Set(x => x.PurchasePoint, vm.SelectedCustomer.PurchasePoint);
    //                    DataProvider<Customer> dbc = new DataProvider<Customer>(Customer.Collection);
    //                    dbc.Update(filter, update);
    //                }
    //            }
    //            if (operationSucceeded)
    //            {
    //                MessageBox.Show("A new purchase has been recorded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    //            Window f = parameter as Window;
    //            f?.Close();

    //            // Log success
    //            Log.Information("A new purchase has been recorded: OrderID - {OrderID}, Time - {OrderTime}", o.ID, DateTime.Now);
    //            }
    //            else
    //                {
    //                    // Notify user after all retries failed
    //                    MessageBox.Show("Failed to create the order after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    //                }
    //        }
    //        catch (Exception ex)
    //        {
    //            var cachedAuthor = SimpleCache.Get<Borrow>($"borrow_{_ID}");

    //            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

    //            // Log error
    //            Log.Error(ex, "Error occurred while inserting a new order.");

                
    //        }
    //    }
    //}
}
