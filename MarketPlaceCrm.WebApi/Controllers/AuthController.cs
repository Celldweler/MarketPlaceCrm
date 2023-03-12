using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using MarketPlaceCrm.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("auth")]
    public class AuthController : ApiBaseController
    {
        private readonly JwtManager _jwtManager;

        public AuthController(JwtManager jwtManager)
        {
            _jwtManager = jwtManager;
        }

        [HttpGet]
        [Authorize(Policy = Constants.AdminPolicy)]
        public string Secret()
        {
            var identity = HttpContext.User;
            var claim = identity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            
            return (claim != null) ? claim.Value : "unknown";   
        }

        public class UserForm
        {
            [Required] public string UserName { get; set; } = "admin";

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "admin";
        }
        
        [HttpPost]
        public IActionResult Login([FromForm]UserForm userFormCredentials)
        {
            // if (userFormCredentials.UserName == "admin" && userFormCredentials.Password == "admin" 
            //                                             && CheckForNull(userFormCredentials))
            // {
            // }
            var token = _jwtManager.GenerateToken(userFormCredentials.UserName);
            if (token == "user not found")
                return BadRequest("user not found");

            return Ok(token);
        }
    }
}