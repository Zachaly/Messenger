using Messenger.Domain.Entity;

namespace Messenger.Application.Abstraction
{
    public interface IFileFactory
    {
        DirectMessageImage CreateImage(string fileName, long messageId);
        ChatMessageFactory CreateChatImage(string fileName, long messageId);
    }
}
