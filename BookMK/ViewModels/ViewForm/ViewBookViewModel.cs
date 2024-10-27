using Amazon.Runtime.Internal.Util;
using BookMK.Commands;
using BookMK.Commands.DeleteCommand;
using BookMK.Commands.UpdateCommand;
using BookMK.Models;
using BookMK.Service;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ILogger = Serilog.ILogger;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewBookViewModel : ViewModelBase
    {
        public ICommand RetireBookCopyCommand { get; }
        private readonly FirebaseStorageService _firebaseStorageService = new FirebaseStorageService();

        private static readonly ILogger _logger = Log.ForContext(typeof(ViewBookViewModel));


        #region Book
        private Book _currentBook = new Book();
        public Book CurrentBook
        {
            get => _currentBook;
            set { _currentBook = value; OnPropertyChanged(nameof(CurrentBook)); }
        }
        private Int32 _id;
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }
        private string _releaseyear;
        public string ReleaseYear
        {
            get { return _releaseyear; }
            set { _releaseyear = value; OnPropertyChanged(nameof(ReleaseYear)); }
        }
        private int _isbn;
        public int ISBN
        {
            get { return _isbn; }
            set { _isbn = value; OnPropertyChanged(nameof(ISBN)); }
        }
        private int _stock;
        public int Stock
        {
            get { return _stock; }
            set { _stock = value; OnPropertyChanged(nameof(Stock)); }
        }
        private string _author;
        public string AuthorName
        {
            get { return _author; }
            set { _author = value; OnPropertyChanged(nameof(Author)); }
        }
        private StringBuilder _filename = new StringBuilder("");
        public StringBuilder Filename
        {
            get => _filename;
            set { _filename = value; OnPropertyChanged(nameof(Filename)); }
        }
        private string _cover;
        public string Cover
        {
            get { return _cover; }
            set { _cover = value; OnPropertyChanged(nameof(Cover)); }
        }
        #endregion

        #region Copy
        private ObservableCollection<BookCopy> _copies;
        public ObservableCollection<BookCopy> Copies
        {
            get { return _copies; }
            set { _copies = value; OnPropertyChanged(nameof(Copies)); }
        }
        private int _copyID;
        public int CopyID
        {
            get { return _copyID; }
            set { _copyID = value; OnPropertyChanged(nameof(CopyID)); }
        }

        private CONDITION _condition;
        public CONDITION CONDITION
        {
            get { return _condition; }
            set { _condition = value; OnPropertyChanged(nameof(CONDITION)); }
        }
        private STATUS _availability;
        public STATUS Availability
        {
            get { return _availability; }
            set { _availability = value; OnPropertyChanged(nameof(Availability)); }
        }

        private int _borrowerID;
        public int BorrowerID
        {
            get { return _borrowerID; }
            set 
            {
                _borrowerID = value;
                OnPropertyChanged(nameof(StrBorrowerID)); 
                OnPropertyChanged(nameof(BorrowerID)); 
            }
        }

        public string StrBorrowerID => BorrowerID == -1 ? "_" : BorrowerID.ToString();

        private bool _isretire;
        public bool IsRetire
        {
            get { return _isretire; }
            set { _isretire = value; OnPropertyChanged(nameof(IsRetire)); }
        }



        #endregion


        private Visibility _adminVisibility;
        public Visibility AdminVisibility
        {
            get => _adminVisibility;
            set { _adminVisibility = value; OnPropertyChanged(nameof(AdminVisibility)); }
        }


        public ICommand UpdateBook { get;set; }
        public ICommand SaveImageDialog { get; set; }
        public ICommand DeleteBook { get; set; }

        public ViewBookViewModel() { _logger.Information("ViewBookViewModel constructor called."); }
        public ViewBookViewModel(Book b, string role) 
        {
            _logger.Information("ViewBookViewModel constructor with Book {a} parameter called.",b.ID);
            this.CurrentBook = b;
            DataProvider<BookCopy> db= new DataProvider<BookCopy>(BookCopy.Collection);

            FilterDefinition<BookCopy> filter = Builders<BookCopy>.Filter.Where(a => a.BookID == b.ID);
            List<BookCopy> allcopies = db.ReadFiltered(filter);

            this._copies=new ObservableCollection<BookCopy>(allcopies);
            AdminVisibility = role == "admin" ? Visibility.Visible : Visibility.Collapsed;

            //this.Filename.Clear();
            //FirebaseStorageService a = new FirebaseStorageService();
            //this.Filename.Append(b.Cover);
            //RetireBookCopyCommand = new RelayCommand<BookCopy>(RetireBookCopy);


            this.SaveImageDialog = new SaveImageDialogCommand(Filename, this);
            this.UpdateBook = new UpdateBookCommand(this, Filename);
            this.DeleteBook = new DeleteBookCommand(this, Filename);

        }


        private void RetireBookCopy(BookCopy copy)
        {
            if (copy != null)
            {
                copy.IsRetire = true;
                OnPropertyChanged(nameof(Copies)); // Notify the ListView to update

                // Optionally flag as updated to be saved later
                // Add logic here to mark for later update, if needed
            }
        }
    }
}
