namespace WebMusic.Application.Commands.Albums
{
    public class DeleteAlbumCommand
    {
        public int AlbumId { get; set; }
    }

    public class DeleteAlbumCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
