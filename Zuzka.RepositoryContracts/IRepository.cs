

namespace Zuzka.RepositoryContracts
{
    public interface IRepository
    {
        IDocumentRepository? Documents { get; }
        Task SaveAsync();
    }
}
