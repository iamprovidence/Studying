namespace ParkingSystem.VehicleInitializers
{
    public class DefaultVehicleInitializer : Interfaces.IVehicleInitializer
    {
        public void InitializeVehiclePrices(Interfaces.IParkingPlace parkingPlace)
        {
            parkingPlace.RegistrateVehicle(nameof(Vehicle.PassengerCar), 2M);
            parkingPlace.RegistrateVehicle(nameof(Vehicle.Motorcycle),   1M);
            parkingPlace.RegistrateVehicle(nameof(Vehicle.Bus),        3.5M);
            parkingPlace.RegistrateVehicle(nameof(Vehicle.Truck),        5M);
        }
    }
}
