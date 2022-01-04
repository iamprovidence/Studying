using ParkingSystem.Interfaces;
using static ParkingSystem.Core.Configurations;

namespace ParkingSystem
{
    public class Parking
    {
        // FIELDS
        decimal balance;

        readonly IParkingPlace parkingPlace;
        readonly ITimeService timeService;
        readonly ITransactionService transactionService;

        static Parking instance;

        // CONSTRUCTORS
        private Parking(IParkingPlace parkingPlace, ITimeService timeService, ITransactionService transactionService, IVehicleInitializer vehicleInitializer)
        {
            this.balance = INITIAL_PARKING_BALANCE;

            this.parkingPlace = parkingPlace;
            this.timeService = timeService;
            this.transactionService = transactionService;
            vehicleInitializer.InitializeVehiclePrices(this.parkingPlace);

            SubscribeServicesOnTimeEvent();
        }
        static Parking()
        {
            instance = new Parking(
                new ParkingPlace.DefaultParkingPlace(),
                
                new TimeServices.DefaultTimeService(
                    partTimeFromSecond: PAYMENT_FREQUENCY_IN_SECOND, 
                    fullTimeFromMinute: SAVE_COMMIT_TIME_FREQUENCY_IN_MINUTE),

                new TransactionServices.DefaultTransactionService(),
                new VehicleInitializers.DefaultVehicleInitializer());
        }
        ~Parking()
        {
            (timeService as System.IDisposable)?.Dispose();
        }

        private void SubscribeServicesOnTimeEvent()
        {
            timeService.FullTimeWent += (obj, evetnArgs) =>
            {
                transactionService.WriteTransactionToFile(TRANSACTION_LOG_FILE);
                transactionService.ClearTransactionHistory();
            };

            timeService.PartTimeWent += (obj, evetnArgs) => 
            {
                PayTime();
            };
        }

        // PROPERIES
        public decimal Balance => balance;
        public IParkingPlace ParkingPlace => parkingPlace;
        public ITransactionService TransactionService => transactionService;

        public static Parking GetInstance => instance;

        // METHODS
        private void PayTime()
        {
            foreach (Vehicle.VehicleBase vehicle in parkingPlace.Vehicles)
            {
                Pay(vehicle);
            }
        }
        private void Pay(Vehicle.VehicleBase vehicle)
        {
            decimal moneyToPay = parkingPlace.Prices[vehicle.GetType().Name];
            if (parkingPlace.IsDebtor(vehicle.Id)) moneyToPay *= FINE_FACTOR;

            if (vehicle.CanPay(moneyToPay))
            {
                TransferMoneyToParking(vehicle, moneyToPay);

                transactionService.Add(new Models.Transaction(vehicle.Id, moneyToPay));
            }
            else
            {
                parkingPlace.Debtors[vehicle.Id] = moneyToPay * FINE_FACTOR;
            }
        }
        private void TransferMoneyToParking(Vehicle.VehicleBase vehicle, decimal moneyAmount)
        {
            vehicle.Balance -= moneyAmount;

            if (parkingPlace.IsDebtor(vehicle.Id))
            {
                parkingPlace.Debtors[vehicle.Id] -= moneyAmount;
            }

            this.balance += moneyAmount;
        }

    }
}
