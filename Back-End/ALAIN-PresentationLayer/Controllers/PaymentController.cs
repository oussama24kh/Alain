using ALAIN_DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC_API.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [Attributes.AdminOnly]
        [HttpGet("Find/RentalID/{rentalID}")]
        public ActionResult<ALAIN_DataAccess.Model.Payment> GetPaymentInfoByRentalID(int rentalID)
        {
            if (rentalID < 1)
                return BadRequest();

            ALAIN_DataAccess.Model.Payment? payment = ALAIN_Business.Payment.GetPaymentInfoByRentalID(rentalID);
            if (payment != null)
                return Ok(payment);

            return NotFound();
        }
        [HttpPost()]
        public ActionResult<int> AddNewPayment(ALAIN_DataAccess.Model.Payment payment)
        {
            int? Id = ALAIN_Business.Payment.AddNewPayment(payment);
            if (Id != null && Id > 0)
                return Ok(Id);

            return NotFound();
        }
        [HttpPut("{Id}")]
        public ActionResult<ALAIN_DataAccess.Model.Payment> EditPayment(int Id, [FromBody] ALAIN_DataAccess.Model.Payment payment)
        {
            if (Id < 1)
                return BadRequest();
            
            if (ALAIN_Business.Payment.EditPayment(payment))
                return Ok(payment);

            return NotFound();
        }
        [HttpGet("TotalAmount/{RentalID}")]
        public ActionResult<decimal> GetTotalPrice(int RentalID)
        {
            if (RentalID < 1)
                return BadRequest();

            decimal? total = ALAIN_Business.Payment.GetTotalPrice(RentalID);
            if (total == null || total <= 0)
                return NotFound();

            return Ok(total);
        }
        [Authorize]
        [HttpGet("Revenue/Year/{Year}/Month/{Month}")]
        public ActionResult<decimal> CountRevenue(int Year, int Month)
        {
            if (Year < 2000 || Year > DateTime.Now.Year || Month < 1 || Month > 12)
                return BadRequest();

            return Ok(ALAIN_Business.Payment.CountRevenue(Year, Month));
        }
        [HttpGet("Revenue/Year/{Year}/Month/{Month}/Day/{Day}")]
        public ActionResult<decimal> CountRevenue(int Year, int Month,int Day)
        {
            if (Year < 2000 || Year > DateTime.Now.Year || Month < 1 || Month > 12 || Day < 1 || Day > 31)
                return BadRequest();

            return Ok(ALAIN_Business.Payment.CountRevenue(Year, Month, Day));
        }
        [HttpGet("Revenue/Total")]
        public ActionResult<decimal> CountRevenue()
        {
            return Ok(ALAIN_Business.Payment.CountRevenue());
        }
        [HttpGet("Revenue/CurrentWeek")]
        public ActionResult<decimal> CountRevenueCurrentWeek()
        {
            return Ok(ALAIN_Business.Payment.CountRevenueCurrentWeek());
        }
        [HttpGet("Count/Year/{Year}/Month/{Month}")]
        public ActionResult<int> CountPayment(int Year, int Month)
        {
            if (Year < 2000 || Year > DateTime.Now.Year || Month < 1 || Month > 12)
                return BadRequest();

            return Ok(ALAIN_Business.Payment.CountPayment(Year,Month));
        }
        [HttpGet("All")]
        public ActionResult<List<Payment_DTO>> GetAllPayments()
        {
            List<Payment_DTO> payments = ALAIN_Business.Payment.GetAllPayments();
            if (payments.Count == 0)
                return NotFound();

            return Ok(payments);
        }
        [HttpGet("RentalCode/{RentalCode}")]
        public ActionResult<List<Payment_DTO>> FilterPaymentByRentalCode(string RentalCode)
        {
            if (string.IsNullOrWhiteSpace(RentalCode))
                return BadRequest();

            List<Payment_DTO>? payments = ALAIN_Business.Payment.FilterPaymentByRentalCode(RentalCode);
            if (payments.Count == 0)
                return NotFound();

            return Ok(payments);
        }
        [HttpGet("Revenue/RentalCode/{RentalCode}")]
        public ActionResult<decimal> GetTotalRevenueByRentalCode(string RentalCode)
        {
            if (string.IsNullOrWhiteSpace(RentalCode))
                return BadRequest();

            return Ok(ALAIN_Business.Payment.GetTotalRevenueByRentalCode(RentalCode));
        }
        [HttpGet("PlateNumber/{PlateNumber}")]
        public ActionResult<List<Payment_DTO>> FilterPaymentByPlateNumber(string PlateNumber)
        {
            if (string.IsNullOrWhiteSpace(PlateNumber))
                return BadRequest();

            List<Payment_DTO>? payments = ALAIN_Business.Payment.FilterPaymentByPlateNumber(PlateNumber);
            if (payments.Count == 0)
                return NotFound();

            return Ok(payments);
        }
        [HttpGet("Revenue/PlateNumber/{PlateNumber}")]
        public ActionResult<decimal> GetTotalRevenueByPlateNumber(string PlateNumber)
        {
            if (string.IsNullOrWhiteSpace(PlateNumber))
                return BadRequest();

            return Ok(ALAIN_Business.Payment.GetTotalRevenueByPlateNumber(PlateNumber));
        }
        [HttpGet("NationalNumber/{NationalNumber}")]
        public ActionResult<List<Payment_DTO>> FilterPaymentByNationalNumber(string NationalNumber)
        {
            if (string.IsNullOrWhiteSpace(NationalNumber))
                return BadRequest();

            List<Payment_DTO>? payments = ALAIN_Business.Payment.FilterPaymentByNationalNumber(NationalNumber);
            if (payments.Count == 0)
                return NotFound();

            return Ok(payments);
        }
        [HttpGet("Revenue/NationalNumber/{NationalNumber}")]
        public ActionResult<decimal> GetTotalRevenueByNationalNumber(string NationalNumber)
        {
            if (string.IsNullOrWhiteSpace(NationalNumber))
                return BadRequest();

            return Ok(ALAIN_Business.Payment.GetTotalRevenueByNationalNumber(NationalNumber));
        }
        [HttpGet("Month/{Month}")]
        public ActionResult<List<Payment_DTO>> FilterPaymentByMonth(int Month)
        {
            if (Month < 1 || Month > 12)
                return BadRequest();

            return Ok(ALAIN_Business.Payment.FilterPaymentByMonth(Month));
        }
        [HttpGet("Revenue/Month/{Month}")]
        public ActionResult<decimal> GetTotalRevenueByMonth(int Month)
        {
            if (Month < 1 || Month > 12)
                return BadRequest();

            return Ok(ALAIN_Business.Payment.GetTotalRevenueByMonth(Month));
        }
        [HttpGet("Year/{Year}/Month/{Month}")]
        public ActionResult<List<Payment_DTO>> FilterPaymentsByPeriod(int Year, int Month)
        {
            if (Year < 2000 || Year > DateTime.Now.Year || Month < 1 || Month > 12)
                return BadRequest();

            List<Payment_DTO> payments = ALAIN_Business.Payment.FilterPaymentsByPeriod(Year, Month);
            if (payments.Count == 0)
                return NotFound();
            
            return Ok(payments);
        }
        [HttpGet("Years")]
        public ActionResult<List<int>> GetPaymentYears()
        {
            List<int> years = ALAIN_Business.Payment.GetPaymentYears();
            if (years.Count == 0)
                return NotFound();
            
            return Ok(years);
        }
    }
}