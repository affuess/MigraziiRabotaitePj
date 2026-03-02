using MigraziiRabotaitePj.Data;
using MigraziiRabotaitePj.Models;
using Microsoft.EntityFrameworkCore;

namespace MigraziiRabotaitePj.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBase _db;

        public ProductRepository(DataBase db)
        {
            _db = db;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _db.Products.AddAsync(product, cancellationToken);
        }

        public async Task DeleteAsync(Product product)
        {
            _db.Products.Remove(product);
            await Task.CompletedTask;
        }

        public async Task<List<Product>> GetAllProducts(CancellationToken cancellationToken = default)
        {
            return await _db.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetProductById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Products
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}