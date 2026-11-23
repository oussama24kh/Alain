using ALAIN_Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC_API.Controllers
{
    [Authorize]
    [Route("api/Brand")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<List<string>> GetAllBrands()
        {
            List<string> Brands = Brand.GetAllBrands();

            if (Brands.Count == 0)
                return NotFound();

            return Ok(Brands);
        }
    }
}