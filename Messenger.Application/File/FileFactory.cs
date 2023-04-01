using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;

namespace Messenger.Application
{
    public class FileFactory : IFileFactory
    {
        public DirectMessageImage CreateImage(string fileName, long messageId)
        {
            throw new NotImplementedException();
        }
    }
}
