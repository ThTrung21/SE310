using BookMK.Models;
using BookMK.ViewModels.ViewForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookMK.Views.ViewForm
{
    /// <summary>
    /// Interaction logic for ViewOrderForm.xaml
    /// </summary>
    public partial class ViewOrderForm : Window
    {

        public ViewOrderForm()
        {
            InitializeComponent();
        }
        public ViewOrderForm(Borrow o)
        {
            InitializeComponent();
            if (o.BorrowStatus==BORROWSTATUS.Returned)
                CheckoutBtn.Visibility = Visibility.Collapsed;
            this.DataContext = new ViewOrderViewModel(o);
        }
        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void UpdateOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewOrderViewModel vm = this.DataContext as ViewOrderViewModel;
            if (vm.ReturnBorrow != null)
            {
                vm.ReturnBorrow.Execute(this);
            }
        }
    }
}
