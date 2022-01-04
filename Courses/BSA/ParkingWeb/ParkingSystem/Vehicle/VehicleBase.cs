namespace ParkingSystem.Vehicle
{
    public class VehicleBase
    {
        // FIELDS
        static int counter;
        
        // CONSTRUCTORS
        public VehicleBase()
        {
            this.Id = counter++;
            this.Name = GetName();
            this.Balance = Core.Configurations.INITIAL_VEHICLE_BALANCE;
        }
        static VehicleBase()
        {
            counter = 0;
        }

        // PROPERTIES
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("balance")]
        public decimal Balance { get; set; }

        // METHODS
        public bool CanPay(decimal money)
        {
            return Balance >= money;
        }
        public override string ToString()
        {
            return $"[{Id}] --{Name}-- has {Balance} on his balance";
        }
        protected virtual string GetName()
        {
            return this.GetType().Name;
        }
    }
}
