using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateBookCommand : AsyncCommandBase
    {
        private readonly ViewBookViewModel vm;
        private readonly StringBuilder filename;
        private bool operationSucceeded = false;

        public UpdateBookCommand(ViewBookViewModel vm, StringBuilder filename)
        {
            this.vm = vm;
            
            
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Book _CurrentBook = vm.CurrentBook;

          

            try
            {
                // Add to cache before attempting to update
               

                // Define retry policy with exponential backoff
                

               
                    FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, _CurrentBook.ID);
                    UpdateDefinition<Book> update = Builders<Book>.Update
                        .Set(x => x.ReleaseYear, _CurrentBook.ReleaseYear)
                        .Set(x => x.Title, _CurrentBook.Title);
                        

                    //// More attributes to update can be added here

                    DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                    //await db.ReadAllAsync();
                await db.UpdateAsync(filter, update);
                operationSucceeded = true;
                if (operationSucceeded)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Staff updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });
                }


            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedBook = SimpleCache.Get<Book>($"book_{_CurrentBook.ID}");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });

                // Log error
                Log.Error(ex, "Error occurred while updating book. Cached book: {@Book}", cachedBook);
            }
        }
    }
}
