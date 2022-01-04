namespace ParkingSystem.Models
{
    public class Transaction
    {
        // CONSTRUCTORS
        public Transaction(int vehicleId, decimal moneyPaid)
        {
            this.VehicleId = vehicleId;
            this.MoneyPaid = moneyPaid;
            this.TimeOfTransaction = System.DateTime.Now;
        }

        // PROPERTIES
        [Newtonsoft.Json.JsonProperty("vehicleId")]
        public int VehicleId { get; set; }
        [Newtonsoft.Json.JsonProperty("timeOfTransaction")]
        public System.DateTime TimeOfTransaction { get; set; }
        [Newtonsoft.Json.JsonProperty("moneyPaid")]
        public decimal MoneyPaid { get; set; }

        // METHODS
        public override string ToString()
        {
            return $"[{TimeOfTransaction}] Vehicle id: {VehicleId}. Money paid {MoneyPaid}";
        }
    }
}
