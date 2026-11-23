using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC_API.Global_Classes;

namespace RC_API.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<List<ALAIN_DataAccess.Model.Customer>> GetAllCustomers()
        {
            List<ALAIN_DataAccess.Model.Customer> customers = ALAIN_Business.Customer.GetAllCutomers();
            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }
        [HttpGet("Nationality/{Nationality}")]
        public ActionResult<List<ALAIN_DataAccess.Model.Customer>> FilterCustomersByNationality(string Nationality)
        {
            if (string.IsNullOrWhiteSpace(Nationality))
                return BadRequest();

            List<ALAIN_DataAccess.Model.Customer> customers = ALAIN_Business.Customer.FilterCustomersByNationality(Nationality);
            if(customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }
        [HttpGet("Name/{Name}")]
        public ActionResult<List<ALAIN_DataAccess.Model.Customer>> FilterCustomersByCustomerName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return BadRequest();

            List<ALAIN_DataAccess.Model.Customer> customers = ALAIN_Business.Customer.FilterCustomersByCustomerName(Name);
            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }
        [HttpGet("NationalNumber/{NationalNumber}")]
        public ActionResult<List<ALAIN_DataAccess.Model.Customer>> FilterCustomersByNationalNumber(string NationalNumber)
        {
            if (string.IsNullOrWhiteSpace(NationalNumber))
                return BadRequest();

            List<ALAIN_DataAccess.Model.Customer> customers = ALAIN_Business.Customer.FilterCustomersByNationalNumber(NationalNumber);
            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }
        [HttpGet("Phone/{Phone}")]
        public ActionResult<List<ALAIN_DataAccess.Model.Customer>> FilterCustomersByPhoneNumber(string Phone)
        {
            if (string.IsNullOrWhiteSpace(Phone))
                return BadRequest();

            List<ALAIN_DataAccess.Model.Customer> customers = ALAIN_Business.Customer.FilterCustomersByPhoneNumber(Phone);
            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }
        [HttpGet("Find/NationalNumber/{NationalNumber}")]
        public ActionResult<ALAIN_DataAccess.Model.Customer> GetCustomerInfoByNationalNumber(string NationalNumber)
        {
            if (string.IsNullOrWhiteSpace(NationalNumber))
                return BadRequest();

            ALAIN_DataAccess.Model.Customer? customer = ALAIN_Business.Customer.GetCustomerInfoByNationalNumber(NationalNumber);
            if (customer != null)
                return Ok(customer);

            return NotFound();
        }
        [HttpGet("Find/CustomerID/{CustomerID}")]
        public ActionResult<ALAIN_DataAccess.Model.Customer> GetCustomerInfoByCustomerID(int CustomerID)
        {
            if (CustomerID < 1)
                return BadRequest();

            ALAIN_DataAccess.Model.Customer? customer = ALAIN_Business.Customer.GetCustomerInfoByCustomerID(CustomerID);
            if (customer != null)
                return Ok(customer);

            return NotFound();
        }
        [HttpGet("Count/Year/{Year}/Month/{Month}")]
        public ActionResult<int> CountCustomerNewInMonth(int Year, int Month)
        {
            if (Month < 1 || Month > 12 || Year < 2000 || Year > DateTime.Now.Year)
                return BadRequest();

            return Ok(ALAIN_Business.Customer.CountCustomerNew(Year, Month));
        }
        [HttpGet("Count")]
        public ActionResult<int> CountTotalCustomers()
        {
            return Ok(ALAIN_Business.Customer.CountTotalCustomers());
        }
        [HttpPost]
        public ActionResult<int> AddNewCustomer([FromBody]ALAIN_DataAccess.Model.Customer customer)
        {
            if (!Validation.ValidateCustomer(customer))
                return BadRequest();

            int? Id = ALAIN_Business.Customer.AddNewCustomer(customer);
            if (Id != null && Id > 0)
                return Ok(Id);

            return NotFound();
        }
        [HttpPut("{Id}")]
        public ActionResult<ALAIN_DataAccess.Model.Customer> EditCustomer(int Id,
            [FromBody] ALAIN_DataAccess.Model.Customer customer)
        {
            if (Id < 1 || !Validation.ValidateCustomer(customer))
                return BadRequest();

            if (ALAIN_Business.Customer.EditCustomer(customer))
                return Ok(customer);
            
            return NotFound();
        }
        [HttpDelete("{NationalNumber}")]
        public ActionResult DeleteCustomer(string NationalNumber)
        {
            if (string.IsNullOrWhiteSpace(NationalNumber))
                return BadRequest();

            int CountCustomers = ALAIN_Business.Customer.DeleteCustomer(NationalNumber);
            if (CountCustomers > -1)
                return Ok(CountCustomers);

            return NotFound();
        }
        [HttpGet("Check/Exist/NationalNumber/{NationalNo}")]
        public ActionResult<bool> CheckIfNationalNumberAlreadyExist(string NationalNo)
        {
            if (string.IsNullOrWhiteSpace(NationalNo))
                return BadRequest();

            return Ok(ALAIN_Business.Customer.CheckIfNationalNumberAlreadyExist(NationalNo));
        }
    }
}