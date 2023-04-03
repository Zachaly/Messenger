using Messenger.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Application.Abstraction
{
    public interface IFileFactory
    {
        DirectMessageImage CreateImage(string fileName, long messageId);
    }
}
