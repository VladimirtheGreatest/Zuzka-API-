using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Zuzka.Data.Entities;
using Zuzka.RepositoryContracts;
using Zuzka.Services.Contracts;
using Zuzka.Services.DTO;
using Zuzka.Services.Exceptions;

namespace Zuzka.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly IValidationService _validator;
        public DocumentService(IRepository repository, IMemoryCache cache, IMapper mapper, IValidationService validator)
        {
            _repository = repository;
            _cache = cache;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<DocumentDto> GetDocumentByIdAsync(Guid documentId)
        {
            var key = $"document_{documentId}";
            var cachedDocument = _cache.Get<DocumentDto>(key);
            if (cachedDocument != null)
            {
                return cachedDocument;
            }

            var document = await _repository.Documents.GetDocumentByIdOrNullAsync(documentId);

            if (document != null)
            {
                var documentModel = _mapper.Map<DocumentDto>(document);
                _cache.Set(key, documentModel, TimeSpan.FromMinutes(30));
                return documentModel;
            }

            return default;
        }

        public async Task<Guid> CreateDocumentAsync(DocumentRequestDto document)
        {
            if (document.DocumentId != null && await _repository.Documents.DocumentExistsAsync((Guid)document.DocumentId))
            {
                throw new Exception(Zuzka.Data.Configuration.Constants.documentAlreadyExists);
            }

            Document documentEntity = ValidateAndMapTheEntity(document);

            return await _repository.Documents.AddDocumentAsync(documentEntity);
        }

        public async Task<Guid> UpdateDocumentAsync(DocumentRequestDto document)
        {
            if (document.DocumentId == null || !await _repository.Documents.DocumentExistsAsync((Guid)document.DocumentId))
            {
                throw new Exception(Zuzka.Data.Configuration.Constants.documentNotExists);
            }

            Document documentUpdatedEntity = ValidateAndMapTheEntity(document);

            var entityToEditDb = await _repository.Documents.GetDocumentByIdOrNullForUpdateAsync((Guid)document.DocumentId);

            entityToEditDb.Tags = documentUpdatedEntity.Tags;
            entityToEditDb.Data.Author = documentUpdatedEntity.Data.Author;
            entityToEditDb.Data.PublishedYear = documentUpdatedEntity.Data.PublishedYear;
            entityToEditDb.Data.IsBestseller = documentUpdatedEntity.Data.IsBestseller;
            entityToEditDb.Data.Title = documentUpdatedEntity.Data.Title;
            entityToEditDb.Data.Rating = documentUpdatedEntity.Data.Rating;

            await _repository.SaveAsync();
            return entityToEditDb.DocumentId;
        }

        private Document ValidateAndMapTheEntity(DocumentRequestDto document)
        {
            var result = _validator.Validate(document);
            if (!result.IsValid) { throw new ValidationException(string.Join(", ", result.Errors.Select(error => error.ErrorMessage))); }
            var documentEntity = _mapper.Map<Document>(document);
            return documentEntity;
        }
    }
}
