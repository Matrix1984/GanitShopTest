using GanitShop.Models;

namespace GanitShop.Infrastructure
{
    public interface IProductDbManager
    {
        Task<ProductDto> CreateAsync(CreateProductDto createProductDto);

        Task UpdateAsync(int id, UpdateProductDto updateProductDto);

        Task DeleteByIdAsync(int id);

        Task<ProductDto> GetByIdAsync(int id);

        Task<List<ProductDto>> GetAllAsync(int offset, string nameName, string codeName, int count = 50);
    }
}
