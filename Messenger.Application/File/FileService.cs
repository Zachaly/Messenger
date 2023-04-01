using Messenger.Application.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Messenger.Application
{
    public class FileService : IFileService
    {
        public FileService(IConfiguration configuration)
        {

        }

        public Task<bool> DeleteProfilePicture(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<FileStream> GetDirectMessageImage(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<FileStream> GetProfilePicture(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> SaveDirectMessageImages(IEnumerable<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveProfilePicture(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
