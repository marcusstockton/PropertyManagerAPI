using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Auth;
using PropertyManagerApi.Services;

namespace PropertyManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, ILogger<AuthController> logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// End point for authenticating individual user account.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /authenticate
        ///     {
        ///        "username": "testUser",
        ///        "password": "Pa$$w0rd"
        ///     }
        ///
        /// </remarks>
        /// <param name="userParam"></param>
        /// <returns>User object with auth token.</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto userParam)
        {
            var validUser = await _userService.ValidateUser(userParam);
            _logger.LogInformation($"User Id {validUser.Id} found. Generating an auth token");
            if (validUser == null)
            {
                return BadRequest();
            }
            var user_token = _userService.Authenticate(validUser);

            var userResult = _mapper.Map<UserResponse>(user_token);

            _logger.LogInformation($"Token generated at {DateTime.Now}.");
            return Ok(userResult);
        }


        /// <summary>
        /// Register a new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /register
        ///     {
        ///        "username": "testUser",
        ///        "password": "password",
        ///        "firstName": "John",
        ///        "lastname": "Doe",
        ///        "dob": "1 mar 1998"
        ///     }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto user)
        {
            var result = await _userService.RegisterUser(user);
            _logger.LogInformation($"User registered sucessfully");
            if (result.Succeeded)
            {
                return Ok();
            }
            _logger.LogError($"An Error occured registering a new user");
            return (BadRequest(result.Errors));

        }

        /// <summary>
        /// Gets all users who don't have a first name set
        /// </summary>
        /// <returns>A IEnumerable of ApplicationUsers without a first name.</returns>
        [HttpGet("GetAllUsersWithoutFirstName")]
        public IActionResult GetAllUsersWithoutFirstName()
        {
            var users = _userService.GetAllUsersWithoutFirstName();
            return Ok(users);
        }

        /// <summary>
        /// Gets the logged in user record
        /// </summary>
        /// <returns>ApplicationUser record</returns>
        [HttpGet("GetLoggedInUserDetails")]
        public IActionResult GetLoggedInUserDetails()
        {
            var user_id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUserById(user_id);
            return Ok(user);
        }
    }
}
