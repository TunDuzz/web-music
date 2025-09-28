using WebMusic.Application.DTOs;

namespace WebMusic.Application.Commands.Playlists
{
    public class UpdatePlaylistCommand
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPublic { get; set; } = true;
    }

    public class UpdatePlaylistCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PlaylistDto? Playlist { get; set; }
    }
}
