using WebMusic.Application.Commands.Songs;
using WebMusic.Application.DTOs;
using WebMusic.Application.Queries.Songs;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;

namespace WebMusic.Application.Services
{
    public interface ISongService
    {
        Task<CreateSongCommandResponse> CreateSongAsync(CreateSongCommand command);
        Task<UpdateSongCommandResponse> UpdateSongAsync(UpdateSongCommand command);
        Task<DeleteSongCommandResponse> DeleteSongAsync(DeleteSongCommand command);
        Task<GetSongByIdQueryResponse> GetSongByIdAsync(GetSongByIdQuery query);
        Task<GetSongsQueryResponse> GetSongsAsync(GetSongsQuery query);
    }

    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;

        public SongService(
            ISongRepository songRepository,
            IUserRepository userRepository,
            IGenreRepository genreRepository,
            IAlbumRepository albumRepository,
            IArtistRepository artistRepository)
        {
            _songRepository = songRepository;
            _userRepository = userRepository;
            _genreRepository = genreRepository;
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
        }

        public async Task<CreateSongCommandResponse> CreateSongAsync(CreateSongCommand command)
        {
            try
            {
                var song = new Song
                {
                    Title = command.Title,
                    FileUrl = command.FileUrl,
                    CoverImage = command.CoverImage,
                    Duration = command.Duration,
                    UserId = command.UserId,
                    GenreId = command.GenreId,
                    AlbumId = command.AlbumId,
                    ArtistId = command.ArtistId,
                    Status = "Pending"
                };

                var createdSong = await _songRepository.AddSongAsync(song);
                var songDto = await MapToSongDto(createdSong);

                return new CreateSongCommandResponse
                {
                    Success = true,
                    Message = "Song created successfully",
                    Song = songDto
                };
            }
            catch (Exception ex)
            {
                return new CreateSongCommandResponse
                {
                    Success = false,
                    Message = $"Error creating song: {ex.Message}"
                };
            }
        }

        public async Task<UpdateSongCommandResponse> UpdateSongAsync(UpdateSongCommand command)
        {
            try
            {
                var existingSong = await _songRepository.GetSongByIdAsync(command.SongId);
                if (existingSong == null)
                {
                    return new UpdateSongCommandResponse
                    {
                        Success = false,
                        Message = "Song not found"
                    };
                }

                existingSong.Title = command.Title;
                existingSong.CoverImage = command.CoverImage;
                existingSong.GenreId = command.GenreId;
                existingSong.AlbumId = command.AlbumId;
                existingSong.ArtistId = command.ArtistId;
                existingSong.UpdatedAt = DateTime.UtcNow;

                var updatedSong = await _songRepository.UpdateSongAsync(existingSong);
                var songDto = await MapToSongDto(updatedSong);

                return new UpdateSongCommandResponse
                {
                    Success = true,
                    Message = "Song updated successfully",
                    Song = songDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateSongCommandResponse
                {
                    Success = false,
                    Message = $"Error updating song: {ex.Message}"
                };
            }
        }

        public async Task<DeleteSongCommandResponse> DeleteSongAsync(DeleteSongCommand command)
        {
            try
            {
                var existingSong = await _songRepository.GetSongByIdAsync(command.SongId);
                if (existingSong == null)
                {
                    return new DeleteSongCommandResponse
                    {
                        Success = false,
                        Message = "Song not found"
                    };
                }

                await _songRepository.DeleteSongAsync(command.SongId);

                return new DeleteSongCommandResponse
                {
                    Success = true,
                    Message = "Song deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteSongCommandResponse
                {
                    Success = false,
                    Message = $"Error deleting song: {ex.Message}"
                };
            }
        }

        public async Task<GetSongByIdQueryResponse> GetSongByIdAsync(GetSongByIdQuery query)
        {
            try
            {
                var song = await _songRepository.GetSongByIdAsync(query.SongId);
                if (song == null)
                {
                    return new GetSongByIdQueryResponse
                    {
                        Success = false,
                        Message = "Song not found"
                    };
                }

                var songDto = await MapToSongDto(song);

                return new GetSongByIdQueryResponse
                {
                    Success = true,
                    Song = songDto
                };
            }
            catch (Exception ex)
            {
                return new GetSongByIdQueryResponse
                {
                    Success = false,
                    Message = $"Error retrieving song: {ex.Message}"
                };
            }
        }

        public async Task<GetSongsQueryResponse> GetSongsAsync(GetSongsQuery query)
        {
            try
            {
                IEnumerable<Song> songs;

                if (!string.IsNullOrEmpty(query.SearchTerm))
                {
                    songs = await _songRepository.SearchSongsAsync(query.SearchTerm);
                }
                else if (query.UserId.HasValue)
                {
                    songs = await _songRepository.GetSongsByUserIdAsync(query.UserId.Value);
                }
                else if (query.GenreId.HasValue)
                {
                    songs = await _songRepository.GetSongsByGenreIdAsync(query.GenreId.Value);
                }
                else if (query.AlbumId.HasValue)
                {
                    songs = await _songRepository.GetSongsByAlbumIdAsync(query.AlbumId.Value);
                }
                else if (query.ArtistId.HasValue)
                {
                    songs = await _songRepository.GetSongsByArtistIdAsync(query.ArtistId.Value);
                }
                else
                {
                    songs = await _songRepository.GetAllSongsAsync();
                }

                var totalCount = songs.Count();
                var pagedSongs = songs
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();

                var songDtos = new List<SongDto>();
                foreach (var song in pagedSongs)
                {
                    songDtos.Add(await MapToSongDto(song));
                }

                return new GetSongsQueryResponse
                {
                    Success = true,
                    Songs = songDtos,
                    TotalCount = totalCount,
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / query.PageSize)
                };
            }
            catch (Exception ex)
            {
                return new GetSongsQueryResponse
                {
                    Success = false,
                    Message = $"Error retrieving songs: {ex.Message}"
                };
            }
        }

        private async Task<SongDto> MapToSongDto(Song song)
        {
            var songDto = new SongDto
            {
                SongId = song.SongId,
                Title = song.Title,
                FileUrl = song.FileUrl,
                CoverImage = song.CoverImage,
                Duration = song.Duration,
                Status = song.Status,
                UploadedAt = song.UploadedAt,
                UpdatedAt = song.UpdatedAt,
                ViewCount = song.ViewCount,
                LikeCount = song.LikeCount,
                CommentCount = song.CommentCount,
                UserId = song.UserId,
                GenreId = song.GenreId,
                AlbumId = song.AlbumId,
                ArtistId = song.ArtistId
            };

            // Load related data
            if (song.User != null)
            {
                songDto.UserName = song.User.UserName;
            }
            else if (song.UserId > 0)
            {
                var user = await _userRepository.GetUserByIdAsync(song.UserId);
                songDto.UserName = user?.UserName;
            }

            if (song.Genre != null)
            {
                songDto.GenreName = song.Genre.GenreName;
            }
            else if (song.GenreId.HasValue)
            {
                var genre = await _genreRepository.GetGenreByIdAsync(song.GenreId.Value);
                songDto.GenreName = genre?.GenreName;
            }

            if (song.Album != null)
            {
                songDto.AlbumName = song.Album.AlbumName;
            }
            else if (song.AlbumId.HasValue)
            {
                var album = await _albumRepository.GetAlbumByIdAsync(song.AlbumId.Value);
                songDto.AlbumName = album?.AlbumName;
            }

            if (song.Artist != null)
            {
                songDto.ArtistName = song.Artist.ArtistName;
            }
            else if (song.ArtistId.HasValue)
            {
                var artist = await _artistRepository.GetArtistByIdAsync(song.ArtistId.Value);
                songDto.ArtistName = artist?.ArtistName;
            }

            return songDto;
        }
    }
}
