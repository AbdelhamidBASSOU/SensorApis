using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SensorApis.Models;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using System.Threading.Tasks;

namespace SensorApis.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMemoryCache _cache;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMemoryCache cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cache = cache;
        }

        // v1.0: POST api/v1/Account/register
        [HttpPost("register"), MapToApiVersion("1.0")]
        public async Task<IActionResult> RegisterV1([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input data", Errors = ModelState });
            }

            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully (v1)" });
            }

            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new { Message = "User registration failed (v1)", Errors = errorMessages });
        }

        // v2.0: POST api/v2/Account/register
        [HttpPost("register"), MapToApiVersion("2.0")]
        public async Task<IActionResult> RegisterV2([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input data", Errors = ModelState });
            }

            var user = new User { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully (v2)" });
            }

            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new { Message = "User registration failed (v2)", Errors = errorMessages });
        }

        // v1.0: POST api/v1/Account/login
        [HttpPost("login"), MapToApiVersion("1.0")]
        public async Task<IActionResult> LoginV1([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input data", Errors = ModelState });
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Login successful (v1)" });
            }

            return Unauthorized(new { Message = "Login failed", Errors = "Invalid username or password" });
        }

        // v2.0: POST api/v2/Account/login
        [HttpPost("login"), MapToApiVersion("2.0")]
        public async Task<IActionResult> LoginV2([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input data", Errors = ModelState });
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Login successful (v2)", Timestamp = DateTime.UtcNow });
            }

            return Unauthorized(new { Message = "Login failed", Errors = "Invalid username or password" });
        }
    }
}
