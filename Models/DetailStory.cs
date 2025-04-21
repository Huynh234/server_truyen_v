namespace server_truyen_v.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class DetailStory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailID { get; set; } = 0; // Mã chương

        [Required]
        public int StoryID { get; set; } = 0; // Mã truyện liên kết

        [Required]
        public string SttName { get; set; } = "";

        [MaxLength(10000)]
        public string UrlImg { get; set; } = ""; // URL ảnh chương
        [MaxLength(10000)]
        public string Content { get; set; } = ""; // Nội dung chương

        [Required]
        public int Views { get; set; } = 0; // Lượt xem, mặc định là 0

        // Quan hệ N-1 với Story
        [ForeignKey("StoryID")]
        public Storys? Storys { get; set; }

        // Quan hệ 1-N với Comments
        public List<Comments> Comments { get; set; } = new List<Comments>();
    }
}