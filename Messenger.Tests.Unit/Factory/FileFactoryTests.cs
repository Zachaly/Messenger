using Messenger.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Factory
{
    public class FileFactoryTests
    {
        private readonly FileFactory _fileFactory;

        public FileFactoryTests()
        {
            _fileFactory = new FileFactory();
        }

        [Fact]
        public void CreateImage_Creates_Proper_Entity()
        {
            const string FileName = "name";
            const long MessageId = 1;

            var image = _fileFactory.CreateImage(FileName, MessageId);

            Assert.Equal(FileName, image.FileName);
            Assert.Equal(MessageId, image.MessageId);
        }
    }
}
