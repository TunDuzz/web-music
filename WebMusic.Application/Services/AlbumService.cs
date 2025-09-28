using WebMusic.Application.Commands.Albums;
using WebMusic.Application.DTOs;
using WebMusic.Application.Queries.Albums;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;

namespace WebMusic.Application.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IUserRepository _userRepository;
        private readonly IArtistRepository _artistRepository;

        public AlbumService(
            IAlbumRepository albumRepository,
            IUserRepository userRepository,
            IArtistRepository artistRepository)
        {
            _albumRepository = albumRepository;
            _userRepository = userRepository;
            _artistRepository = artistRepository;
        }

        public async Task<CreateAlbumCommandResponse> CreateAlbumAsync(CreateAlbumCommand command)
        {
            try
            {
                var album = new Album
                {
                    AlbumName = command.AlbumName,
                    Description = command.Description,
                    ReleaseDate = command.ReleaseDate,
                    CoverImage = command.CoverImage,
                    UserId = command.UserId,
                    ArtistId = command.ArtistId,
                    CreatedAt = DateTime.UtcNow
                };

                var createdAlbum = await _albumRepository.AddAlbumAsync(album);
                var albumDto = await MapToAlbumDto(createdAlbum);

                return new CreateAlbumCommandResponse
                {
                    Success = true,
                    Message = "Album created successfully",
                    Album = albumDto
                };
            }
            catch (Exception ex)
            {
                return new CreateAlbumCommandResponse
                {
                    Success = false,
                    Message = $"Error creating album: {ex.Message}"
                };
            }
        }

        public async Task<UpdateAlbumCommandResponse> UpdateAlbumAsync(UpdateAlbumCommand command)
        {
            try
            {
                var existingAlbum = await _albumRepository.GetAlbumByIdAsync(command.AlbumId);
                if (existingAlbum == null)
                {
                    return new UpdateAlbumCommandResponse
                    {
                        Success = false,
                        Message = "Album not found"
                    };
                }

                existingAlbum.AlbumName = command.AlbumName;
                existingAlbum.Description = command.Description;
                existingAlbum.ReleaseDate = command.ReleaseDate;
                existingAlbum.CoverImage = command.CoverImage;
                existingAlbum.ArtistId = command.ArtistId;
                existingAlbum.UpdatedAt = DateTime.UtcNow;

                var updatedAlbum = await _albumRepository.UpdateAlbumAsync(existingAlbum);
                var albumDto = await MapToAlbumDto(updatedAlbum);

                return new UpdateAlbumCommandResponse
                {
                    Success = true,
                    Message = "Album updated successfully",
                    Album = albumDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateAlbumCommandResponse
                {
                    Success = false,
                    Message = $"Error updating album: {ex.Message}"
                };
            }
        }

        public async Task<DeleteAlbumCommandResponse> DeleteAlbumAsync(DeleteAlbumCommand command)
        {
            try
            {
                var existingAlbum = await _albumRepository.GetAlbumByIdAsync(command.AlbumId);
                if (existingAlbum == null)
                {
                    return new DeleteAlbumCommandResponse
                    {
                        Success = false,
                        Message = "Album not found"
                    };
                }

                await _albumRepository.DeleteAlbumAsync(command.AlbumId);

                return new DeleteAlbumCommandResponse
                {
                    Success = true,
                    Message = "Album deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DeleteAlbumCommandResponse
                {
                    Success = false,
                    Message = $"Error deleting album: {ex.Message}"
                };
            }
        }

        public async Task<GetAlbumByIdQueryResponse> GetAlbumByIdAsync(GetAlbumByIdQuery query)
        {
            try
            {
                var album = await _albumRepository.GetAlbumByIdAsync(query.AlbumId);
                if (album == null)
                {
                    return new GetAlbumByIdQueryResponse
                    {
                        Success = false,
                        Message = "Album not found"
                    };
                }

                var albumDto = await MapToAlbumDto(album);

                return new GetAlbumByIdQueryResponse
                {
                    Success = true,
                    Album = albumDto
                };
            }
            catch (Exception ex)
            {
                return new GetAlbumByIdQueryResponse
                {
                    Success = false,
                    Message = $"Error retrieving album: {ex.Message}"
                };
            }
        }

        public async Task<GetAlbumsQueryResponse> GetAlbumsAsync(GetAlbumsQuery query)
        {
            try
            {
                IEnumerable<Album> albums;

                if (!string.IsNullOrEmpty(query.SearchTerm))
                {
                    albums = await _albumRepository.SearchAlbumsAsync(query.SearchTerm);
                }
                else if (query.UserId.HasValue)
                {
                    albums = await _albumRepository.GetAlbumsByUserIdAsync(query.UserId.Value);
                }
                else if (query.ArtistId.HasValue)
                {
                    albums = await _albumRepository.GetAlbumsByArtistIdAsync(query.ArtistId.Value);
                }
                else
                {
                    albums = await _albumRepository.GetAllAlbumsAsync();
                }

                var totalCount = albums.Count();
                var pagedAlbums = albums
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();

                var albumDtos = new List<AlbumDto>();
                foreach (var album in pagedAlbums)
                {
                    albumDtos.Add(await MapToAlbumDto(album));
                }

                return new GetAlbumsQueryResponse
                {
                    Success = true,
                    Albums = albumDtos,
                    TotalCount = totalCount,
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / query.PageSize)
                };
            }
            catch (Exception ex)
            {
                return new GetAlbumsQueryResponse
                {
                    Success = false,
                    Message = $"Error retrieving albums: {ex.Message}"
                };
            }
        }

        private async Task<AlbumDto> MapToAlbumDto(Album album)
        {
            var albumDto = new AlbumDto
            {
                AlbumId = album.AlbumId,
                AlbumName = album.AlbumName,
                Description = album.Description,
                ReleaseDate = album.ReleaseDate,
                CoverImage = album.CoverImage,
                CreatedAt = album.CreatedAt,
                UpdatedAt = album.UpdatedAt,
                SongCount = album.SongCount,
                UserId = album.UserId,
                ArtistId = album.ArtistId
            };

            // Load related data
            if (album.User != null)
            {
                albumDto.UserName = album.User.UserName;
            }
            else if (album.UserId > 0)
            {
                var user = await _userRepository.GetUserByIdAsync(album.UserId);
                albumDto.UserName = user?.UserName;
            }

            if (album.Artist != null)
            {
                albumDto.ArtistName = album.Artist.ArtistName;
            }
            else if (album.ArtistId.HasValue)
            {
                var artist = await _artistRepository.GetArtistByIdAsync(album.ArtistId.Value);
                albumDto.ArtistName = artist?.ArtistName;
            }

            return albumDto;
        }
    }
}
