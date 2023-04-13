using Zuzka.Data;
using Zuzka.RepositoryContracts;

namespace Zuzka.Repository
{
    public class Repository : IRepository
    {
        private DocumentContext _repositoryContext;

        private IDocumentRepository? _documentRepository;

        public Repository(DocumentContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IDocumentRepository Documents
        {
            get
            {
                if (_documentRepository is null)
                    _documentRepository = new DocumentRepository(_repositoryContext);

                return _documentRepository;
            }
        }

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync(true);
    }
}
