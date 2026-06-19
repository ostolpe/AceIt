using AceIt.DTOs;
using AceIt.Models;
using AceIt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AceIt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var user = new User
            {
                Email = req.Email,
                UserName = req.Email,
            };
            var res = await userManager.CreateAsync(user, req.Password);
            if (!res.Succeeded)
                return BadRequest(res.Errors);
            var token = jwtService.GenerateToken(user);
            return StatusCode(201, new LoginResponse(token));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is null) return Unauthorized();
            var res = await signInManager.CheckPasswordSignInAsync(user, req.Password, true);
            if (!res.Succeeded)
                return Unauthorized();

            var token = jwtService.GenerateToken(user);
            return Ok(new LoginResponse(token));
        }
    }
}
