using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
	public enum BORROWSTATUS { Borrowing, Overdued, Returned }
	public class Borrow
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        

        public int ID { get; set; }
        
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public int StaffID { get; set; }
        public string StaffName { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public List<BookCopy> BorrowedCopies { get; set; }

        public static string Collection = "borrows";

        public BORROWSTATUS BorrowStatus { get; set; }

        public static int CreateID()
        {
            DataProvider<Borrow> db = new DataProvider<Borrow>(Borrow.Collection);
            List<Borrow> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
            int expectedValue = 0;
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
    }
}
