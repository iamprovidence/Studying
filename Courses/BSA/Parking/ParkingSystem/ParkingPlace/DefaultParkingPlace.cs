using System.Linq;
using System.Collections.Generic;

using ParkingSystem.Vehicle;

namespace ParkingSystem.ParkingPlace
{
    public class DefaultParkingPlace : Interfaces.IParkingPlace
    {
        // FIELDS
        IList<VehicleBase> vehicles;
        IDictionary<string, decimal> prices;
        IDictionary<int, decimal> debstorrs;
        int parkingCapacity;

        // CONSTRUCTORS
        public DefaultParkingPlace()
            : this(Core.Configurations.PARKING_CAPACITY) { }

        public DefaultParkingPlace(int parkingCapacity)
        {
            this.parkingCapacity = parkingCapacity;
            this.vehicles = new List<VehicleBase>(parkingCapacity);
            this.prices = new Dictionary<string, decimal>();
            this.debstorrs = new Dictionary<int, decimal>();
        }

        // PROPERTIES
        public int ParkingCapacity => parkingCapacity;
        public IList<VehicleBase> Vehicles => vehicles;
        public IDictionary<string, decimal> Prices => prices;
        public IDictionary<int, decimal> Debtors => debstorrs;

        public bool HasSpace => parkingCapacity > vehicles.Count;

        public int FreeSpacesAmount => parkingCapacity - vehicles.Count;
        public int BusySpacesAmount => vehicles.Count;

        // METHODS
        public bool AddCar(VehicleBase vehicle)
        {
            if (HasSpace)
            {
                vehicles.Add(vehicle);
                return true;
            }
            return false;
        }
        public bool DeleteCar(int carId)
        {
            int carToDeleteIndex = ((List<VehicleBase>)vehicles).FindIndex(car => car.Id == carId);

            if (carToDeleteIndex != -1)
            {
                vehicles.RemoveAt(carToDeleteIndex);
                return true;
            }
            return false;
        }

        public VehicleBase GetVehicleById(int vehicleId)
        {
            return vehicles.FirstOrDefault(v => v.Id == vehicleId);
        }

        public bool IsDebtor(int vehicleId)
        {
            bool doContainKey = debstorrs.ContainsKey(vehicleId);
            return doContainKey && Debtors[vehicleId] > 0;
        }

        public void RegistrateVehicle(string name, decimal price)
        {
            prices.Add(name, price);
        }

        public void UnRegistrateVehicle(string name)
        {
            prices.Remove(name);
        }

    }
}
