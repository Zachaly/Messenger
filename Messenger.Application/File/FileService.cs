using Messenger.Application.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Messenger.Application
{
    public class FileService : IFileService
    {
        private readonly string _directMessageImagePath;
        private readonly string _profileImagePath;
        private readonly string _defaultProfileImage;

        public FileService(IConfiguration configuration)
        {
            _directMessageImagePath = configuration["Image:DirectMessage"]!;
            _profileImagePath = configuration["Image:ProfileImage"]!;
            _defaultProfileImage = configuration["Image:Default"]!;
        }

        private Task<bool> DeleteFile(string name, string path)
        {
            try
            {
                var filePath = Path.Combine(path, name);

                File.Delete(filePath);

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        private Task<FileStream> GetFile(string name, string path)
            => Task.FromResult(File.OpenRead(Path.Combine(path, name)));

        public Task<bool> DeleteProfilePicture(string fileName)
        {
            if(!string.IsNullOrEmpty(fileName) && fileName != _defaultProfileImage)
            {
                return DeleteFile(fileName, _profileImagePath);
            }

            return Task.FromResult(true);
        }

        public Task<FileStream> GetDirectMessageImage(string fileName)
            => GetFile(fileName, _directMessageImagePath);

        public Task<FileStream> GetProfilePicture(string? fileName)
        {
            var name = string.IsNullOrEmpty(fileName) ? _defaultProfileImage : fileName;

            return GetFile(name, _profileImagePath);
        }

        public async Task<IEnumerable<string>> SaveDirectMessageImages(IEnumerable<IFormFile> files)
        {
            var names = new List<string>();

            Directory.CreateDirectory(_directMessageImagePath);

            foreach (var file in files)
            {
                var name = $"{Guid.NewGuid()}.png";
                var path = Path.Combine(_directMessageImagePath, name);

                using(var stream = File.Create(path))
                {
                    await file.CopyToAsync(stream);
                }

                names.Add(name);
            }

            return names;
        }

        public async Task<string> SaveProfilePicture(IFormFile file)
        {
            if(file is null)
            {
                return _defaultProfileImage;
            }

            Directory.CreateDirectory(_profileImagePath);

            var name = $"{Guid.NewGuid().ToString()}.png";
            var path = Path.Combine(_profileImagePath, name);

            using(var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            return name;
        }

        public Task<IEnumerable<string>> SaveChatMessageImages(IEnumerable<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public Task<FileStream> GetChatMessageImage(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
