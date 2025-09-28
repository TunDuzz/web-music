using WebMusic.Application.Commands.Songs;
using WebMusic.Application.DTOs;
using WebMusic.Application.Queries.Songs;
using WebMusic.Application.Services;
using WebMusic.Domain.Interfaces;
using WebMusic.Web.ViewModels.Songs;

namespace WebMusic.Web.Services
{
    /// <summary>
    /// Service implementation cho xử lý business logic của Songs trong Web layer
    /// </summary>
    public class SongWebService : ISongWebService
    {
        private readonly ISongService _songService;
        private readonly IGenreRepository _genreRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IFileUploadService _fileUploadService;

        public SongWebService(
            ISongService songService,
            IGenreRepository genreRepository,
            IAlbumRepository albumRepository,
            IArtistRepository artistRepository,
            IFileUploadService fileUploadService)
        {
            _songService = songService;
            _genreRepository = genreRepository;
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<SongListViewModel> GetSongsAsync(SongListViewModel model)
        {
            var query = new GetSongsQuery
            {
                UserId = model.UserId,
                GenreId = model.GenreId,
                AlbumId = model.AlbumId,
                ArtistId = model.ArtistId,
                SearchTerm = model.SearchTerm,
                Page = model.Page,
                PageSize = model.PageSize,
                SortBy = model.SortBy,
                SortDirection = model.SortDirection
            };

            var result = await _songService.GetSongsAsync(query);

            return new SongListViewModel
            {
                Songs = result.Songs,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                SearchTerm = model.SearchTerm,
                GenreId = model.GenreId,
                AlbumId = model.AlbumId,
                ArtistId = model.ArtistId,
                SortBy = model.SortBy,
                SortDirection = model.SortDirection
            };
        }

        public async Task<SongCreateViewModel> GetCreateViewModelAsync()
        {
            var genres = await _genreRepository.GetAllGenresAsync();
            var albums = await _albumRepository.GetAllAlbumsAsync();
            var artists = await _artistRepository.GetAllArtistsAsync();

            return new SongCreateViewModel
            {
                Genres = genres.Select(g => new GenreDto
                {
                    GenreId = g.GenreId,
                    GenreName = g.GenreName
                }),
                Albums = albums.Select(a => new AlbumDto
                {
                    AlbumId = a.AlbumId,
                    AlbumName = a.AlbumName
                }),
                Artists = artists.Select(a => new ArtistDto
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName
                })
            };
        }

        public async Task<SongEditViewModel> GetEditViewModelAsync(int id)
        {
            var songQuery = new GetSongByIdQuery { SongId = id };
            var songResult = await _songService.GetSongByIdAsync(songQuery);

            if (!songResult.Success || songResult.Song == null)
                throw new ArgumentException("Song not found");

            var genres = await _genreRepository.GetAllGenresAsync();
            var albums = await _albumRepository.GetAllAlbumsAsync();
            var artists = await _artistRepository.GetAllArtistsAsync();

            return new SongEditViewModel
            {
                SongId = songResult.Song.SongId,
                Title = songResult.Song.Title,
                CoverImage = songResult.Song.CoverImage,
                FileUrl = songResult.Song.FileUrl,
                Duration = songResult.Song.Duration,
                Status = songResult.Song.Status,
                UploadedAt = songResult.Song.UploadedAt,
                ViewCount = songResult.Song.ViewCount,
                LikeCount = songResult.Song.LikeCount,
                CommentCount = songResult.Song.CommentCount,
                GenreId = songResult.Song.GenreId,
                AlbumId = songResult.Song.AlbumId,
                ArtistId = songResult.Song.ArtistId,
                Genres = genres.Select(g => new GenreDto
                {
                    GenreId = g.GenreId,
                    GenreName = g.GenreName
                }),
                Albums = albums.Select(a => new AlbumDto
                {
                    AlbumId = a.AlbumId,
                    AlbumName = a.AlbumName
                }),
                Artists = artists.Select(a => new ArtistDto
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName
                })
            };
        }

        public async Task<bool> CreateSongAsync(SongCreateViewModel model)
        {
            try
            {
                string? fileUrl = model.FileUrl;
                string? coverImage = model.CoverImage;

                // Upload audio file if provided
                if (model.AudioFile != null)
                {
                    fileUrl = await _fileUploadService.UploadAudioAsync(model.AudioFile, 1); // TODO: Get from current user
                }

                // Upload cover image if provided
                if (model.CoverImageFile != null)
                {
                    coverImage = await _fileUploadService.UploadImageAsync(model.CoverImageFile, 1, "songs");
                }

                var command = new CreateSongCommand
                {
                    Title = model.Title,
                    FileUrl = fileUrl ?? string.Empty,
                    CoverImage = coverImage,
                    Duration = model.Duration,
                    GenreId = model.GenreId,
                    AlbumId = model.AlbumId,
                    ArtistId = model.ArtistId,
                    UserId = 1 // TODO: Get from current user context
                };

                var result = await _songService.CreateSongAsync(command);
                return result.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSongAsync(SongEditViewModel model)
        {
            var command = new UpdateSongCommand
            {
                SongId = model.SongId,
                Title = model.Title,
                CoverImage = model.CoverImage,
                GenreId = model.GenreId,
                AlbumId = model.AlbumId,
                ArtistId = model.ArtistId
            };

            var result = await _songService.UpdateSongAsync(command);
            return result.Success;
        }

        public async Task<bool> DeleteSongAsync(int id)
        {
            var command = new DeleteSongCommand { SongId = id };
            var result = await _songService.DeleteSongAsync(command);
            return result.Success;
        }

        public async Task<SongDto?> GetSongByIdAsync(int id)
        {
            var query = new GetSongByIdQuery { SongId = id };
            var result = await _songService.GetSongByIdAsync(query);
            return result.Success ? result.Song : null;
        }
    }
}
