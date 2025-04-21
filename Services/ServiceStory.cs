using Microsoft.EntityFrameworkCore;
using server_truyen_v.Data;
using server_truyen_v.Models;
using server_truyen_v.Schemas;
namespace server_truyen_v.Services
{
    public class ServiceStory
    {
        private readonly ReadPdf _readPdfService;
        private readonly ApplicationDbContext _context;


        public ServiceStory(ReadPdf readPdfService, ApplicationDbContext context)
        {
            _readPdfService = readPdfService;
            _context = context;
        }
        public async Task<Storys> InsertIntoStory(formStory st)
        {
            try
            {
                var sto = new Storys
                {
                    StoryCode = st.storyCode,
                    StoryName = st.storyName,
                    CoverImage = st.imgCover,
                    StoryAuthor = st.autho,
                    Description = st.description,
                    TypeStory = st.typeStory,
                    TypeDetailStory = st.typeDetailStory,
                };

                await _context.AddAsync(sto);
                await _context.SaveChangesAsync();
                return sto ?? new Storys();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<Storys>> GetStoryAsync(int? code = null, char? shor = null)
        {
            try
            {

                if (code.HasValue)
                {
                    var story = await _context.Story.FirstOrDefaultAsync(x => x.Id == code.Value);
                    return story != null ? new List<Storys> { story } : new List<Storys>();
                }

                if (shor.HasValue && shor.Value == 'd')
                {
                    return await _context.Story.OrderByDescending(c => c.Id).ToListAsync();
                }

                return await _context.Story.ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching stories: {e.Message}");
            }
        }

        public async Task<List<Storys>> GetTopStoriesByViewsAsync(int limit = 10, string type = "TT")
        {
            try
            {
                var topStories = await _context.Story
                    .Where(s => s.TypeStory == type)
                    .Select(s => new
                    {
                        Story = s,
                        TotalViews = _context.DetailStories
                            .Where(d => d.StoryID == s.Id)
                            .Sum(d => d.Views)
                    })
                    .OrderByDescending(s => s.TotalViews)
                    .Take(limit)
                    .Select(s => s.Story)
                    .ToListAsync();

                return topStories;
            }
            catch (Exception e)
            {
                throw new Exception($"Error fetching top stories: {e.Message}");
            }
        }

        public async Task<List<Storys>> FindStorys(string nameOrCode)
        {
            var list = new List<Storys>();
            try
            {
                list = await _context.Story.Where(x => x.StoryCode.ToLower().Trim().Contains(nameOrCode.ToLower().Trim()) || x.StoryName.ToLower().Trim().Contains(nameOrCode.ToLower().Trim()) || x.StoryAuthor.ToLower().Trim().Contains(nameOrCode.ToLower().Trim())).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Storys>> FilterStorys(string fil)
        {
            try
            {
                var ar = fil.Split(',');
                var listS = await _context.Story.Where(story => ar.All(a => story.TypeDetailStory.ToLower().Contains(a.ToLower().Trim()))).ToListAsync();
                return listS;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> deleteForID(int id)
        {
            try
            {
                var sto = await _context.Story.FirstOrDefaultAsync(x => x.Id == id);
                if (sto == null)
                {
                    return false;
                }
                _context.Story.Remove(sto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Storys> updateStory(formUpdateStory fus)
        {
            var storyId = int.Parse(fus.Id);
            var sto = await _context.Story.FirstOrDefaultAsync(x => x.Id == storyId);
            if (sto == null)
            {
                throw new Exception("Story not found");
            }

            if (!string.IsNullOrEmpty(fus.StoryName))
            {
                sto.StoryName = fus.StoryName;
            }
            if (!string.IsNullOrEmpty(fus.CoverImage))
            {
                sto.CoverImage = fus.CoverImage;
            }
            if (!string.IsNullOrEmpty(fus.Description))
            {
                sto.Description = fus.Description;
            }
            if (!string.IsNullOrEmpty(fus.StoryAuthor))
            {
                sto.StoryAuthor = fus.StoryAuthor;
            }
            if (!string.IsNullOrEmpty(fus.TypeDetailStory))
            {
                sto.TypeDetailStory = fus.TypeDetailStory;
            }

            await _context.SaveChangesAsync();
            return sto;
        }
        public async Task<List<string>> getTypeDetailStory()
        {
            try
            {
                var li = await _context.Story.Select(s => s.TypeDetailStory).Distinct().ToListAsync();
                string? ss = "";
                foreach (var item in li)
                {
                    ss += item + ",";
                }
                return ss.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Storys>> GetStoriesOrderedByCommentCountAsync()
        {
            var stories = await _context.Story
                .Select(story => new
                {
                    Story = story,
                    TotalComments = _context.DetailStories
                        .Where(d => d.StoryID == story.Id)
                        .SelectMany(d => d.Comments)
                        .Count()
                })
                .OrderByDescending(x => x.TotalComments)
                .Select(x => x.Story)
                .ToListAsync();

            return stories;
        }
    }

}
