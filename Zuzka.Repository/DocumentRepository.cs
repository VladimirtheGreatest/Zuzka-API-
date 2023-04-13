using Microsoft.EntityFrameworkCore;
using Zuzka.Data;
using Zuzka.Data.Entities;
using Zuzka.RepositoryContracts;

namespace Zuzka.Repository
{
    public class DocumentRepository : RepositoryBase<DocumentContext, Document>, IDocumentRepository
    {
        public DocumentRepository(DocumentContext repositoryContext) : base(repositoryContext)
        {
            
        }

        public async Task<bool> DocumentExistsAsync(Guid documentId)
        {
            var document = await _context.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentId == documentId);
            if (document != null) { return true; }
            return false;
        }

        public async Task<Document> GetDocumentByIdOrNullAsync(Guid documentId) =>
          await _context.Documents.AsNoTracking().Include(d => d.Data).FirstOrDefaultAsync(d => d.DocumentId == documentId);

        public async Task<Document> GetDocumentByIdOrNullForUpdateAsync(Guid documentId) =>
          await _context.Documents.Include(d => d.Data).FirstOrDefaultAsync(d => d.DocumentId == documentId);

        public async Task<Guid> AddDocumentAsync(Document document)
        {
            await Add(document);
            await _context.SaveChangesAsync();
            Guid documentId = (Guid)document.DocumentId;
            return documentId;
        }
    }
}
