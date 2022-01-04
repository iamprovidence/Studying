namespace ParkingSystem.Interfaces
{
    public interface ITransactionService
    {
        System.Collections.Generic.IList<Models.Transaction> Transactions { get; }

        decimal EarnedMoney();


        void WriteTransactionToFile(string fileName);
        string[] GetRawTransactionFromFile(string fileName);

        void ClearTransactionHistory();
        void Add(Models.Transaction transaction);
    }
}
