namespace ParkingSystem.Interfaces
{
    public interface IParkingPlace
    {
        int ParkingCapacity { get; }
        bool HasSpace { get; }
        int FreeSpacesAmount { get; }
        int BusySpacesAmount { get; }

        System.Collections.Generic.IList<Vehicle.VehicleBase> Vehicles { get; }
        System.Collections.Generic.IDictionary<string, decimal> Prices { get; }
        System.Collections.Generic.IDictionary<int, decimal> Debtors { get; }
        
        bool AddCar(Vehicle.VehicleBase vehicle);
        bool DeleteCar(int vehicleId);

        Vehicle.VehicleBase GetVehicleById(int vehicleId);

        bool IsDebtor(int vehicleId);

        void RegistrateVehicle(string name, decimal price);
        void UnRegistrateVehicle(string name);
    }
}
