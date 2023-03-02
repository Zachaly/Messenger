using FluentValidation.Results;
using Messenger.Application;

namespace Messenger.Tests.Unit.Factory
{
    public class ResponseFactoryTests
    {
        private readonly ResponseFactory _factory;

        public ResponseFactoryTests()
        {
            _factory = new ResponseFactory();
        }

        [Fact]
        public void CreateSuccess()
        {
            var response = _factory.CreateSuccess();

            Assert.True(response.Success);
        }

        [Fact]
        public void CreateFailure()
        {
            const string Message = "Error";
            var response = _factory.CreateFailure(Message);

            Assert.False(response.Success);
            Assert.Equal(Message, response.Error);
        }

        [Fact]
        public void CreateSuccess_DataResponse()
        {
            const int Data = 2137;
            var response = _factory.CreateSuccess(Data);

            Assert.True(response.Success);
            Assert.Equal(Data, response.Data);
        }

        [Fact]
        public void CreateFailure_DataResponse()
        {
            const string Error = "Error";
            var response = _factory.CreateFailure<int>(Error);

            Assert.False(response.Success);
            Assert.Equal(default, response.Data);
        }

        [Fact]
        public void CreateValidationError()
        {
            var validation = new ValidationResult();
            validation.Errors = new List<ValidationFailure>
            {
                new ValidationFailure
                {
                    PropertyName = "Prop",
                    ErrorMessage = "message"
                },
                new ValidationFailure
                {
                    PropertyName = "Prop2",
                    ErrorMessage = "message2"
                }
            };

            var response = _factory.CreateValidationError(validation);

            Assert.False(response.Success);
            Assert.Equivalent(validation.Errors.Select(x => x.PropertyName), response.Errors.Keys);
            Assert.Equivalent(validation.Errors.Select(x => x.ErrorMessage), response.Errors.SelectMany(x => x.Value));
        }

        [Fact]
        public void CreateCreatedResponse()
        {
            const int Id = 1;
            var response = _factory.CreateCreatedSuccess(Id);

            Assert.True(response.Success);
            Assert.Equal(Id, response.NewEntityId);
        }
    }
}
