using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server_truyen_v.Models
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; } = 0; // Mã bình luận

        [Required]
        public int DetailID { get; set; } = 0; // Mã chương liên kết

        [Required]
        [MaxLength(300)]
        public string CommentText { get; set; } = ""; // Nội dung bình luận

        [Required]
        public int Ratting { get; set; } = 0; // Đánh giá chương (không nullable)

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Thời gian bình luận

        // Quan hệ N-1 với DetailStory
        [ForeignKey(nameof(DetailID))]
        public DetailStory? DetailStory { get; set; }
    }
}
