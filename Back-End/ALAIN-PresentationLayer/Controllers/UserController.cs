using ALAIN_Business;
using ALAIN_DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC_API.Controllers
{
    [Route("api/User")]
    [ApiController]
    [Authorize(Roles = "Administrateur")]
    public class UserController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<User_DTO> GetAllUsers()
        {
            List<User_DTO> users = ALAIN_Business.User.GetAllUsers();
            if (users.Count == 0)
                return NotFound();

            return Ok(users);
        }
        [HttpGet("Statistics")]
        public ActionResult<Dictionary<string,int>> GetUserStatistics()
        {
            Dictionary<string, int>? statistics = ALAIN_Business.User.GetUserStatistics();
            if (statistics == null || statistics.Count == 0)
                return NotFound();

            return Ok(statistics);
        }
        [HttpPost]
        public ActionResult<int> AddNewUser([FromBody] User_DTO user)
        {
            int? Id = ALAIN_Business.User.AddNewUser(user);
            if (Id.HasValue && Id.Value > 0)
                return Ok(Id.Value);

            return NotFound();
        }
        [HttpPut("{userId}")]
        public ActionResult<User_DTO> EditUser(int userId, [FromBody] User_DTO user)
        {
            if (userId < 1)
                return BadRequest();
            
            if(ALAIN_Business.User.EditUser(user))
                return Ok(user);
            
            return NotFound();
        }
        [HttpGet("Check/Exist/Username/{Username}")]
        public ActionResult<bool> CheckIfUsernameExist(string Username)
        {
            if (string.IsNullOrWhiteSpace(Username))
                return BadRequest();

            return Ok(ALAIN_Business.User.CheckIfUsernameExist(Username));
        }
        [HttpGet("Find/Username/{Username}")]
        public ActionResult<User_DTO> GetUserInfoByUsername(string Username)
        {
            if (string.IsNullOrWhiteSpace(Username))
                return BadRequest();

            User_DTO? user = ALAIN_Business.User.GetUserInfoByUsername(Username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpGet("Find/UserID/{UserID}")]
        public ActionResult<User_DTO> GetUserInfoByUserID(int UserID)
        {
            if (UserID < 1)
                return BadRequest();

            User_DTO? user = ALAIN_Business.User.GetUserInfoByUserID(UserID);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpPut("UserName/{UserName}/Status/{Status}")]
        public ActionResult<bool> ChangeUserStatus(string UserName, bool Status, [FromBody] string User)
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(User) || UserName != User)
                return BadRequest();

            return Ok(ALAIN_Business.User.ChangeUserStatus(User, Status));
        }
    }
}