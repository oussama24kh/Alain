using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC_API.Global_Classes;

namespace RC_API.Controllers
{
    [Route("api/Rental")]
    [ApiController]
    [Authorize]
    public class RentalController : ControllerBase
    {
        [HttpGet("Count/Status/{Status}")]
        public ActionResult<int> CountRentalsByStatus(string Status)
        {
            if (string.IsNullOrWhiteSpace(Status))
                return BadRequest();

            return Ok(ALAIN_Business.Rental.CountRentalsByStatus(Status));
        }
        [HttpGet("Count/Status/{Status}/Year/{Year}/Month/{Month}")]
        public ActionResult<int> CountRentalsByStatus(string Status, int Year, int Month)
        {
            if (string.IsNullOrWhiteSpace(Status) || Year < 1950 || Year > DateTime.Now.Year || Month < 1 || Month > 12)
                return BadRequest();

            return Ok(ALAIN_Business.Rental.CountRentalsByStatus(Status, Year, Month));
        }
        [HttpGet("Count/Active")]
        public ActionResult<int> CountActiveRentals()
        {
            return Ok(ALAIN_Business.Rental.CountActiveRentals());
        }
        [HttpGet("Count/Completed/Year/{Year}/Month/{Month}")]
        public ActionResult<int> CountCompletedRentals(int Year, int Month)
        {
            if (Year < 1950 || Year > DateTime.Now.Year || Month < 1 || Month > 12)
                return BadRequest();

            return Ok(ALAIN_Business.Rental.CountCompletedRentals(Year, Month));
        }
        [HttpGet("All")]
        public ActionResult<List<ALAIN_DataAccess.DTO.Rental_DTO>> GetAllRentals()
        {
            List<ALAIN_DataAccess.DTO.Rental_DTO> rentals = ALAIN_Business.Rental.GetAllRentals();
            if (rentals.Count == 0)
                return NotFound();

            return Ok(rentals);
        }
        [HttpGet("Status/{RentalStatus}")]
        public ActionResult<List<ALAIN_DataAccess.DTO.Rental_DTO>> FilterRentalsByRentalStatus(string RentalStatus)
        {
            if (string.IsNullOrWhiteSpace(RentalStatus))
                return BadRequest();

            List<ALAIN_DataAccess.DTO.Rental_DTO> rentals = ALAIN_Business.Rental.FilterRentalsByRentalStatus(RentalStatus);
            if (rentals.Count == 0)
                return NotFound();

            return Ok(rentals);
        }
        [HttpGet("Code/{RentalCode}")]
        public ActionResult<List<ALAIN_DataAccess.DTO.Rental_DTO>> FilterRentalsByRentalCode(string RentalCode)
        {
            if (string.IsNullOrWhiteSpace(RentalCode))
                return BadRequest();

            List<ALAIN_DataAccess.DTO.Rental_DTO> rentals = ALAIN_Business.Rental.FilterRentalsByRentalCode(RentalCode);
            if (rentals.Count == 0)
                return NotFound();

            return Ok(rentals);
        }
        [HttpGet("PlateNumber/{PlateNumber}")]
        public ActionResult<List<ALAIN_DataAccess.DTO.Rental_DTO>> FilterRentalsByPlateNumber(string PlateNumber)
        {
            if (string.IsNullOrWhiteSpace(PlateNumber))
                return BadRequest();

            List<ALAIN_DataAccess.DTO.Rental_DTO> rentals = ALAIN_Business.Rental.FilterRentalsByPlateNumber(PlateNumber);
            if (rentals.Count == 0)
                return NotFound();

            return Ok(rentals);
        }
        [HttpGet("NationalNumber/{NationalNumber}")]
        public ActionResult<List<ALAIN_DataAccess.DTO.Rental_DTO>> FilterRentalsByNationalNumber(string NationalNumber)
        {
            if (string.IsNullOrWhiteSpace(NationalNumber))
                return BadRequest();

            List<ALAIN_DataAccess.DTO.Rental_DTO> rentals = ALAIN_Business.Rental.FilterRentalsByNationalNumber(NationalNumber);
            if (rentals.Count == 0)
                return NotFound();

            return Ok(rentals);
        }
        //NumberOfRentals between 3 and 10
        [HttpGet("Latest/{NumberOfRentals}")]
        public ActionResult<List<ALAIN_DataAccess.DTO.Rental_DTO>> GetLatestRentals(int NumberOfRentals)
        {
            if (NumberOfRentals < 3 || NumberOfRentals > 10)
                return BadRequest();

            List<ALAIN_DataAccess.DTO.Rental_DTO> rentals = ALAIN_Business.Rental.GetLatestRentals(NumberOfRentals);
            if (rentals.Count == 0)
                return NotFound();

            return Ok(rentals);
        }
        [HttpGet("Find/RentalID/{RentalID}")]
        public ActionResult<ALAIN_DataAccess.Model.Rental> GetRentalInfoByRentalID(int RentalID)
        {
            if (RentalID < 1)
                return BadRequest();

            ALAIN_DataAccess.Model.Rental? rental = ALAIN_Business.Rental.GetRentalInfoByRentalID(RentalID);
            if (rental != null)
                return Ok(rental);

            return NotFound(rental);
        }
        [HttpGet("Find/RentalCode/{RentalCode}")]
        public ActionResult<ALAIN_DataAccess.Model.Rental> GetRentalInfoByRentalCode(string RentalCode)
        {
            if (string.IsNullOrWhiteSpace(RentalCode))
                return BadRequest();

            ALAIN_DataAccess.Model.Rental? rental = ALAIN_Business.Rental.GetRentalInfoByRentalCode(RentalCode);
            if (rental != null)
                return Ok(rental);

            return NotFound(rental);
        }
        [HttpPost()]
        public ActionResult<ALAIN_DataAccess.Model.Rental> AddNewRental([FromBody] ALAIN_DataAccess.Model.Rental? rental)
        {
            if (rental == null || !Validation.ValidateRental(rental))
                return BadRequest();

            rental = ALAIN_Business.Rental.AddNewRental(rental);
            if (rental != null)
                return Ok(rental);

            return NotFound();
        }
        [HttpPut("{Id}")]
        public ActionResult<ALAIN_DataAccess.Model.Rental> EditRental(int Id, [FromBody] ALAIN_DataAccess.Model.Rental rental)
        {
            if (Id < 1)
                return BadRequest();

            if (ALAIN_Business.Rental.EditRental(rental))
                return Ok(rental);

            return NotFound();
        }
        [HttpGet("RentalDetails/Find/RentalCode/{RentalCode}")]
        public ActionResult<ALAIN_DataAccess.DTO.RentalDetails_DTO> GetRentalDetailsByRentalCode(string RentalCode)
        {
            if (string.IsNullOrWhiteSpace(RentalCode))
                return BadRequest();

            ALAIN_DataAccess.DTO.RentalDetails_DTO? rental = ALAIN_Business.Rental.GetRentalDetailsByRentalCode(RentalCode);
            if (rental != null)
                return Ok(rental);

            return BadRequest();
        }
        [HttpGet("Count/Overdue")]
        public ActionResult<int> CountOverdueRentals()
        {
            return Ok(ALAIN_Business.Rental.CountOverdueRentals());
        }
        [HttpGet("Duration/RentalCode/{RentalCode}")]
        public ActionResult<int> GetRentalDuration(string RentalCode)
        {
            if (string.IsNullOrWhiteSpace(RentalCode))
                return BadRequest();

            int duration = ALAIN_Business.Rental.GetRentalDuration(RentalCode);
            if(duration >= 0) 
                return Ok(duration);

            return BadRequest();
        }
    }
}