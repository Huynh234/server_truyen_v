namespace server_truyen_v
{
    using server_truyen_v.Data;
    using server_truyen_v.Models;
    using server_truyen_v.Schemas;
    using Microsoft.EntityFrameworkCore;
    public class ServiceComent
    {
        private readonly ApplicationDbContext _context;

        public ServiceComent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comments> insertCommnet(formComsent co)
        {
            var com = new Comments();
            if (co.id == 0)
            {
                return new Comments();
            }
            else
            {
                com.DetailID = co.id;
            }
            if (co.comment == "")
            {
                return new Comments();
            }
            else
            {
                com.CommentText = co.comment;
            }
            if (co.ratting == 0)
            {
                return new Comments();
            }
            else
            {
                com.Ratting = (int)co.ratting;
            }

            try
            {
                await _context.Comment.AddAsync(com);
                await _context.SaveChangesAsync();
                return com;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<List<Comments>> getComentAsync(int ?id)
        {
            try
            {// Lấy tất cả các bình luận có StoryId giống với tham số truyền vào
                if(id != null){
                    return await _context.Comment.Where(c => c.DetailID == id).ToListAsync();
                }else{
                    return await _context.Comment.ToListAsync();
                }
            }
                
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> updateComent(formComsent com)
        {
            if (com.id <= 0)
            {
                return false;
            }
            else
            {
                if (com.comment.Equals(""))
                {
                    return false;
                }
                var co = new Comments
                {
                    CommentText = com.comment
                };
                await _context.Comment
                    .Where(c => c.CommentID == com.id)
                    .ExecuteUpdateAsync(c => c.SetProperty(p => p.CommentText, com.comment));

                if (com.ratting <= 0)
                {
                    return false;
                }
                co = new Comments
                {
                    Ratting = (int)com.ratting
                };
                await _context.Comment
                    .Where(c => c.CommentID == com.id)
                    .ExecuteUpdateAsync(c => c.SetProperty(p => p.Ratting, (int)com.ratting));
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> deleteForId(int id)
        {
            try
            {
                var com = await _context.Comment.FirstOrDefaultAsync(x => x.CommentID == id);
                if (com == null)
                {
                    return false;
                }
                _context.Comment.Remove(com);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> deleteForIdDS(int id)
        {
            try
            {
                var com = await _context.Comment.Where(x => x.DetailID == id).ToListAsync();
                if (com == null)
                {
                    return false;
                }
                _context.Comment.RemoveRange(com);
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