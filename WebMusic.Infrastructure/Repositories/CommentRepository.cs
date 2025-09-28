using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly WebMusicDbContext _context;

        public CommentRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Song)
                .ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Song)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsBySongIdAsync(int songId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Song)
                .Where(c => c.SongId == songId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Song)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Comments.AnyAsync(c => c.CommentId == id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Comments.CountAsync();
        }

        public async Task<int> GetCountBySongIdAsync(int songId)
        {
            return await _context.Comments.CountAsync(c => c.SongId == songId);
        }
    }
}
