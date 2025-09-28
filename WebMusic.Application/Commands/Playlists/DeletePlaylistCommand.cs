namespace WebMusic.Application.Commands.Playlists
{
    public class DeletePlaylistCommand
    {
        public int PlaylistId { get; set; }
    }

    public class DeletePlaylistCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
