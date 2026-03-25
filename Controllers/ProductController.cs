using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MigraziiRabotaitePj.Data;
using MigraziiRabotaitePj.DTO;
using MigraziiRabotaitePj.Models;
using MigraziiRabotaitePj.Repository;
using MigraziiRabotaitePj.Services;

using Microsoft.AspNetCore.Authorization;
namespace MigraziiRabotaitePj.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        private readonly IMapper _maper;
        private readonly DataBase _db;
        private readonly IFileStorage _fileStorage;

        public ProductController(
            IProductRepository repo,
            IMapper mapper,
            IFileStorage fileStorage)

        public ProductController(DataBase db, IMapper mapper, IFileStorage fileStorage)
        {
            _repo = repo;
            _mapper = mapper;
            _maper = mapper;
            _db = db;
            _fileStorage = fileStorage;
        }




        //[HttpGet]
        //public async Task<ActionResult<List<ProductReadDTO>>> GetAll()
        //{
        //    var items = await _db.Products.ToListAsync();
        //    return Ok(_maper.Map<ProductReadDTO>(items));
        //}


        [HttpGet]
        public async Task<ActionResult<List<ProductReadDTO>>> GetAll(CancellationToken ct)
        public async Task<ActionResult<PageResult<ProductReadDTO>>> Get([FromQuery] ProductQuery q)
        {
            var items = await _repo.GetAllProducts(ct);
            var result = _mapper.Map<List<ProductReadDTO>>(items);
            //filtering
            IQueryable<Product> query = _db.Products
                .AsNoTracking();// не відслідковує зміни об'єкту, для оптимізації
            if (!string.IsNullOrWhiteSpace(q.Brand))
            {
                query = query.Where(p =>
                p.Brand == q.Brand);
            }
            if (!string.IsNullOrWhiteSpace(q.State))
            {
                query = query.Where(p =>
                p.Characteristics.State == q.State);
            }

            if (q.PriceFrom > 0)
            {
                query = query.Where(p =>
                p.Price >= q.PriceFrom);

            }
            if (q.PriceTo > 0)
            {
                query = query.Where(p =>
                p.Price <= q.PriceTo);

            }

            //Sorting
            bool desc = string.Equals(q.SortDir,
                "desc",
                StringComparison.OrdinalIgnoreCase);

            query = q.SortBy.ToLower() switch
            {
                "price" => desc ?
                query.OrderByDescending(o => o.Price)
                : query.OrderBy(o => o.Price),

                "quantity" => desc ?
                query.OrderByDescending(o => o.Quantity)
                : query.OrderBy(o => o.Quantity),

                "brand" => desc ?
                query.OrderByDescending(o => o.Brand)
                : query.OrderBy(o => o.Brand),

                _ => query.OrderBy(o => o.Brand)


            };


            return Ok(result);
            var totalCount = await query.CountAsync();
            //Pagination
            var items = await query
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .ToListAsync();

            var result = _maper.Map<List<ProductReadDTO>>(items);

            return Ok(new
            {
                totalCount,
                q.Page,
                q.PageSize,
                Data = result
            });
        }



        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductReadDTO>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _repo.GetProductById(id, ct);
            var item = await _db.Products.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductReadDTO>(item));

            return Ok(_maper.Map<ProductReadDTO>(item));
        }






        [HttpPost("create-product")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductReadDTO>> CreateProduct(
            [FromForm] ProductCreateDTO product,
            CancellationToken ct)
        public async Task<ActionResult<ProductCreateDTO>> CreateProduct([FromBody] ProductCreateDTO product, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = _mapper.Map<Product>(product);
            var entity = _maper.Map<Product>(product);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.Characteristics ??= new Characteristics();

            if (product.Image != null && product.Image.Length > 0)
            {
                var imagePath = await _fileStorage.SaveProductImageAsync(product.Image, ct);
                entity.ImagePath = imagePath;
            }

            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);

            var result = _mapper.Map<ProductReadDTO>(entity);
            _db.Products.Add(entity);
            await _db.SaveChangesAsync(ct);

            var result = _maper.Map<ProductReadDTO>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] ProductUpdateDTO product,
            CancellationToken ct)
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDTO product, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);

            var entity = await _repo.GetProductById(id, ct);
            }

            if (entity == null)
                return NotFound();
            var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) { return NotFound(); }

            entity.Characteristics ??= new Characteristics();

            _mapper.Map(product, entity);

            await _repo.SaveChangesAsync(ct);
            _maper.Map(product, entity);

            await _db.SaveChangesAsync();
            return Ok();
        }

 
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var entity = await _repo.GetProductById(id, ct);

            if (entity == null)
                return NotFound();

            if (!string.IsNullOrEmpty(entity.ImagePath))
            {
                await _fileStorage.DeleteAsync(entity.ImagePath, ct);
        }
        // створити Delete
        //Створити модель Користувача(id, Date of birth, phone number, email, name, lastname)
        // реалізувати CRUD та Automaper(DTO)


            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}

