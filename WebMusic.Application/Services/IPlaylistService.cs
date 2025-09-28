using WebMusic.Application.Commands.Playlists;
using WebMusic.Application.Queries.Playlists;

namespace WebMusic.Application.Services
{
    public interface IPlaylistService
    {
        Task<CreatePlaylistCommandResponse> CreatePlaylistAsync(CreatePlaylistCommand command);
        Task<UpdatePlaylistCommandResponse> UpdatePlaylistAsync(UpdatePlaylistCommand command);
        Task<DeletePlaylistCommandResponse> DeletePlaylistAsync(DeletePlaylistCommand command);
        Task<GetPlaylistByIdQueryResponse> GetPlaylistByIdAsync(GetPlaylistByIdQuery query);
        Task<GetPlaylistsQueryResponse> GetPlaylistsAsync(GetPlaylistsQuery query);
    }
}
