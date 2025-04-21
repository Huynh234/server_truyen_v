using Microsoft.EntityFrameworkCore;
using server_truyen_v.Models;
namespace server_truyen_v.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet đại diện cho bảng trong cơ sở dữ liệu
        public DbSet<Storys> Story { get; set; } = null!;
        public DbSet<Comments> Comment { get; set; } = null!;

        public DbSet<DetailStory> DetailStories { get; set; } = null!;

    }
}
