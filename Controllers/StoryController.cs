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
    public class StoryController : ControllerBase
    {
        private readonly ServiceStory _serviceStory;
        private readonly DetailService _serviceDetail;
        private readonly ServiceComent _serComment;
        public StoryController(ServiceStory serviceStory, DetailService serviceDetail, ServiceComent serComment)
        {
            _serviceStory = serviceStory;
            _serviceDetail = serviceDetail;
            _serComment = serComment;
        }

        [HttpPost("inserts")]
        [Authorize]
        public async Task<IActionResult> inSert([FromBody] formStory st)
        {
            try
            {
                var sto = await _serviceStory.InsertIntoStory(st);
                if (sto == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(new { data = sto });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("getAll/")]
        public async Task<IActionResult> getAllStory([FromQuery] string? id = null, [FromQuery] char? shortChar = null)
        {
            try
            {
                int? ids = null;
                if (!string.IsNullOrEmpty(id))
                {
                    ids = int.Parse(id);
                }
                if (shortChar.HasValue && !shortChar.Value.ToString().Equals(""))
                {
                    shortChar = 'a';
                }
                var allStory = await _serviceStory.GetStoryAsync(ids, shortChar);
                return Ok(new { Data = allStory });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("get-top-view/{limit}/{type}")]
        public async Task<IActionResult> getTopStory(string limit, string type)
        {
            try
            {
                int li = int.Parse(limit);
                var allStory = await _serviceStory.GetTopStoriesByViewsAsync(li, type);
                return Ok(new { Data = allStory });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("find-Story/{search}")]
        public async Task<IActionResult> findStory(string search)
        {
            try
            {
                var allStory = await _serviceStory.FindStorys(search);
                return Ok(new { Data = allStory });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("filter-story/{search}")]
        public async Task<IActionResult> filterStory(string search)
        {
            try
            {
                var allStory = await _serviceStory.FilterStorys(search);
                return Ok(new { Data = allStory });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("top-by-comments")]
        public async Task<IActionResult> GetTopStoriesByComments()
        {
            var result = await _serviceStory.GetStoriesOrderedByCommentCountAsync();
            return Ok(new { data = result});
        }

        [HttpDelete("delete-story/{id}")]
        [Authorize]
        public async Task<IActionResult> deleteStory(string id)
        {
            try
            {
                int ids = int.Parse(id);
                var all = await _serviceDetail.getAllDetailID(ids);
                foreach (var x in all)
                {
                    var allC = await _serComment.deleteForIdDS(x.DetailID);
                }
                var allDStory = await _serviceDetail.deleteAllForIdS(ids);
                var allStory = await _serviceStory.deleteForID(ids);
                return Ok(new { Data = allStory && allDStory ? "Xóa Thành công" : allDStory ? "Chỉ xóa được các chương" : "Xóa thất bại" });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut("update-story")]
        [Authorize]
        public async Task<IActionResult> updateNoS([FromBody] formUpdateStory fus)
        {
            try
            {
                var data = await _serviceStory.updateStory(fus);
                if (data == null)
                {
                    return BadRequest(new { Data = "Quá trình không được thực thi" });
                }
                else
                {
                    return Ok(new { Data = data });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("get-type")]
        public async Task<IActionResult> getType()
        {
            try
            {
                var allStory = await _serviceStory.getTypeDetailStory();
                return allStory != null ? Ok(new { Data = allStory }) : BadRequest(new { Data = "Không có dữ liệu" });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}