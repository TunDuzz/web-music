using WebMusic.Application.Commands.Albums;
using WebMusic.Application.DTOs;
using WebMusic.Application.Queries.Albums;
using WebMusic.Application.Services;
using WebMusic.Domain.Interfaces;
using WebMusic.Web.ViewModels.Albums;

namespace WebMusic.Web.Services
{
    /// <summary>
    /// Service implementation cho xử lý business logic của Albums trong Web layer
    /// </summary>
    public class AlbumWebService : IAlbumWebService
    {
        private readonly IAlbumService _albumService;
        private readonly IArtistRepository _artistRepository;
        private readonly IUserRepository _userRepository;

        public AlbumWebService(
            IAlbumService albumService,
            IArtistRepository artistRepository,
            IUserRepository userRepository)
        {
            _albumService = albumService;
            _artistRepository = artistRepository;
            _userRepository = userRepository;
        }

        public async Task<AlbumListViewModel> GetAlbumsAsync(AlbumListViewModel model)
        {
            var query = new GetAlbumsQuery
            {
                UserId = model.UserId,
                ArtistId = model.ArtistId,
                SearchTerm = model.SearchTerm,
                Page = model.Page,
                PageSize = model.PageSize,
                SortBy = model.SortBy,
                SortDirection = model.SortDirection
            };

            var result = await _albumService.GetAlbumsAsync(query);

            return new AlbumListViewModel
            {
                Albums = result.Albums,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                SearchTerm = model.SearchTerm,
                ArtistId = model.ArtistId,
                UserId = model.UserId,
                SortBy = model.SortBy,
                SortDirection = model.SortDirection
            };
        }

        public async Task<AlbumCreateViewModel> GetCreateViewModelAsync()
        {
            var artists = await _artistRepository.GetAllArtistsAsync();

            return new AlbumCreateViewModel
            {
                Artists = artists.Select(a => new ArtistDto
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName
                })
            };
        }

        public async Task<AlbumEditViewModel> GetEditViewModelAsync(int id)
        {
            var albumQuery = new GetAlbumByIdQuery { AlbumId = id };
            var albumResult = await _albumService.GetAlbumByIdAsync(albumQuery);

            if (!albumResult.Success || albumResult.Album == null)
                throw new ArgumentException("Album not found");

            var artists = await _artistRepository.GetAllArtistsAsync();
            var user = await _userRepository.GetUserByIdAsync(albumResult.Album.UserId);

            return new AlbumEditViewModel
            {
                AlbumId = albumResult.Album.AlbumId,
                AlbumName = albumResult.Album.AlbumName,
                Description = albumResult.Album.Description,
                ReleaseDate = albumResult.Album.ReleaseDate,
                CoverImage = albumResult.Album.CoverImage,
                ArtistId = albumResult.Album.ArtistId,
                CreatedAt = albumResult.Album.CreatedAt,
                SongCount = albumResult.Album.SongCount,
                UserName = user?.UserName ?? "Unknown",
                Artists = artists.Select(a => new ArtistDto
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName
                })
            };
        }

        public async Task<bool> CreateAlbumAsync(AlbumCreateViewModel model)
        {
            var command = new CreateAlbumCommand
            {
                AlbumName = model.AlbumName,
                Description = model.Description,
                ReleaseDate = model.ReleaseDate,
                CoverImage = model.CoverImage,
                ArtistId = model.ArtistId,
                UserId = 1 // TODO: Get from current user context
            };

            var result = await _albumService.CreateAlbumAsync(command);
            return result.Success;
        }

        public async Task<bool> UpdateAlbumAsync(AlbumEditViewModel model)
        {
            var command = new UpdateAlbumCommand
            {
                AlbumId = model.AlbumId,
                AlbumName = model.AlbumName,
                Description = model.Description,
                ReleaseDate = model.ReleaseDate,
                CoverImage = model.CoverImage,
                ArtistId = model.ArtistId
            };

            var result = await _albumService.UpdateAlbumAsync(command);
            return result.Success;
        }

        public async Task<bool> DeleteAlbumAsync(int id)
        {
            var command = new DeleteAlbumCommand { AlbumId = id };
            var result = await _albumService.DeleteAlbumAsync(command);
            return result.Success;
        }

        public async Task<AlbumDto?> GetAlbumByIdAsync(int id)
        {
            var query = new GetAlbumByIdQuery { AlbumId = id };
            var result = await _albumService.GetAlbumByIdAsync(query);
            return result.Success ? result.Album : null;
        }
    }
}
