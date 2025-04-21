using Microsoft.AspNetCore.Mvc;
using server_truyen_v.Services;
using server_truyen_v.Schemas;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace server_truyen_v.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceStotyController : Controller
    {
        private readonly StoryStatisticsService _ssser;
        public ServiceStotyController(StoryStatisticsService ssser)
        {
            _ssser = ssser;
        }

        [Authorize]
        [HttpGet("get-statistic/top-type-view")]
        public async Task<IActionResult> getyop_type_v()
        {
            try{
            var da = await _ssser.GetMostReadGenres();
            return Ok(new {data = da});
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [Authorize]
        [HttpGet("get-statistic/top-view")]
        public async Task<IActionResult> getyop_v()
        {
            try
            {
                var da = await _ssser.GetMostReadStories();
                return Ok(new { data = da });
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        [Authorize]
        [HttpGet("get-statistic/total-type")]
        public async Task<IActionResult> getytotal()
        {
            try
            {
                var da = await _ssser.GetStoryCountByType();
                return Ok(new { data = da });
            }
            catch (Exception e)
            {
                return BadRequest(new { data = e.Message });
            }
        }
    }
}
