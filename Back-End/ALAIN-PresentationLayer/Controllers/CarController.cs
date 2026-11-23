using ALAIN_DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC_API.Global_Classes;

namespace RC_API.Controllers
{
    [Authorize]
    [Route("api/Car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<List<Car_DTO>> GetAllCars()
        {
            List<Car_DTO> cars = ALAIN_Business.Car.GetAllCars();
            if (cars.Count == 0)
                return NotFound();
            
            return Ok(cars);
        }
        [HttpGet("BodyType/{BodyType}")]
        public ActionResult<List<Car_DTO>> FilterCarsByBodyType(string BodyType)
        {
            if (string.IsNullOrWhiteSpace(BodyType))
                return BadRequest();
            
            List<Car_DTO> cars = ALAIN_Business.Car.FilterCarsByBodyType(BodyType);
            if (cars.Count == 0)
                return NotFound();

            return Ok(cars);
        }
        [HttpGet("PlateNumber/{Plate}")]
        public ActionResult<List<Car_DTO>> FilterCarsByPlateNumber(string Plate)
        {
            if (string.IsNullOrWhiteSpace(Plate))
                return BadRequest();

            List<Car_DTO> cars = ALAIN_Business.Car.FilterCarsByPlateNumber(Plate);
            if (cars.Count == 0)
                return NotFound();

            return Ok(cars);
        }
        [HttpGet("Brand/{Brand}")]
        public ActionResult<List<Car_DTO>> FilterCarsByBrand(string Brand)
        {
            if (string.IsNullOrWhiteSpace(Brand))
                return BadRequest();

            List<Car_DTO> cars = ALAIN_Business.Car.FilterCarsByBrand(Brand);
            if(cars.Count == 0)
                return NotFound();

            return Ok(cars);
        }
        [HttpGet("Model/{Model}")]
        public ActionResult<List<Car_DTO>> FilterCarsByModel(string Model)
        {
            if (string.IsNullOrWhiteSpace(Model))
                return BadRequest();

            List<Car_DTO> cars = ALAIN_Business.Car.FilterCarsByModel(Model);
            if (cars.Count == 0)
                return NotFound();

            return Ok(cars);
        }
        [HttpGet("Status/{Status}")]
        public ActionResult<List<Car_DTO>> FilterCarsByStatus(string Status)
        {
            if (string.IsNullOrWhiteSpace(Status))
                return BadRequest();

            List<Car_DTO> cars = ALAIN_Business.Car.FilterCarsByStatus(Status);
            if (cars.Count == 0)
                return NotFound();

            return Ok(cars);
        }
        [HttpGet("Count/Status/{Status}")]
        public ActionResult<int> CountCarsByStatus(string Status)
        {
            if (string.IsNullOrWhiteSpace(Status))
                return BadRequest();

            return Ok(ALAIN_Business.Car.CountCarsByStatus(Status));
        }
        [HttpGet("Count/All")]
        public ActionResult<int> CountAllCars()
        {
            return Ok(ALAIN_Business.Car.CountAllCars());
        }
        [HttpGet("Statistics/All")]
        public ActionResult<Dictionary<string, int>> GetCarStatusCounts()
        {
            Dictionary<string, int> carStatusCounts = ALAIN_Business.Car.GetCarStatusCounts();

            if (carStatusCounts.Count == 0)
                return NotFound();

            return Ok(carStatusCounts);
        }
        [HttpGet("Statistics/BodyType/{BodyType}")]
        public ActionResult<Dictionary<string,int>> GetCarStatusCounts(string BodyType)
        {
            Dictionary<string, int> carStatusCounts = ALAIN_Business.Car.GetCarStatusCounts(BodyType);

            if (carStatusCounts.Count == 0)
                return NotFound();

            return Ok(carStatusCounts);
        }
        [HttpGet("Find/PlateNumber/{PlateNumber}")]
        public ActionResult<ALAIN_DataAccess.Model.Car> GetCarInfoByPlateNumber(string PlateNumber)
        {
            if (string.IsNullOrWhiteSpace(PlateNumber))
                return BadRequest();


            ALAIN_DataAccess.Model.Car? car = ALAIN_Business.Car.GetCarInfoByPlateNumber(PlateNumber);
            if(car == null) 
                return NotFound();

            return Ok(car);
        }
        [HttpGet("Find/CarID/{CarID}")]
        public ActionResult<ALAIN_DataAccess.Model.Car> GetCarInfoByCarID(int CarID)
        {
            if (CarID < 1)
                return BadRequest();

            ALAIN_DataAccess.Model.Car? car = ALAIN_Business.Car.GetCarInfoByCarID(CarID);
            if (car == null)
                return NotFound();

            return Ok(car);
        }
        [HttpDelete("{PlateNumber}")]
        public ActionResult<int> DeleteCar(string PlateNumber)
        {
            if (string.IsNullOrWhiteSpace(PlateNumber))
                return BadRequest();

            int countCars = ALAIN_Business.Car.DeleteCar(PlateNumber);
            if (countCars > -1)
                return Ok(countCars);

            return NotFound($"Car: [{PlateNumber}] not found.");
        }
        [HttpPost()]
        public ActionResult<int> AddNewCar([FromBody] ALAIN_DataAccess.Model.Car car)
        {
            if (!Validation.ValidateCar(car))
                return BadRequest(ModelState);

            int? Id = ALAIN_Business.Car.AddNewCar(car);
            if (Id != null && Id > 0)
                return Ok(Id);

            return NotFound();
        }
        [HttpPut("{CarId}")]
        public ActionResult<ALAIN_DataAccess.Model.Car>EditCar(int CarId,[FromBody] ALAIN_DataAccess.Model.Car car)
        {
            if (CarId < 1 || !Validation.ValidateCar(car))
                return BadRequest();

            if (ALAIN_Business.Car.EditCar(car))
                return Ok(car);

            return NotFound();
        }
        [HttpGet("Check/Exist/PlateNumber/{PlateNumber}")]
        public ActionResult<bool> CheckIfPlateAlreadyExist(string PlateNumber)
        {
            if (string.IsNullOrWhiteSpace(PlateNumber))
                return BadRequest();

            return Ok(ALAIN_Business.Car.CheckIfPlateAlreadyExist(PlateNumber));
        }
    }
}