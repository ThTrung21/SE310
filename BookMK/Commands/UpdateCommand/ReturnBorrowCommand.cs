using BookMK.Models;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookMK.Commands.UpdateCommand
{
    public class ReturnBorrowCommand: AsyncCommandBase
    {
        private readonly ViewOrderViewModel viewModel;
        public ReturnBorrowCommand(ViewOrderViewModel vm)
        {
            this.viewModel = vm;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                FilterDefinition<Borrow> filter = Builders<Borrow>.Filter.Eq(x => x.ID, viewModel.CurrentOrder.ID);
                UpdateDefinition<Borrow> update = Builders<Borrow>.Update
                     .Set(x => x.BorrowStatus, BORROWSTATUS.Returned);

                DataProvider<Borrow> db = new DataProvider<Borrow>(Borrow.Collection);
                await db.UpdateAsync(filter, update);

                foreach (var item in viewModel.CurrentOrder.BorrowedCopies)
                {

                    FilterDefinition<BookCopy> filter2 = Builders<BookCopy>.Filter.Eq(x => x.ID, item.ID);
                    UpdateDefinition<BookCopy> update2 = Builders<BookCopy>.Update
                        .Set(x => x.BorrowerID,-1)
                        .Set(x => x.Availability, STATUS.Available);
                    DataProvider<BookCopy> dbb = new DataProvider<BookCopy>(BookCopy.Collection);
                    dbb.Update(filter2, update2);
                }
                MessageBox.Show("Borrowed books are returned!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window f = parameter as Window;
                f?.Close();

            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }
}
