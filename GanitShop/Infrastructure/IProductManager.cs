using GanitShop.Models;

namespace GanitShop.Infrastructure
{
    public interface IProductManager
    {
        Task<ProductDto> CreateAsync(CreateProductDto createProductDto);

        Task UpdateAsync(int id, UpdateProductDto updateProductDto);

        Task DeleteByIdAsync(int id);

        Task<ProductDto> GetByIdAsync(int id);

        Task<QueryResults> GetAllAsync(int offset, string nameName, string codeName, string? orderBy, bool asc, int count = 10);
    }
}
