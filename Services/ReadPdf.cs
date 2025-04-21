using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PdfiumViewer;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace server_truyen_v.Services
{
    public class ReadPdf
    {
        private readonly Cloudinarys _cloudinaryService;

        public ReadPdf(Cloudinarys cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<string> ReadAndRenderAsync(IFormFile file, string name)
        {
            string allUrl = "";
            try
            {
                // Kiểm tra xem file có phải là PDF không
                if (file == null || file.Length == 0)
                {
                    return "Không có file được tải lên.";
                }

                // Đọc file PDF từ MemoryStream
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream); // Tải file vào MemoryStream
                    using (var pdfDocument = PdfDocument.Load(memoryStream))
                    {
                        int pageCount = pdfDocument.PageCount;

                        // Lặp qua từng trang và render ra hình ảnh
                        for (int i = 0; i < pageCount; i++)
                        {
                            // Render trang PDF thành bitmap
                            using (var image = pdfDocument.Render(i, 300, 300, true))
                            {
                                using (var ms = new MemoryStream())
                                {
                                    image.Save(ms, ImageFormat.Png);
                                    ms.Seek(0, SeekOrigin.Begin); // Đặt con trỏ file trở lại đầu

                                    // Tạo tên file cho mỗi trang
                                    string fileName = $"{name}_Page_{i + 1}.png";
                                    string fileNames = $"{name}_Page_{i + 1}";

                                    // Gọi hàm uploadImg để upload ảnh lên Cloudinary
                                    var uploadResult = await _cloudinaryService.uploadImg(ms, fileName, fileNames);
                                    allUrl += uploadResult + ",";
                                    // Nếu upload thành công, kết quả trả về sẽ là Secure URL
                                    if (!string.IsNullOrEmpty(uploadResult))
                                    {
                                        Console.WriteLine($"Trang {i + 1} sucsseful: {uploadResult}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"error upload {i + 1}");
                                    }
                                }
                            }
                        }
                    }
                }

                return allUrl;
            }
            catch (Exception ex)
            {
                return $"Có lỗi xảy ra: {ex.Message}";
            }
        }
    }
}
