using ALAIN_Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC_API.Controllers
{
    [Authorize]
    [Route("api/BodyType")]
    [ApiController]
    public class BodyTypeController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<List<string>> GetAllBodyTypes()
        {
            List<string> BodyTypes = BodyType.GetAllBodyTypes();

            if (BodyTypes.Count == 0)
                return NotFound();

            return Ok(BodyTypes);
        }
    }
}