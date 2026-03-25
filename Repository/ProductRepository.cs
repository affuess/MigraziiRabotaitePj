using Microsoft.EntityFrameworkCore;
using MigraziiRabotaitePj.Data;
using MigraziiRabotaitePj.Models;
using Microsoft.EntityFrameworkCore;

namespace MigraziiRabotaitePj.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBase _context;
        private readonly DataBase _db;

        public ProductRepository(DataBase context)
        public ProductRepository(DataBase db)
        {
            _context = context;
            _db = db;
        }

        public async Task<List<Product>> GetAllProducts(CancellationToken ct)
        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(p => p.Characteristics)
                .ToListAsync(ct);
            await _db.Products.AddAsync(product, cancellationToken);
        }

        public async Task<Product?> GetProductById(Guid id, CancellationToken ct)
        public async Task DeleteAsync(Product product)
        {
            return await _context.Products
                .Include(p => p.Characteristics)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
            _db.Products.Remove(product);
            await Task.CompletedTask;
        }

        public async Task AddAsync(Product product, CancellationToken ct)
        public async Task<List<Product>> GetAllProducts(CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(product, ct);
            return await _db.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteAsync(Product product)
        public async Task<Product?> GetProductById(Guid id, CancellationToken cancellationToken = default)
        {
            _context.Products.Remove(product);
            return await _db.Products
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(ct);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}