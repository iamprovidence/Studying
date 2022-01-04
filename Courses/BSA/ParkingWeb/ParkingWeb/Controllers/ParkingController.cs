using Microsoft.AspNetCore.Mvc;

namespace ParkingWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ParkingController : ControllerBase
    {
        // FIELDS
        ParkingSystem.Parking parking;

        // CONSTRUCTORS
        public ParkingController(ParkingSystem.Parking parking)
        {
            this.parking = parking;
        }

        // ACTIONS

        // GET: api/parking/earned_money/
        [HttpGet]
        [Route("earned_money")]
        public ActionResult<decimal> GetEarnedMoney()
        {
            return parking.TransactionService.EarnedMoney();
        }
        // GET: api/parking/balance/
        [HttpGet]
        [Route("balance")]
        public ActionResult<decimal> GetBalance()
        {
            return parking.Balance;
        }

        // GET: api/parking/capacity/
        [HttpGet]
        [Route("capacity")]
        public ActionResult<int> GetCapacity()
        {
            return parking.ParkingPlace.ParkingCapacity;
        }

        // GET: api/parking/free_spaces/
        [HttpGet]
        [Route("free_spaces")]
        public ActionResult<int> GetFreePlacesAmount()
        {
            return parking.ParkingPlace.FreeSpacesAmount;
        }

        // GET: api/parking/busy_spaces/
        [HttpGet]
        [Route("busy_spaces")]
        public ActionResult<int> GetBusyPlacesAmount()
        {
            return parking.ParkingPlace.BusySpacesAmount;
        }
        
    }
}
