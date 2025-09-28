using WebMusic.Domain.Entities;

namespace WebMusic.Domain.Interfaces
{
    public interface IArtistRepository
    {
        Task<IEnumerable<Artist>> GetAllArtistsAsync();
        Task<Artist?> GetArtistByIdAsync(int id);
        Task<Artist?> GetArtistByNameAsync(string name);
        Task<IEnumerable<Artist>> SearchArtistsAsync(string searchTerm);
        Task<Artist> AddArtistAsync(Artist artist);
        Task<Artist> UpdateArtistAsync(Artist artist);
        Task DeleteArtistAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NameExistsAsync(string name);
        Task<int> GetTotalCountAsync();
    }
}
