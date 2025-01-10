using GanitShop.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

//TODO: implement CRUD.
//RESEARCH: mdf output db. best way to connect.
namespace GanitShop.Infrastructure
{ 
    public class ProductManager : IProductManager
    {
        IProductDbManager _productDbManager;

        IProductFileUploadManager _productFileUploadManager;

        public ProductManager(IProductDbManager productDbManager, IProductFileUploadManager productFileUploadManager) {

            _productDbManager = productDbManager;

            _productFileUploadManager = productFileUploadManager;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        { 
           var productDto= await _productDbManager.CreateAsync(createProductDto);

            if (productDto.Id > 0 & createProductDto.ProductImage !=null)
                await _productFileUploadManager.UploadFileAsync(createProductDto.ProductImage, createProductDto.UniqueImageFileName);

            return productDto;   
        }

        public async Task<List<ProductDto>> GetAllAsync(int offset, string nameName, string codeName, int count=50)
        {
            return await _productDbManager.GetAllAsync( offset,  nameName,  codeName,  count);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
          => await _productDbManager.GetByIdAsync(id);

        public async Task UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var productDto = await _productDbManager.GetByIdAsync(id);

            if (productDto.Id == 0)
                throw new Exception("Product not implemented. ");

            await _productDbManager.UpdateAsync(id, updateProductDto);

            if (updateProductDto.ProductImage != null)
            {
                await _productFileUploadManager.UploadFileAsync(updateProductDto.ProductImage, updateProductDto.UniqueImageFileName);
                 
                if (productDto.ImagePath != null)
                    _productFileUploadManager.DeleteFile(productDto.ImagePath);
            } 

        }

        public async Task DeleteByIdAsync(int id)
        {
            var productDto = await _productDbManager.GetByIdAsync(id);

            await _productDbManager.DeleteByIdAsync(id);

            _productFileUploadManager.DeleteFile(productDto.ImagePath);
        } 
    }
}
