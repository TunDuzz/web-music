using WebMusic.Application.Commands.Albums;
using WebMusic.Application.Queries.Albums;

namespace WebMusic.Application.Services
{
    public interface IAlbumService
    {
        Task<CreateAlbumCommandResponse> CreateAlbumAsync(CreateAlbumCommand command);
        Task<UpdateAlbumCommandResponse> UpdateAlbumAsync(UpdateAlbumCommand command);
        Task<DeleteAlbumCommandResponse> DeleteAlbumAsync(DeleteAlbumCommand command);
        Task<GetAlbumByIdQueryResponse> GetAlbumByIdAsync(GetAlbumByIdQuery query);
        Task<GetAlbumsQueryResponse> GetAlbumsAsync(GetAlbumsQuery query);
    }
}
