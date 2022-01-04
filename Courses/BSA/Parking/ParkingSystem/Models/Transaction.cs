namespace ParkingSystem.Models
{
    public class Transaction
    {
        // FIELDS
        readonly int vehicleId;
        readonly System.DateTime timeOfTransaction;
        readonly decimal moneyPaid;

        // CONSTRUCTORS
        public Transaction(int vehicleId, decimal moneyPaid)
        {
            this.vehicleId = vehicleId;
            this.moneyPaid = moneyPaid;
            this.timeOfTransaction = System.DateTime.Now;
        }

        // PROPERTIES
        public decimal MoneyPaid => moneyPaid;

        // METHODS
        public override string ToString()
        {
            return $"[{timeOfTransaction}] Vehicle id: {vehicleId}. Money paid {moneyPaid}";
        }
    }
}
