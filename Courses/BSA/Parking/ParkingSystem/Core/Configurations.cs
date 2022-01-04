namespace ParkingSystem.Core
{
    public static class Configurations
    {
        public static readonly decimal INITIAL_PARKING_BALANCE = 0;
        public static readonly decimal INITIAL_VEHICLE_BALANCE = 20;
        public static readonly int PARKING_CAPACITY = 10;
        public static readonly int PAYMENT_FREQUENCY_IN_SECOND = 5;
        public static readonly int SAVE_COMMIT_TIME_FREQUENCY_IN_MINUTE = 1;
        public static readonly decimal FINE_FACTOR = 2.5M;
        public static readonly string TRANSACTION_LOG_FILE = "Transactions.log";
    }
}
