using Zuzka.Data.Entities;

namespace Zuzka.RepositoryContracts
{
    public interface IDocumentRepository
    {

        Task<bool> DocumentExistsAsync(Guid documentId);

        /// <summary>
        /// Get document by Id or returns null, IMPORTANT to keep as no tracking to speed up querying.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<Document> GetDocumentByIdOrNullAsync(Guid documentId);

        /// <summary>
        /// Add new document entity after validation.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
         Task<Guid> AddDocumentAsync(Document document);

        /// <summary>
        /// Used to edit the entity details, IMPORTANT to track the entity changes so we can modify it.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<Document> GetDocumentByIdOrNullForUpdateAsync(Guid documentId);
    }
}
