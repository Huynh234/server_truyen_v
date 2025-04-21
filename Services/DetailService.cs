using Microsoft.EntityFrameworkCore;
using server_truyen_v.Data;
using server_truyen_v.Models;
using server_truyen_v.Schemas;
namespace server_truyen_v.Services
{
    public class DetailService
    {
        private readonly ApplicationDbContext _context;
        private readonly ServiceStory _sstory;
        public DetailService(ApplicationDbContext context, ServiceStory sstory)
        {
            _context = context;
            _sstory = sstory;
        }
        public async Task<DetailStory> InsertIntoDetail(string imgOrContent, int id, string last, string stt)
        {
            DetailStory detailSto = new DetailStory();
            var sto = await _sstory.GetStoryAsync(id);
            if (last.Equals(".pdf") && sto[0].TypeStory.Equals("TT"))
            {
                detailSto = new DetailStory
                {
                    SttName = stt,
                    StoryID = sto[0].Id,
                    UrlImg = imgOrContent,
                };
            }
            else if (last.Equals(".txt") && sto[0].TypeStory.Equals("TC"))
            {
                detailSto = new DetailStory
                {
                    SttName = stt,
                    StoryID = sto[0].Id,
                    Content = imgOrContent
                };
            }

            try
            {
                await _context.DetailStories.AddAsync(detailSto);
                await _context.SaveChangesAsync();
                return detailSto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                detailSto = new DetailStory();
            }
        }

        public async Task<DetailStory> getDetailID(int? id)
        {

            try
            {
                var listX = await _context.DetailStories.FirstOrDefaultAsync(x => x.DetailID == id);
                return listX ?? new DetailStory();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<DetailStory>> getAllDetailID(int? id)
        {

            try
            {
                List<DetailStory> listX = new List<DetailStory>();
                if (id != 0){
                   listX = await _context.DetailStories.Where(x => x.StoryID == id).ToListAsync();
                }else{
                    listX = await _context.DetailStories.ToListAsync();
                }
                
                return listX ?? new List<DetailStory>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<formChapter>> getChapter(int? id)
        {

            try
            {
                var listX = await _context.DetailStories.Where(x => x.StoryID == id).OrderBy(x => x.SttName).Select(x => new formChapter{
                    id = x.StoryID,
                    detailId = x.DetailID,
                    chapter = x.SttName
                }).ToListAsync();
                return listX ?? new List<formChapter>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<DetailStory> updateView(int? id)
        {
            var detail = await getDetailID(id);
            if (detail == null)
            {
                return new DetailStory();
            }
            try
            {
                detail.Views++;
                await _context.SaveChangesAsync();
                return detail;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> deleteForIdDS(int? id)
        {
            var detail = await getDetailID(id);
            if (detail == null)
            {
                return false;
            }
            try
            {
                _context.DetailStories.Remove(detail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> deleteAllForIdS(int? id)
        {
            var detail = await getAllDetailID(id);
            if (detail == null)
            {
                return false;
            }
            try
            {
                _context.DetailStories.RemoveRange(detail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}