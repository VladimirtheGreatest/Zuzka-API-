using Zuzka.Services.DTO;

namespace Zuzka.Services.Contracts
{
    public interface IDocumentService
    {
        Task<Guid> CreateDocumentAsync(DocumentRequestDto document);
        Task<DocumentDto> GetDocumentByIdAsync(Guid documentId);
        Task<Guid> UpdateDocumentAsync(DocumentRequestDto document);
    }
}