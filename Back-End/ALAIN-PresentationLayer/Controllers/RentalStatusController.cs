using ALAIN_Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC_API.Controllers
{
    [Route("api/RentalStatus")]
    [ApiController]
    [Authorize]
    public class RentalStatusController : ControllerBase
    {
        [HttpGet("Active")]
        public ActionResult<List<string>> GetActiveStatus()
        {
            List<string> Rentalstatus = RentalStatus.GetActiveStatus();
            if (Rentalstatus.Count == 0)
                return NotFound();

            return Ok(Rentalstatus);
        }
    }
}