using Messenger.Application.Abstraction;
using Messenger.Domain.Entity;

namespace Messenger.Application
{
    public class FileFactory : IFileFactory
    {
        public ChatMessageImage CreateChatImage(string fileName, long messageId)
            => new ChatMessageImage
            {
                FileName = fileName,
                MessageId = messageId
            };

        public DirectMessageImage CreateImage(string fileName, long messageId)
            => new DirectMessageImage
            {
                MessageId = messageId,
                FileName = fileName,
            };
    }
}
