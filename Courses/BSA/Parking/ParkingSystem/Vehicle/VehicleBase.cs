namespace ParkingSystem.Vehicle
{
    public abstract class VehicleBase
    {
        // FIELDS
        static int counter;

        int id;
        decimal balance;

        // CONSTRUCTORS
        public VehicleBase()
        {
            this.id = counter++;
            this.balance = Core.Configurations.INITIAL_VEHICLE_BALANCE;
        }
        static VehicleBase()
        {
            counter = 0;
        }

        // PROPERTIES
        public int Id => id;
        public decimal Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }

        // METHODS
        public bool CanPay(decimal money)
        {
            return balance >= money;
        }
        public override string ToString()
        {
            return $"[{id}] --{this.GetType().Name}-- has {balance} on his balance";
        }
    }
}
