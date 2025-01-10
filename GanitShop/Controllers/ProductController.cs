using GanitShop.Infrastructure;
using GanitShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GanitShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        IProductManager _productManager;
        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateProductDto createProductDto)
        {
            var dto = await _productManager.CreateAsync(createProductDto);

            if (dto == null)
                return StatusCode(500);

            return Ok(dto);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<ActionResult> GetById(int id)
        {
            var dto = await _productManager.GetByIdAsync(id);
             
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult> GetAll(int offset, string? name, string? code)
        {
            var dtos = await _productManager.GetAllAsync(offset, name, code);

            return Ok(dtos);
        }
         
        [HttpPatch]
        public async Task<ActionResult> Update(int id, UpdateProductDto updateProductDto)
        {
            if(id == 0)
                return BadRequest();

            if (String.IsNullOrEmpty(updateProductDto.Name)
                & String.IsNullOrEmpty(updateProductDto.Code) 
                & String.IsNullOrEmpty(updateProductDto.Description)
                  &  updateProductDto.ProductImage ==null)
                return BadRequest();

            var dto = await _productManager.GetByIdAsync(id);

            if (dto == null)
                return NotFound();

            await _productManager.UpdateAsync(id, updateProductDto);

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _productManager.GetByIdAsync(id);

            if (dto == null)
                return NotFound();

            await _productManager.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
