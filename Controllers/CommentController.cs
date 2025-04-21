using Microsoft.AspNetCore.Mvc;
using server_truyen_v.Services;
using server_truyen_v.Schemas;
using server_truyen_v.Models;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
namespace server_truyen_v.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ServiceComent _serComment;
        public CommentController(ServiceComent serComment)
        {
            _serComment = serComment;
        }

        [HttpPost("insert-comment")]
        public async Task<IActionResult> insertC([FromBody] formComsent fc)
        {
            var c = await _serComment.insertCommnet(fc);

            if (c == null)
            {
                return BadRequest(new { Data = "Lỗi" });
            }
            else
            {
                return Ok(new { Data = c });
            }
        }

        [HttpPut("update-comment")]
        [Authorize]
        public async Task<IActionResult> updateComment([FromBody] formComsent fc)
        {
            var c = await _serComment.updateComent(fc);

            if (!c)
            {
                return BadRequest(new { Data = "Lỗi" });
            }
            else
            {
                return Ok(new { Data = c });
            }
        }

        [HttpDelete("delete-comment/{id}")]
        [Authorize]
        public async Task<IActionResult> del(string id)
        {
            int i = int.Parse(id);
            var c = await _serComment.deleteForId(i);

            if (!c)
            {
                return BadRequest(new { Data = "Lỗi" });
            }
            else
            {
                return Ok(new { Data = "Thành công" });
            }
        }

        [HttpGet("get-all-comment/")]
        public async Task<IActionResult> getAllC([FromQuery] string? id)
        {
            List<Comments> listC = new List<Comments>();
            if(!string.IsNullOrEmpty(id)){
                int i = int.Parse(id);
                listC = await _serComment.getComentAsync(i);
            }else{
                listC = await _serComment.getComentAsync(null);
            }
            if (listC != null)
            {
                return Ok(new { Data = listC });
            }
            else
            {
                return Ok(new { Data = "Chưa có dữ liệu phù hợp" });
            }
        }
    }
}