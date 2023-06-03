using CrudOperations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext _dbContext;

        public BrandController(BrandContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAll()
        {

            if (_dbContext.Brands == null)
            {
                return NotFound();

            }
            return await _dbContext.Brands.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetById(int id)
        {

            if (_dbContext.Brands == null)
            {
                return NotFound();

            }

            var brand = await _dbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> Save(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, Brand brand)
        {
            if (id != brand.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(brand).State = EntityState.Modified;

            try
            {

                await _dbContext.SaveChangesAsync();


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();

        }

        private bool BrandAvailable(int id)
        {
            return (_dbContext.Brands?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {

            if (_dbContext.Brands == null)
            {
                return NotFound();
            }

            var brand = await _dbContext.Brands.FindAsync(id);

            if (brand == null)
            {

                return NotFound();
            }

            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();
            return Ok();

        }



    }
}
