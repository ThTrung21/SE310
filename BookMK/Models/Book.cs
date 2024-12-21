using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookMK.Models
{
    public enum CONDITION
    {
        New,
        OK,
        Damaged,
       
    }
    public enum STATUS
    {
        
        Available,
        Borrowing,
        Overdue,
    }
    public class Book
    {
        

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]

        public int ID { get; set; }

        //avatar
        public string Cover { get; set; }
        
        public string Title { get; set; }
        public string ISBN { get; set; }

        
        public List<string> Genre { get; set; }
        public string ReleaseYear { get; set; }

        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        
        
        public int Stock { get; set; }
        public static string Collection = "books";

        public Book() { }
        public override string ToString()
        {
            if (this.Title != null)
                return this.Title;
            return "";
        }
        public static int CreateID()
        {
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            List<Book> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
            int expectedValue = 1;
            foreach (var o in allcs)
            {
                int num = o.ID;
                if (num == expectedValue)
                {
                    // Increment the expected value if it matches the current number
                    expectedValue++;
                }
                else
                {
                    // Found the smallest missing integer
                    return expectedValue;
                }
            }
            return expectedValue;
        }
        public static bool IsExisted(int id)
        {
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, 1);
            return db.collection.Find(filter).Any();
        }
        public static bool IsExisted(string Name, string Author, string Year)
        {


            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            FilterDefinition<Book> filter = Builders<Book>.Filter.And(
                Builders<Book>.Filter.Regex(x => x.Title, new BsonRegularExpression("^" + Regex.Escape(Name) + "$", "i")),
                Builders<Book>.Filter.Regex(x => x.AuthorName, new BsonRegularExpression("^" + Regex.Escape(Author) + "$", "i")),
                Builders<Book>.Filter.Regex(x => x.ReleaseYear, new BsonRegularExpression("^" + Regex.Escape(Year) + "$", "i"))
                
                );

            //List<Staff> resultls = db.ReadFiltered(filter);
            //bool exists = resultls.Count > 0;

            //return exists;
            return db.collection.Find(filter).Any();


        }
        public static List<Book> GetBooksList()
        {
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            List<Book> AllBooks = db.ReadAll();

            return AllBooks;
        }
        public static string GetTitle(int id)
        {
			DataProvider<Book> db = new DataProvider<Book>(Book.Collection);

			FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, id);
            Book a =  db.ReadFiltered(filter).FirstOrDefault();
            return a.Title;
        }

        public static Book GetBook(int id)
        {
            if(id==0)
            {
                return null;
            }

            Book c = new Book();
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);

            FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, id);
            List<Book> b = db.ReadFiltered(filter);
            if (b.Count > 0)
            {
                return b[0];
            }
            else
                return null;
        }


        public static List<Book> GetOutOfStock()
        {
           

           
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);

            FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.Stock, 0);
            List<Book> b = db.ReadFiltered(filter);
            return b;
        }
    }


    public class BookCopy
    {

        

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        public int ID { get; set; }

        public int CopyID { get; set; }//the id of the copy in one title
        public int BookID { get; set; } // Reference to the main Book
        public CONDITION Condition { get; set; } // e.g., New, Good, Damaged
        public STATUS Availability { get; set; } // Whether the copy is currently available
        //public DateTime? BorrowedDate { get; set; }
        //public DateTime? DueDate { get; set; }
        public int? BorrowerID { get; set; }

        public bool IsRetire { get; set; }

        public static string Collection = "bookcopies";
        

        public static int CreateID()
        {
            DataProvider<BookCopy> db = new DataProvider<BookCopy>(BookCopy.Collection);
            List<BookCopy> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
            int expectedValue = 1;
            foreach (var o in allcs)
            {
                int num = o.ID;
                if (num == expectedValue)
                {
                    // Increment the expected value if it matches the current number
                    expectedValue++;
                }
                else
                {
                    // Found the smallest missing integer
                    return expectedValue;
                }
            }
            return expectedValue;
        }
        public static int CreateCopyID(int BookID)
        {
            DataProvider<BookCopy> db = new DataProvider<BookCopy>(BookCopy.Collection);
            List<BookCopy> allcs = db.ReadAll().Where(a=>a.BookID==BookID).OrderBy(p => p.ID).ToList();
            int expectedValue = 1;
            foreach (var o in allcs)
            {
                int num = o.ID;
                if (num == expectedValue)
                {
                    // Increment the expected value if it matches the current number
                    expectedValue++;
                }
                else
                {
                    // Found the smallest missing integer
                    return expectedValue;
                }
            }
            return expectedValue;
        }
        public static bool IsExisted(int id)
        {
            DataProvider<BookCopy> db = new DataProvider<BookCopy>(BookCopy.Collection);
            FilterDefinition<BookCopy> filter = Builders<BookCopy>.Filter.Eq(x => x.ID, 1);
            return db.collection.Find(filter).Any();
        }
        public static BookCopy GetBookCopy(int titleID, int copyID)
        {
			

			DataProvider<BookCopy> db = new DataProvider<BookCopy>(BookCopy.Collection);
            FilterDefinition<BookCopy> filter = Builders<BookCopy>.Filter.Where(x=>x.CopyID==copyID && x.BookID==titleID);
			return db.ReadFiltered(filter).FirstOrDefault();

        }
        public static List<BookCopy> GetCopies(int titleID)
        {
			DataProvider<BookCopy> db = new DataProvider<BookCopy>(BookCopy.Collection);
			FilterDefinition<BookCopy> filter = Builders<BookCopy>.Filter.Where(x=>x.BookID == titleID);
            return db.ReadFiltered(filter).ToList();
		}
        
    }
   
}
