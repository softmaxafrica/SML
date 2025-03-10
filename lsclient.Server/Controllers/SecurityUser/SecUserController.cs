using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using lsclient.Server.Models; 
using lsclient.Server.Shared;

namespace lsclient.Server.Controllers.SecurityUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecUserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SecUserController(AppDbContext context)
        {
            _context = context;
        }

        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var functionName = nameof(Login);
            var executionResult = new ExecutionResult();

            try
            {
                var secUser = await _context.SecUsers
                    .Where(su => su.Email == loginRequest.Email)
                    .FirstOrDefaultAsync();

                if (secUser == null)
                {
                    executionResult.SetNotFound("User not found.");
                    return NotFound(executionResult.GetServerResponse().Message);
                }

                // Verify password
                if (!Functions.VerifyPassword(loginRequest.Password, secUser.PasswordHash))
                {
                    executionResult.SetUnauthorized();
                    return Unauthorized(executionResult.GetServerResponse().Message);
                }

                //// Generate JWT token or any other authentication token if needed
                //var token = Functions.GenerateJwtToken(secUser.UserID, secUser.Role);

                //var userResponse = new
                //{
                //    UserID = secUser.UserID,
                //    Email = secUser.Email,
                //    Role = secUser.Role,
                //    Token = token
                //};

                executionResult.SetData(secUser);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(SecUserController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetAllUsers
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllUsers);

            try
            {
                var users = _context.SecUsers
                    .ToList();
                executionResult.SetData(users);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(SecUserController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
    }


}
