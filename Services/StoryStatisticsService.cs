using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server_truyen_v.Data;
using Microsoft.EntityFrameworkCore;

namespace server_truyen_v.Services
{
    public class StoryStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public StoryStatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Thống kê những thể loại được đọc nhiều nhất
        public async Task<List<object>> GetMostReadGenres()
        {
            var stories = await _context.Story.ToListAsync();
            var detailStories = await _context.DetailStories.ToListAsync();

            return stories
                .SelectMany(s => s.TypeDetailStory.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(genre => new { s.Id, Genre = genre.Trim() }))
                .GroupBy(g => g.Genre)
                .Select(g => new
                {
                    Genre = g.Key,
                    TotalViews = g.Sum(s => detailStories
                        .Where(d => d.StoryID == s.Id)
                        .Sum(d => d.Views))
                })
                .OrderByDescending(g => g.TotalViews)
                .ToList<object>();
        }

        // Thống kê truyện được đọc nhiều nhất
        public async Task<List<object>> GetMostReadStories()
        {
            return await _context.Story
                .Select(s => new
                {
                    StoryName = s.StoryName,
                    TotalViews = _context.DetailStories
                        .Where(d => d.StoryID == s.Id)
                        .Sum(d => d.Views)
                })
                .OrderByDescending(s => s.TotalViews)
                .ToListAsync<object>();
        }

        // Thống kê số lượng truyện theo TypeDetailStory
        public async Task<List<object>> GetStoryCountByType()
        {
            var stories = await _context.Story.ToListAsync();

            return stories
                .SelectMany(s => s.TypeDetailStory.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(genre => new { Genre = genre.Trim() }))
                .GroupBy(g => g.Genre)
                .Select(g => new
                {
                    Genre = g.Key,
                    StoryCount = g.Count()
                })
                .OrderByDescending(g => g.StoryCount)
                .ToList<object>();
        }
    }
}
