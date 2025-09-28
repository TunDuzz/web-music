using WebMusic.Application.Commands.Playlists;
using WebMusic.Application.DTOs;
using WebMusic.Application.Queries.Playlists;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;

namespace WebMusic.Application.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;

        public PlaylistService(
            IPlaylistRepository playlistRepository,
            IUserRepository userRepository)
        {
            _playlistRepository = playlistRepository;
            _userRepository = userRepository;
        }

        public async Task<CreatePlaylistCommandResponse> CreatePlaylistAsync(CreatePlaylistCommand command)
        {
            try
            {
                var playlist = new Playlist
                {
                    PlaylistName = command.PlaylistName,
                    Description = command.Description,
                    IsPublic = command.IsPublic,
                    UserId = command.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                var createdPlaylist = await _playlistRepository.AddPlaylistAsync(playlist);
                var playlistDto = await MapToPlaylistDto(createdPlaylist);

                return new CreatePlaylistCommandResponse
                {
                    Success = true,
                    Message = "Playlist created successfully",
                    Playlist = playlistDto
                };
            }
            catch (Exception ex)
            {
                return new CreatePlaylistCommandResponse
                {
                    Success = false,
                    Message = $"Error creating playlist: {ex.Message}"
                };
            }
        }

        public async Task<UpdatePlaylistCommandResponse> UpdatePlaylistAsync(UpdatePlaylistCommand command)
        {
            try
            {
                var existingPlaylist = await _playlistRepository.GetPlaylistByIdAsync(command.PlaylistId);
                if (existingPlaylist == null)
                {
                    return new UpdatePlaylistCommandResponse
                    {
                        Success = false,
                        Message = "Playlist not found"
                    };
                }

                existingPlaylist.PlaylistName = command.PlaylistName;
                existingPlaylist.Description = command.Description;
                existingPlaylist.IsPublic = command.IsPublic;
                existingPlaylist.UpdatedAt = DateTime.UtcNow;

                var updatedPlaylist = await _playlistRepository.UpdatePlaylistAsync(existingPlaylist);
                var playlistDto = await MapToPlaylistDto(updatedPlaylist);

                return new UpdatePlaylistCommandResponse
                {
                    Success = true,
                    Message = "Playlist updated successfully",
                    Playlist = playlistDto
                };
            }
            catch (Exception ex)
            {
                return new UpdatePlaylistCommandResponse
                {
                    Success = false,
                    Message = $"Error updating playlist: {ex.Message}"
                };
            }
        }

        public async Task<DeletePlaylistCommandResponse> DeletePlaylistAsync(DeletePlaylistCommand command)
        {
            try
            {
                var existingPlaylist = await _playlistRepository.GetPlaylistByIdAsync(command.PlaylistId);
                if (existingPlaylist == null)
                {
                    return new DeletePlaylistCommandResponse
                    {
                        Success = false,
                        Message = "Playlist not found"
                    };
                }

                await _playlistRepository.DeletePlaylistAsync(command.PlaylistId);

                return new DeletePlaylistCommandResponse
                {
                    Success = true,
                    Message = "Playlist deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeletePlaylistCommandResponse
                {
                    Success = false,
                    Message = $"Error deleting playlist: {ex.Message}"
                };
            }
        }

        public async Task<GetPlaylistByIdQueryResponse> GetPlaylistByIdAsync(GetPlaylistByIdQuery query)
        {
            try
            {
                var playlist = await _playlistRepository.GetPlaylistByIdAsync(query.PlaylistId);
                if (playlist == null)
                {
                    return new GetPlaylistByIdQueryResponse
                    {
                        Success = false,
                        Message = "Playlist not found"
                    };
                }

                var playlistDto = await MapToPlaylistDto(playlist);

                return new GetPlaylistByIdQueryResponse
                {
                    Success = true,
                    Playlist = playlistDto
                };
            }
            catch (Exception ex)
            {
                return new GetPlaylistByIdQueryResponse
                {
                    Success = false,
                    Message = $"Error retrieving playlist: {ex.Message}"
                };
            }
        }

        public async Task<GetPlaylistsQueryResponse> GetPlaylistsAsync(GetPlaylistsQuery query)
        {
            try
            {
                IEnumerable<Playlist> playlists;

                if (!string.IsNullOrEmpty(query.SearchTerm))
                {
                    playlists = await _playlistRepository.SearchPlaylistsAsync(query.SearchTerm);
                }
                else if (query.UserId.HasValue)
                {
                    playlists = await _playlistRepository.GetPlaylistsByUserIdAsync(query.UserId.Value);
                }
                else if (query.IsPublic.HasValue)
                {
                    playlists = await _playlistRepository.GetPublicPlaylistsAsync();
                }
                else
                {
                    playlists = await _playlistRepository.GetAllPlaylistsAsync();
                }

                var totalCount = playlists.Count();
                var pagedPlaylists = playlists
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();

                var playlistDtos = new List<PlaylistDto>();
                foreach (var playlist in pagedPlaylists)
                {
                    playlistDtos.Add(await MapToPlaylistDto(playlist));
                }

                return new GetPlaylistsQueryResponse
                {
                    Success = true,
                    Playlists = playlistDtos,
                    TotalCount = totalCount,
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / query.PageSize)
                };
            }
            catch (Exception ex)
            {
                return new GetPlaylistsQueryResponse
                {
                    Success = false,
                    Message = $"Error retrieving playlists: {ex.Message}"
                };
            }
        }

        private async Task<PlaylistDto> MapToPlaylistDto(Playlist playlist)
        {
            var playlistDto = new PlaylistDto
            {
                PlaylistId = playlist.PlaylistId,
                PlaylistName = playlist.PlaylistName,
                Description = playlist.Description,
                CreatedAt = playlist.CreatedAt,
                UpdatedAt = playlist.UpdatedAt,
                IsPublic = playlist.IsPublic,
                SongCount = playlist.SongCount,
                UserId = playlist.UserId
            };

            // Load related data
            if (playlist.User != null)
            {
                playlistDto.UserName = playlist.User.UserName;
            }
            else if (playlist.UserId > 0)
            {
                var user = await _userRepository.GetUserByIdAsync(playlist.UserId);
                playlistDto.UserName = user?.UserName;
            }

            return playlistDto;
        }
    }
}
