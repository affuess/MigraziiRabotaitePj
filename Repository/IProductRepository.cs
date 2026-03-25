using MigraziiRabotaitePj.Models;

namespace MigraziiRabotaitePj.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts(CancellationToken ct);
        Task<Product?> GetProductById(Guid id, CancellationToken ct);
        Task AddAsync(Product product, CancellationToken ct);
        Task<List<Product>> GetAllProducts(CancellationToken cancellationToken = default);
        Task<Product?> GetProductById(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        Task DeleteAsync(Product product);
        Task SaveChangesAsync(CancellationToken ct);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);


    }
}