using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SensorApis.Models;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;

namespace SensorApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input data", Errors = ModelState });
            }

            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully" });
            }

            // Collect and return detailed error messages
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new { Message = "User registration failed", Errors = errorMessages });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input data", Errors = ModelState });
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Login successful" });
            }

            return Unauthorized(new { Message = "Login failed", Errors = "Invalid username or password" });
        }
    }
}

