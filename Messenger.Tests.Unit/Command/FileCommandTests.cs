using Messenger.Application.Abstraction;
using Messenger.Application.Command;
using Messenger.Database.Repository;
using Messenger.Domain.Entity;
using Messenger.Models.ChatMessageImage.Request;
using Messenger.Models.Response;
using Messenger.Models.User.Request;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Messenger.Tests.Unit.Command
{
    public class FileCommandTests
    {
        private readonly Mock<IFileService> _fileService;

        public FileCommandTests()
        {
            _fileService = new Mock<IFileService>();
        }

        [Fact]
        public async Task GetDirectMessageImageQuery_Success()
        {
            var image = new DirectMessageImage { FileName = "file" };

            var imageRepository = new Mock<IDirectMessageImageRepository>();
            imageRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(image);

            _fileService.Setup(x => x.GetDirectMessageImage(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var query = new GetDirectMessageImageQuery { ImageId = 1 };

            var res = await new GetDirectMessageImageHandler(_fileService.Object, imageRepository.Object).Handle(query, default);

            Assert.Null(res);
        }

        [Fact]
        public async Task GetProfileImageQuery_Success()
        {
            var user = new User { ProfileImage = "image" };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>())).ReturnsAsync(user);

            _fileService.Setup(x => x.GetProfilePicture(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var query = new GetProfileImageQuery { UserId = 1 };
            var res = await new GetProfileImageHandler(_fileService.Object, userRepository.Object).Handle(query, default);

            Assert.Null(res);
        }

        [Fact]
        public async Task GetChatMessageImageQuery_Success()
        {
            var image = new ChatMessageImage { FileName = "file " };

            var imageRepository = new Mock<IChatMessageImageRepository>();
            imageRepository.Setup(x => x.GetAsync(It.IsAny<GetChatMessageImageRequest>()))
                .ReturnsAsync(new List<ChatMessageImage> { image });

            _fileService.Setup(x => x.GetChatMessageImage(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var query = new GetChatMessageImageQuery { ImageId = 1 };
            var res = await new GetChatMessageImageHandler(_fileService.Object, imageRepository.Object).Handle(query, default);

            Assert.Null(res);
        }

        [Fact]
        public async Task SaveProfileImageCommand_Success()
        {
            var user = new User { ProfileImage = "img" };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) =>
                {
                    user.ProfileImage = request.ProfileImage;
                });
            userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            _fileService.Setup(x => x.SaveProfilePicture(It.IsAny<IFormFile>()))
                .ReturnsAsync((IFormFile file) => file.Name);
            _fileService.Setup(x => x.DeleteProfilePicture(It.IsAny<string>()));

            const string FileName = "filename";
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(x => x.Name).Returns(FileName);

            var command = new SaveProfileImageCommand { UserId = 1, File = mockFile.Object };

            var res = await new SaveProfileImageHandler(_fileService.Object, responseFactory.Object, userRepository.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(FileName, user.ProfileImage);
        }

        [Fact]
        public async Task SaveProfileImageCommand_ExceptionThrown_Fail()
        {
            var user = new User();

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) =>
                {
                    user.ProfileImage = request.ProfileImage;
                });
            userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel { Success = false, Error = msg });

            const string ErrorMessage = "Error";

            _fileService.Setup(x => x.SaveProfilePicture(It.IsAny<IFormFile>()))
                .Callback(() => throw new Exception(ErrorMessage));
            _fileService.Setup(x => x.DeleteProfilePicture(It.IsAny<string>()));

            const string FileName = "filename";
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(x => x.Name).Returns(FileName);

            var command = new SaveProfileImageCommand { UserId = 1, File = mockFile.Object };

            var res = await new SaveProfileImageHandler(_fileService.Object, responseFactory.Object, userRepository.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEqual(FileName, user.ProfileImage);
            Assert.Equal(ErrorMessage, res.Error);
        }

        [Fact]
        public async Task SaveDirectMessageImagesCommand_Success()
        {
            var images = new List<DirectMessageImage>();

            var repository = new Mock<IDirectMessageImageRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<DirectMessageImage>()))
                .Callback((DirectMessageImage img) => images.Add(img));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });
            
            var fileFactory = new Mock<IFileFactory>();
            fileFactory.Setup(x => x.CreateImage(It.IsAny<string>(), It.IsAny<long>()))
                .Returns((string file, long id) => new DirectMessageImage { MessageId = id, FileName = file });

            _fileService.Setup(x => x.SaveDirectMessageImages(It.IsAny<IEnumerable<IFormFile>>()))
                .ReturnsAsync((IEnumerable<IFormFile> files) => files.Select(x => x.Name));

            var files = new List<IFormFile>();

            for(int i = 0; i < 5; i++)
            {
                var file = new Mock<IFormFile>();
                file.Setup(x => x.Name).Returns($"img{i}");
                files.Add(file.Object);
            }

            var command = new SaveDirectMessageImagesCommand { MessageId = 1, Files = files };
            var res = await new SaveDirectMessageImagesHandler(_fileService.Object, responseFactory.Object, fileFactory.Object, repository.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Equivalent(files.Select(x => x.Name), images.Select(x => x.FileName));
            Assert.All(images, x => Assert.Equal(command.MessageId, x.MessageId));
        }

        [Fact]
        public async Task SaveDirectMessageImagesCommand_ExceptionThrowb_Fail()
        {
            var images = new List<DirectMessageImage>();

            var repository = new Mock<IDirectMessageImageRepository>();
            repository.Setup(x => x.InsertAsync(It.IsAny<DirectMessageImage>()))
                .Callback((DirectMessageImage img) => images.Add(img));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });


            const string Error = "Error";
            var fileFactory = new Mock<IFileFactory>();
            fileFactory.Setup(x => x.CreateImage(It.IsAny<string>(), It.IsAny<long>()))
                .Callback(() => throw new Exception(Error));

            _fileService.Setup(x => x.SaveDirectMessageImages(It.IsAny<IEnumerable<IFormFile>>()))
                .ReturnsAsync((IEnumerable<IFormFile> files) => files.Select(x => x.Name));

            var files = new List<IFormFile>();

            for (int i = 0; i < 5; i++)
            {
                var file = new Mock<IFormFile>();
                file.Setup(x => x.Name).Returns($"img{i}");
                files.Add(file.Object);
            }

            var command = new SaveDirectMessageImagesCommand { MessageId = 1, Files = files };
            var res = await new SaveDirectMessageImagesHandler(_fileService.Object, responseFactory.Object, fileFactory.Object, repository.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
            Assert.Empty(images);
        }

        [Fact]
        public async Task SaveChatImageCommand_Success()
        {
            var images = new List<ChatMessageImage>();

            var imageRepository = new Mock<IChatMessageImageRepository>();
            imageRepository.Setup(x => x.InsertAsync(It.IsAny<ChatMessageImage>()))
                .Callback((ChatMessageImage img) => images.Add(img));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel() { Success = true });

            var fileFactory = new Mock<IFileFactory>();
            fileFactory.Setup(x => x.CreateChatImage(It.IsAny<string>(), It.IsAny<long>()))
                .Returns((string name, long id) => new ChatMessageImage { FileName = name, Id = id });

            _fileService.Setup(x => x.SaveChatMessageImages(It.IsAny<IEnumerable<IFormFile>>()))
                .ReturnsAsync((IEnumerable<IFormFile> files) => files.Select(x => x.Name));

            var files = new List<IFormFile>();

            for (int i = 0; i < 5; i++)
            {
                var file = new Mock<IFormFile>();
                file.Setup(x => x.Name).Returns($"img{i}");
                files.Add(file.Object);
            }

            var command = new SaveChatMessageImageCommand { MessageId = 1, Files = files };
            var res = await new SaveChatMessageImageHandler(_fileService.Object, fileFactory.Object, imageRepository.Object, responseFactory.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(images.Count, files.Count);
            Assert.Equivalent(images.Select(x => x.FileName), files.Select(x => x.Name));
        }

        [Fact]
        public async Task SaveChatImageCommand_ExceptionThrown_Fail()
        {
            var images = new List<ChatMessageImage>();

            const string Error = "error";
            var imageRepository = new Mock<IChatMessageImageRepository>();
            imageRepository.Setup(x => x.InsertAsync(It.IsAny<ChatMessageImage>()))
                .Callback((ChatMessageImage img) => throw new Exception(Error));

            var responseFactory = new Mock<IResponseFactory>();
            responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string msg) => new ResponseModel() { Success = false, Error = msg });

            var fileFactory = new Mock<IFileFactory>();
            fileFactory.Setup(x => x.CreateChatImage(It.IsAny<string>(), It.IsAny<long>()))
                .Returns((string name, long id) => new ChatMessageImage { FileName = name, Id = id });

            _fileService.Setup(x => x.SaveChatMessageImages(It.IsAny<IEnumerable<IFormFile>>()))
                .ReturnsAsync((IEnumerable<IFormFile> files) => files.Select(x => x.Name));

            var files = new List<IFormFile>();

            for (int i = 0; i < 5; i++)
            {
                var file = new Mock<IFormFile>();
                file.Setup(x => x.Name).Returns($"img{i}");
                files.Add(file.Object);
            }

            var command = new SaveChatMessageImageCommand { MessageId = 1, Files = files };
            var res = await new SaveChatMessageImageHandler(_fileService.Object, fileFactory.Object, imageRepository.Object, responseFactory.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.Empty(images);
            Assert.Equal(Error, res.Error);
        }
    }
}
