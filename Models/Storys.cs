namespace server_truyen_v.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    public class Storys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Khóa chính tự động tăng

        [Required]
        [MaxLength(50)]
        public string StoryCode { get; set; } = ""; // Mã truyện (bắt buộc)

        [Required]
        [MaxLength(200)]
        public string StoryName { get; set; } = ""; // Tên truyện (bắt buộc)

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow; // Ngày xuất bản (bắt buộc)

        [MaxLength(250)]
        public string CoverImage { get; set; } = ""; // URL ảnh bìa

        [MaxLength(100)]
        public string StoryAuthor { get; set; } = ""; // Tên tác giả

        [MaxLength(1000)]
        public string Description { get; set; } = ""; // Mô tả nội dung truyện (tùy chọn)

        [Required]
        [MaxLength(3)]
        public string TypeStory { get; set; } = "TT"; // Thể loại truyện (truyện tranh hoặc truyện chữ)

        [Required]
        [MaxLength(500)]
        public string TypeDetailStory { get; set; } = ""; // chi tiết thể loại

        public bool IsPublished { get; set; } = true; // Trạng thái xuất bản

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo (mặc định là thời gian hiện tại)

        public DateTime? UpdatedAt { get; set; } // Ngày cập nhật cuối (có thể null)
    }
}
