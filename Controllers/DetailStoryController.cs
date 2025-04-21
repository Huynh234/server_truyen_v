using Microsoft.AspNetCore.Mvc;
using server_truyen_v.Services;
using server_truyen_v.Schemas;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using server_truyen_v.Models;
namespace server_truyen_v.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailStoryController : ControllerBase
    {
        private readonly ReadPdf _readPdf;
        private readonly ReadTxt _readTxt;
        private readonly DetailService _detail;
        private readonly ServiceComent _serComment;
        public DetailStoryController(ReadPdf readPdf, DetailService deatil, ReadTxt readTxt, ServiceComent serComment)
        {
            _readPdf = readPdf;
            _detail = deatil;
            _readTxt = readTxt;
            _serComment = serComment;
        }

        [HttpPost("upload/{id}/{stt}")]
        [Authorize]
        public async Task<IActionResult> UploadFile(IFormFile file, string id, string stt)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Không có file nào được gửi.");
            }

            // Kiểm tra phần mở rộng của tệp
            var allowedExtensions = new[] { ".pdf", ".txt" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Sai định dạng. Chỉ chấp nhận file pdf và txt.");
            }

            try
            {
                int i = int.Parse(id);
                if (fileExtension == ".pdf")
                {
                    var uri = await _readPdf.ReadAndRenderAsync(file, file.FileName);
                    var req = await _detail.InsertIntoDetail(uri, i, fileExtension, stt);
                    if (req != null)
                    {
                        return Ok(new { data = req });
                    }
                    else
                    {
                        return BadRequest(new { data = "lỗi" });
                    }
                }
                else if (fileExtension == ".txt")
                {
                    var content = await _readTxt.readAll(file);
                    var req = await _detail.InsertIntoDetail(content, i, fileExtension, stt);
                    return Ok(new { data = req });
                }
                else
                {
                    return BadRequest("Sai định dạng. Chỉ chấp nhận file pdf và txt.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-all-detailstory")]
        public async Task<IActionResult> getAllDStory([FromQuery] string? id)
        {
            try
            {
                List<DetailStory> allDStory = new List<DetailStory>();
                if (!id.IsNullOrEmpty()){
                    int ids = int.Parse(id);
                    allDStory = await _detail.getAllDetailID(ids);
                }
                else
                {
                    allDStory = await _detail.getAllDetailID(0);
                }
                return Ok(new { Data = allDStory });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("get-detailstory/{id}")]
        public async Task<IActionResult> getDStory(string id)
        {
            try
            {
                int ids = int.Parse(id);
                var allDStory = await _detail.getDetailID(ids);
                return Ok(new { Data = allDStory });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("get-chapter/{id}")]
        public async Task<IActionResult> getAllChapter(string id)
        {
            try
            {
                int ids = int.Parse(id);
                var allDStory = await _detail.getChapter(ids);
                return Ok(new { Data = allDStory });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("delete-detailstory/{id}")]
        [Authorize]
        public async Task<IActionResult> deleteDStory(string id)
        {
            try
            {
                int ids = int.Parse(id);
                var al = await _serComment.deleteForIdDS(ids);
                var allDStory = await _detail.deleteForIdDS(ids);
                return Ok(new { Data = allDStory ? "Xóa thành công" : "Xóa thất bại" });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut("update-view/{id}")]
        public async Task<IActionResult> updateViewPut(string id)
        {
            try
            {
                int i = int.Parse(id);
                var dat = await _detail.updateView(i);
                if (dat == null)
                {
                    return BadRequest(new { Data = "Lỗi trong quá trình" });
                }
                else
                {
                    return Ok(new { Data = dat });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}