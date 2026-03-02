using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraziiRabotaitePj.DTO;
using MigraziiRabotaitePj.Models;
using MigraziiRabotaitePj.Repository;
using MigraziiRabotaitePj.Services;

namespace MigraziiRabotaitePj.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;

        public ProductController(
            IProductRepository repo,
            IMapper mapper,
            IFileStorage fileStorage)
        {
            _repo = repo;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }


        [HttpGet]
        public async Task<ActionResult<List<ProductReadDTO>>> GetAll(CancellationToken ct)
        {
            var items = await _repo.GetAllProducts(ct);
            var result = _mapper.Map<List<ProductReadDTO>>(items);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductReadDTO>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _repo.GetProductById(id, ct);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<ProductReadDTO>(item));
        }


        [HttpPost("create-product")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductReadDTO>> CreateProduct(
            [FromForm] ProductCreateDTO product,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var entity = _mapper.Map<Product>(product);
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

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] ProductUpdateDTO product,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var entity = await _repo.GetProductById(id, ct);

            if (entity == null)
                return NotFound();

            entity.Characteristics ??= new Characteristics();

            _mapper.Map(product, entity);

            await _repo.SaveChangesAsync(ct);

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

            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}