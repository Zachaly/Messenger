using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Abstraction
{
    public interface IFileService
    {
        Task<string> SaveProfilePicture(IFormFile file);
        Task<bool> DeleteProfilePicture(string fileName);
        Task<FileStream> GetProfilePicture(string fileName);

        Task<string> SaveDirectMessageImages(IEnumerable<IFormFile> files);
        Task<FileStream> GetDirectMessageImage(string fileName);
    }
}
