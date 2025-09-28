namespace WebMusic.Application.Commands.Songs
{
    public class DeleteSongCommand
    {
        public int SongId { get; set; }
    }

    public class DeleteSongCommandResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
