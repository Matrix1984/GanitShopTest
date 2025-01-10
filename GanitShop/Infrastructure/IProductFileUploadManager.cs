namespace GanitShop.Infrastructure
{
    public interface IProductFileUploadManager
    {
       Task UploadFileAsync(IFormFile productImage, string uniqueFileName);

        void DeleteFile(string filePath);
    }
}
