using WebMusic.Application.Commands.Playlists;
using WebMusic.Application.DTOs;
using WebMusic.Application.Queries.Playlists;
using WebMusic.Application.Services;
using WebMusic.Domain.Interfaces;
using WebMusic.Web.ViewModels.Playlists;

namespace WebMusic.Web.Services
{
    /// <summary>
    /// Service implementation cho xử lý business logic của Playlists trong Web layer
    /// </summary>
    public class PlaylistWebService : IPlaylistWebService
    {
        private readonly IPlaylistService _playlistService;
        private readonly IUserRepository _userRepository;

        public PlaylistWebService(
            IPlaylistService playlistService,
            IUserRepository userRepository)
        {
            _playlistService = playlistService;
            _userRepository = userRepository;
        }

        public async Task<PlaylistListViewModel> GetPlaylistsAsync(PlaylistListViewModel model)
        {
            var query = new GetPlaylistsQuery
            {
                UserId = model.UserId,
                SearchTerm = model.SearchTerm,
                IsPublic = model.IsPublic,
                Page = model.Page,
                PageSize = model.PageSize,
                SortBy = model.SortBy,
                SortDirection = model.SortDirection
            };

            var result = await _playlistService.GetPlaylistsAsync(query);

            return new PlaylistListViewModel
            {
                Playlists = result.Playlists,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                SearchTerm = model.SearchTerm,
                UserId = model.UserId,
                IsPublic = model.IsPublic,
                SortBy = model.SortBy,
                SortDirection = model.SortDirection
            };
        }

        public async Task<PlaylistCreateViewModel> GetCreateViewModelAsync()
        {
            return new PlaylistCreateViewModel();
        }

        public async Task<PlaylistEditViewModel> GetEditViewModelAsync(int id)
        {
            var playlistQuery = new GetPlaylistByIdQuery { PlaylistId = id };
            var playlistResult = await _playlistService.GetPlaylistByIdAsync(playlistQuery);

            if (!playlistResult.Success || playlistResult.Playlist == null)
                throw new ArgumentException("Playlist not found");

            var user = await _userRepository.GetUserByIdAsync(playlistResult.Playlist.UserId);

            return new PlaylistEditViewModel
            {
                PlaylistId = playlistResult.Playlist.PlaylistId,
                PlaylistName = playlistResult.Playlist.PlaylistName,
                Description = playlistResult.Playlist.Description,
                IsPublic = playlistResult.Playlist.IsPublic,
                CreatedAt = playlistResult.Playlist.CreatedAt,
                SongCount = playlistResult.Playlist.SongCount,
                UserName = user?.UserName ?? "Unknown"
            };
        }

        public async Task<bool> CreatePlaylistAsync(PlaylistCreateViewModel model)
        {
            var command = new CreatePlaylistCommand
            {
                PlaylistName = model.PlaylistName,
                Description = model.Description,
                IsPublic = model.IsPublic,
                UserId = 1 // TODO: Get from current user context
            };

            var result = await _playlistService.CreatePlaylistAsync(command);
            return result.Success;
        }

        public async Task<bool> UpdatePlaylistAsync(PlaylistEditViewModel model)
        {
            var command = new UpdatePlaylistCommand
            {
                PlaylistId = model.PlaylistId,
                PlaylistName = model.PlaylistName,
                Description = model.Description,
                IsPublic = model.IsPublic
            };

            var result = await _playlistService.UpdatePlaylistAsync(command);
            return result.Success;
        }

        public async Task<bool> DeletePlaylistAsync(int id)
        {
            var command = new DeletePlaylistCommand { PlaylistId = id };
            var result = await _playlistService.DeletePlaylistAsync(command);
            return result.Success;
        }

        public async Task<PlaylistDto?> GetPlaylistByIdAsync(int id)
        {
            var query = new GetPlaylistByIdQuery { PlaylistId = id };
            var result = await _playlistService.GetPlaylistByIdAsync(query);
            return result.Success ? result.Playlist : null;
        }
    }
}
