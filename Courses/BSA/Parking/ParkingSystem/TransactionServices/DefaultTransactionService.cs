using System.IO;
using System.Linq;
using System.Collections.Generic;

using ParkingSystem.Models;

namespace ParkingSystem.TransactionServices
{
    public class DefaultTransactionService : Interfaces.ITransactionService
    {
        // FIELDS
        IList<Transaction> transactions;
        
        // CONSTRUCTORS
        public DefaultTransactionService()
        {
            transactions = new List<Transaction>();
        }

        // PROPERTIES
        public IList<Transaction> Transactions => transactions;

        // METHODS
        public void ClearTransactionHistory()
        {
            transactions.Clear();
        }
        public void WriteTransactionToFile(string fileName)
        {
            using (StreamWriter w = File.AppendText(fileName))
            {
                foreach (Transaction transaction in transactions)
                {
                    w.WriteLine(transaction.ToString());
                }
            }
        }

        public string[] GetRawTransactionFromFile(string fileName)
        {
            return File.ReadAllLines(fileName);
        }
        public void Add(Transaction transaction)
        {
            transactions.Add(transaction);
        }

        public decimal EarnedMoney()
        {
            return transactions.Sum(t => t.MoneyPaid);
        }

    }
}
