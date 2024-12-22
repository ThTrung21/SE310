using BookMK.Models;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class RetireBookCopyCommand : AsyncCommandBase
    {
        private readonly ViewBookViewModel viewModel;
        public RetireBookCopyCommand(ViewBookViewModel vm)
        {
            this.viewModel = vm;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                FilterDefinition<BookCopy> filter = Builders<BookCopy>.Filter.Eq(x => x.ID, viewModel.CurrentCopy.ID);
                UpdateDefinition<BookCopy> update = Builders<BookCopy>.Update
                     .Set(x => x.IsRetire, true);

                DataProvider<BookCopy> db = new DataProvider<BookCopy>(BookCopy.Collection);
                await db.UpdateAsync(filter, update);
            }
            catch (Exception e){
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }

}
