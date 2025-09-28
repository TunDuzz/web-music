using WebMusic.Application.DTOs;

namespace WebMusic.Application.Queries.Playlists
{
    public class GetPlaylistByIdQuery
    {
        public int PlaylistId { get; set; }
    }

    public class GetPlaylistByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PlaylistDto? Playlist { get; set; }
    }
}
