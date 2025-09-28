using WebMusic.Application.DTOs;

namespace WebMusic.Application.Commands.Playlists
{
    public class CreatePlaylistCommand
    {
        public string PlaylistName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPublic { get; set; } = true;
        public int UserId { get; set; }
    }

    public class CreatePlaylistCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PlaylistDto? Playlist { get; set; }
    }
}
