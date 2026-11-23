using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC_API.Controllers
{
    [Route("api/Country")]
    [ApiController]
    [Authorize]
    public class CountryController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<List<string>> GetAllCountries()
        {
            List<string> countries = ALAIN_Business.Country.GetAllCountries();
            if (countries.Count == 0)
                return NotFound();

            return Ok(countries);
        }
        [HttpGet("Active")]
        public ActionResult<List<string>> GetActiveCountries()
        {
            List<string> countries = ALAIN_Business.Country.GetActiveCountries();
            if (countries.Count == 0)
                return NotFound();

            return Ok(countries);
        }
    }
}