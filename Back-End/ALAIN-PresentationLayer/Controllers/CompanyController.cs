using ALAIN_Business;
using ALAIN_DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RC_API.Controllers
{
    [Route("api/Company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [HttpGet("")]
        public ActionResult<ALAIN_DataAccess.Model.Company> GetCompanyInfo()
        {
            ALAIN_DataAccess.Model.Company? company = ALAIN_Business.Company.GetCompanyInfo();
            if (company == null)
                return NotFound();

            return Ok(company);
        }
        [HttpPut("CompanyID/{CompanyID}")]
        public ActionResult<bool> SaveCompanySettings(int CompanyID, [FromBody] ALAIN_DataAccess.Model.Company company)
        {
            if (CompanyID < 1 || company.Id != CompanyID)
                return BadRequest();

            return Ok(ALAIN_Business.Company.SaveCompanySettings(company));
        }
    }
}