using System.Linq.Expressions;

namespace server_truyen_v.Services
{
    public class ReadTxt
    {
        public async Task<string> readAll(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    // Đặt con trỏ về đầu stream để có thể đọc
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(memoryStream))
                    {
                        // Đọc toàn bộ nội dung của file vào chuỗi dataText
                        var dataText = await reader.ReadToEndAsync();

                        // Chuyển đổi nội dung sang HTML
                        // Thay đổi theo yêu cầu (ví dụ: mỗi dòng sẽ được bao quanh bởi thẻ <p> hoặc <br>)
                        string htmlContent = ConvertTextToHtml(dataText);

                        return htmlContent;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string ConvertTextToHtml(string text)
        {
            // Thay thế các dòng mới thành thẻ <br> để duy trì định dạng
            var htmlContent = text.Replace(Environment.NewLine, "<br />");
            // Bao quanh toàn bộ nội dung bằng thẻ <p> nếu bạn muốn hiển thị nó như văn bản trong một đoạn
            htmlContent = $"<p>{htmlContent}</p>";

            return htmlContent;
        }
    }
}