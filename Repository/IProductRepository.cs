using MigraziiRabotaitePj.Models;

namespace MigraziiRabotaitePj.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts(CancellationToken ct);
        Task<Product?> GetProductById(Guid id, CancellationToken ct);
        Task AddAsync(Product product, CancellationToken ct);
        Task DeleteAsync(Product product);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
