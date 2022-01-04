using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace ParkingWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class VehicleController : ControllerBase
    {
        // FIELDS
        ParkingSystem.Parking parking;

        // CONSTRUCTOR
        public VehicleController(ParkingSystem.Parking parking)
        {
            this.parking = parking;
        }

        // ACTIONS
        // GET: api/vehicle/
        [HttpGet]
        public IList<ParkingSystem.Vehicle.VehicleBase> Get()
        {
            return parking.ParkingPlace.Vehicles;
        }

        // GET: api/vehicle/types
        [HttpGet]
        [Route("types")]
        public ActionResult<Dictionary<int, string>> VehicleTypes()
        {
            return new Dictionary<int, string>()
            {
                [0] = "Bus",
                [1] = "Motorcycle",
                [2] = "Passenger car",
                [3] = "Truck"
            };
        }

        // PATCH: api/vehicle/refill_balance/?vehicleId={vehicleId}&money={moneyInput}
        [HttpPatch("vehicleId, money")]
        [Route("refill_balance")]
        public ActionResult RefillBalance(int vehicleId, decimal money)
        {
            ParkingSystem.Vehicle.VehicleBase vehicle = parking.ParkingPlace.GetVehicleById(vehicleId);
            if (vehicle == null) return BadRequest();

            vehicle.Balance += money;
            return Ok();
        }

        // PUT: api/vehicle/1
        [HttpPut("{type}")]
        public ActionResult ParkVehicle(int type)
        {
            return parking.ParkingPlace.AddCar(CreateVehicle(type)) ? (ActionResult)Ok() : (ActionResult)BadRequest();
        }

        private ParkingSystem.Vehicle.VehicleBase CreateVehicle(int vehicleType)
        {
            switch (vehicleType)
            {
                case 0: return new ParkingSystem.Vehicle.Bus();
                case 1: return new ParkingSystem.Vehicle.Motorcycle();
                case 2: return new ParkingSystem.Vehicle.PassengerCar();
                case 3: return new ParkingSystem.Vehicle.Truck();

                default: throw new System.ArgumentException("Wrong vehicle type");
            }
        }

        // DELETE: api/vehicle/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return parking.ParkingPlace.DeleteCar(id) ? (ActionResult)Ok() : (ActionResult)BadRequest();
        }
    }
}
