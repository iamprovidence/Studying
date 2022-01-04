using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace ParkingWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TransactionController : ControllerBase
    {
        // FIELDS
        ParkingSystem.Parking parking;

        // CONSTRUCTORS
        public TransactionController(ParkingSystem.Parking parking)
        {
            this.parking = parking;
        }

        // ACTIONS

        // GET: api/transaction/last
        [HttpGet]
        [Route("last")]
        public IEnumerable<ParkingSystem.Models.Transaction> Get()
        {
            return parking.TransactionService.Transactions;
        }
        // GET: api/transaction/all
        [HttpGet]
        [Route("all")]
        public IEnumerable<string> GetAll()
        {
            return parking.TransactionService.GetRawTransactionFromFile(ParkingSystem.Core.Configurations.TRANSACTION_LOG_FILE);
        }
    }
}
