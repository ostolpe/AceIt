using System.Security.Claims;
using AceIt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AceIt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController(IProfileService service) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProfileData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return NotFound();

            var res = await service.GetProfileDataAsync(userId);

            return Ok(res);
        }
    }
}
