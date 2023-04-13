using AutoMapper;
using CodeRama.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Zuzka.RepositoryContracts;
using Zuzka.Services;
using Zuzka.Services.Contracts;
using Zuzka.Services.DTO;

namespace Zuzka.Tests
{
    public class UnitTestsApi
    {
        [Fact]
        public async Task GetDocumentById_ShouldReturnDocumentAndCorrectStatusCode()
        {
            //arrange 
            Mock<IDocumentService> documentServiceMock;
            Mock<ILogger<DocumentsController>> loggerMock;
            ArrangeDependencies(out documentServiceMock, out loggerMock);

            var documentId = Guid.NewGuid();
            Mock<DocumentDto> documentMock = new Mock<DocumentDto>();
            documentMock.Object.DocumentId = documentId;

            documentServiceMock.Setup(x => x.GetDocumentByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync(documentMock.Object);

            var controller = SetupDocumentController(loggerMock, documentServiceMock);

            //act 
            var result = await controller.GetDocumentById(Guid.NewGuid());
      
            //assert
            ((DocumentDto)((ObjectResult)result.Result).Value).DocumentId.Should().Be(documentId);
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetDocumentById_ShouldReturnNoContentIfDocumentNotFound()
        {
            //arrange 
            Mock<IDocumentService> documentServiceMock;
            Mock<ILogger<DocumentsController>> loggerMock;
            ArrangeDependencies(out documentServiceMock, out loggerMock);

            documentServiceMock.Setup(x => x.GetDocumentByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((DocumentDto)null);

            var controller = SetupDocumentController(loggerMock, documentServiceMock);

            //act 
            var result = await controller.GetDocumentById(Guid.NewGuid());

            //assert
            var noContentResult = result.Result as NoContentResult;
            result.Value.Should().BeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task CreateDocumentShould_CreateDocumentAndReturnId()
        {
            //arrange 
            Guid documentId;
            Mock<IDocumentService> documentServiceMock;
            Mock<ILogger<DocumentsController>> loggerMock;
            Mock<DocumentRequestDto> documentMock;
            ArrangeDependenciesForCreateOrEdit(out documentId, out documentServiceMock, out loggerMock, out documentMock);

            documentServiceMock.Setup(x => x.CreateDocumentAsync(It.IsAny<DocumentRequestDto>())).ReturnsAsync(documentId);
            var controller = SetupDocumentController(loggerMock, documentServiceMock);

            //act 
            var result = await controller.CreateDocument(documentMock.Object);
            var resultValue = (((ObjectResult)result).Value);
            //assert
            result.Should().NotBeNull();
            resultValue.Should().Be(documentId.ToString());
        }

        [Fact]
        public async Task EditDocumentShould_EditDocumentAndReturnId()
        {
            //arrange 
            Guid documentId;
            Mock<IDocumentService> documentServiceMock;
            Mock<ILogger<DocumentsController>> loggerMock;
            Mock<DocumentRequestDto> documentMock;
            ArrangeDependenciesForCreateOrEdit(out documentId, out documentServiceMock, out loggerMock, out documentMock);

            documentServiceMock.Setup(x => x.UpdateDocumentAsync(It.IsAny<DocumentRequestDto>())).ReturnsAsync(documentId);
            var controller = SetupDocumentController(loggerMock, documentServiceMock);

            //act 
            var result = await controller.EditDocument(documentMock.Object);
            var resultValue = (((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value);
            //assert
            result.Should().NotBeNull();
            resultValue.Should().Be(documentId.ToString());
        }

        [Fact]
        public async Task DocumentService_Should_NotCreateDocumentIf_Document_Already_Exists()
        {
            //arrange 
            Mock<IRepository> repositoryMock;
            Mock<IMemoryCache> cacheMock;
            Mock<IMapper> mapperMock;
            Mock<IValidationService> validationServiceMock;
            Mock<DocumentRequestDto> documentMock;
            ArrangeDocumentService(out repositoryMock, out cacheMock, out mapperMock, out validationServiceMock, out documentMock);

            repositoryMock.Setup(x => x.Documents.DocumentExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            //act 
            var validationService = new DocumentService(repositoryMock.Object, cacheMock.Object, mapperMock.Object,validationServiceMock.Object );

            try
            {
                await validationService.CreateDocumentAsync(documentMock.Object);
            }
            catch (Exception ex)
            {
                //assert
                ex.Message.Should().Be(Zuzka.Data.Configuration.Constants.documentAlreadyExists);
            }
        }

        [Fact]
        public async Task DocumentService_Should_NotUpdateDocumentIf_Document_Does_Not_Exists()
        {
            //arrange 
            Mock<IRepository> repositoryMock;
            Mock<IMemoryCache> cacheMock;
            Mock<IMapper> mapperMock;
            Mock<IValidationService> validationServiceMock;
            Mock<DocumentRequestDto> documentMock;
            ArrangeDocumentService(out repositoryMock, out cacheMock, out mapperMock, out validationServiceMock, out documentMock);

            repositoryMock.Setup(x => x.Documents.DocumentExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            //act 
            var validationService = new DocumentService(repositoryMock.Object, cacheMock.Object, mapperMock.Object, validationServiceMock.Object);

            try
            {
                await validationService.UpdateDocumentAsync(documentMock.Object);
            }
            catch (Exception ex)
            {
                //assert
                ex.Message.Should().Be(Zuzka.Data.Configuration.Constants.documentNotExists);
            }
        }

        [Theory]
        [InlineData(698, 1999, Zuzka.Data.Configuration.Constants.ratingValidationMessage, false)]
        [InlineData(48, 145, Zuzka.Data.Configuration.Constants.publishedYearValidationMessage, false)]
        [InlineData(40, 1999, "", true)]
        public void DocumentRules_Should_Be_Validated(int exampleRating, int examplePublishedYear, string expectedValidationErrorMessage, bool expectedValid)
        {
            //arrange 
            var documentId = Guid.NewGuid();
            string[] tags = { "Volvo", "BMW", "Ford", "Vodka" };
            var documentMock = Mock.Of<DocumentRequestDto>(m =>
                     m.Rating == exampleRating &&
                     m.PublishedYear == examplePublishedYear && m.DocumentId == documentId &&
                     m.Tags == tags);

            //act
            var validationService = new ValidationService();
            var result = validationService.Validate(documentMock);

            //assert
            result.Should().NotBeNull();
            result.IsValid.Should().Be(expectedValid);
            if (!result.IsValid)
            {
                result.Errors.Should().NotBeEmpty();
                result.Errors.First().ErrorMessage.Should().Be(expectedValidationErrorMessage);
            }
        }

        private DocumentsController SetupDocumentController(Mock<ILogger<DocumentsController>> loggerMock, Mock<IDocumentService> documentServiceMock)
        {
            var controller = new DocumentsController(loggerMock.Object, documentServiceMock.Object);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext { HttpContext = new DefaultHttpContext() };
            //setupheaders here if needed or api key if added
            return controller;
        }

        private static void ArrangeDocumentService(out Mock<IRepository> repositoryMock, out Mock<IMemoryCache> cacheMock, out Mock<IMapper> mapperMock, out Mock<IValidationService> validationServiceMock, out Mock<DocumentRequestDto> documentMock)
        {
            var documentId = Guid.NewGuid();
            repositoryMock = new Mock<IRepository>();
            cacheMock = new Mock<IMemoryCache>();
            mapperMock = new Mock<IMapper>();
            validationServiceMock = new Mock<IValidationService>();
            Mock<ILogger<DocumentsController>> loggerMock = new Mock<ILogger<DocumentsController>>();
            documentMock = new Mock<DocumentRequestDto>();
            documentMock.Object.DocumentId = documentId;
        }

        private static void ArrangeDependenciesForCreateOrEdit(out Guid documentId, out Mock<IDocumentService> documentServiceMock, out Mock<ILogger<DocumentsController>> loggerMock, out Mock<DocumentRequestDto> documentMock)
        {
            documentId = Guid.NewGuid();
            documentServiceMock = new Mock<IDocumentService>();
            loggerMock = new Mock<ILogger<DocumentsController>>();
            documentMock = new Mock<DocumentRequestDto>();
            documentMock.Object.DocumentId = documentId;
        }

        private static void ArrangeDependencies(out Mock<IDocumentService> documentServiceMock, out Mock<ILogger<DocumentsController>> loggerMock)
        {
            documentServiceMock = new Mock<IDocumentService>();
            loggerMock = new Mock<ILogger<DocumentsController>>();
        }
    }
}