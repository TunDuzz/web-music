# WebMusic - Hệ thống Web Nghe Nhạc

## Tổng quan

WebMusic là một hệ thống web nghe nhạc được xây dựng theo kiến trúc Clean Architecture với ASP.NET Core, Entity Framework Core và SQL Server.

## Kiến trúc

Dự án được tổ chức theo Clean Architecture với các layer sau:

### 1. Domain Layer (WebMusic.Domain)
- **Entities**: Các thực thể chính của hệ thống (User, Song, Album, Playlist, Genre, Comment, Like, Artist, PlayHistory, Follow)
- **Interfaces**: Các interface cho repositories
- **Value Objects**: Các đối tượng giá trị (Email, Duration)
- **Common**: Các base classes và interfaces chung

### 2. Application Layer (WebMusic.Application)
- **DTOs**: Data Transfer Objects
- **Commands/Queries**: Các lệnh và truy vấn theo CQRS pattern
- **Services**: Business logic services
- **Validators**: FluentValidation validators

### 3. Infrastructure Layer (WebMusic.Infrastructure)
- **Data**: DbContext và cấu hình Entity Framework
- **Repositories**: Triển khai các repository interfaces
- **DependencyInjection**: Cấu hình DI container

### 4. API Layer (WebMusic.API)
- **Controllers**: REST API controllers
- **Middleware**: Custom middleware cho validation và exception handling
- **Configuration**: Cấu hình ứng dụng

## Tính năng chính

### Quản lý Người dùng
- Đăng ký/Đăng nhập
- Quản lý profile
- Theo dõi người dùng khác

### Quản lý Nhạc
- Upload và quản lý bài hát
- Phân loại theo thể loại
- Quản lý album
- Tìm kiếm và lọc

### Quản lý Playlist
- Tạo và quản lý playlist
- Thêm/xóa bài hát khỏi playlist
- Chia sẻ playlist

### Tương tác
- Like/Unlike bài hát
- Bình luận
- Lịch sử nghe nhạc

## Cài đặt và Chạy

### Yêu cầu hệ thống
- .NET 9.0 SDK
- SQL Server (LocalDB hoặc SQL Server)
- Visual Studio 2022 hoặc VS Code

### Các bước cài đặt

1. **Clone repository**
```bash
git clone <repository-url>
cd WebMusic
```

2. **Restore packages**
```bash
dotnet restore
```

3. **Cấu hình database**
- Cập nhật connection string trong `appsettings.json`
- Chạy migrations:
```bash
dotnet ef database update --project WebMusic.Infrastructure --startup-project WebMusic.API
```

4. **Chạy ứng dụng**
```bash
dotnet run --project WebMusic.API
```

5. **Truy cập API**
- Swagger UI: `https://localhost:7000/swagger`
- API Base URL: `https://localhost:7000/api`

## API Endpoints

### Songs
- `GET /api/songs` - Lấy danh sách bài hát
- `GET /api/songs/{id}` - Lấy bài hát theo ID
- `POST /api/songs` - Tạo bài hát mới
- `PUT /api/songs/{id}` - Cập nhật bài hát
- `DELETE /api/songs/{id}` - Xóa bài hát

### Users
- `GET /api/users` - Lấy danh sách người dùng
- `GET /api/users/{id}` - Lấy người dùng theo ID
- `POST /api/users` - Tạo người dùng mới
- `PUT /api/users/{id}` - Cập nhật người dùng
- `DELETE /api/users/{id}` - Xóa người dùng

### Albums
- `GET /api/albums` - Lấy danh sách album
- `GET /api/albums/{id}` - Lấy album theo ID
- `POST /api/albums` - Tạo album mới
- `PUT /api/albums/{id}` - Cập nhật album
- `DELETE /api/albums/{id}` - Xóa album

## Cấu trúc Database

### Bảng chính
- **Users**: Thông tin người dùng
- **Songs**: Thông tin bài hát
- **Albums**: Thông tin album
- **Playlists**: Thông tin playlist
- **Genres**: Thể loại nhạc
- **Artists**: Nghệ sĩ
- **Comments**: Bình luận
- **Likes**: Lượt thích
- **PlayHistories**: Lịch sử nghe nhạc
- **Follows**: Theo dõi người dùng
- **PlaylistSongs**: Quan hệ nhiều-nhiều giữa playlist và song

## Validation

Hệ thống sử dụng FluentValidation để validate dữ liệu đầu vào:
- Validation cho tất cả các command/query
- Custom error messages
- Automatic validation middleware

## Error Handling

- Global exception handling middleware
- Structured error responses
- Logging cho debugging

## CORS

Cấu hình CORS để cho phép frontend kết nối từ bất kỳ origin nào (chỉ dành cho development).

## Phát triển thêm

### Thêm tính năng mới
1. Tạo entity trong Domain layer
2. Tạo repository interface
3. Triển khai repository trong Infrastructure
4. Tạo DTOs và Commands/Queries
5. Tạo service trong Application layer
6. Tạo controller trong API layer
7. Tạo migration cho database changes

### Thêm validation
1. Tạo validator class kế thừa từ `AbstractValidator<T>`
2. Đăng ký validator trong DI container
3. Validation sẽ tự động được áp dụng

## License

MIT License
