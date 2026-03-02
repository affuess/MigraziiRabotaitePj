using MigraziiRabotaitePj.Models;

namespace MigraziiRabotaitePj.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts(CancellationToken cancellationToken = default);
        Task<Product?> GetProductById(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        Task DeleteAsync(Product product);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);


    }
}