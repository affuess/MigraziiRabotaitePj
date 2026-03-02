using Microsoft.EntityFrameworkCore;
using MigraziiRabotaitePj.Data;
using MigraziiRabotaitePj.Models;

namespace MigraziiRabotaitePj.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBase _context;

        public ProductRepository(DataBase context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts(CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.Characteristics)
                .ToListAsync(ct);
        }

        public async Task<Product?> GetProductById(Guid id, CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.Characteristics)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task AddAsync(Product product, CancellationToken ct)
        {
            await _context.Products.AddAsync(product, ct);
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
