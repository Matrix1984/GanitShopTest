
using Microsoft.Extensions.Hosting.Internal;
using System.Reflection; 

namespace GanitShop.Infrastructure
{
    public class ProductFileUploadManager : IProductFileUploadManager
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        public ProductFileUploadManager(IWebHostEnvironment environment)
        {
            hostingEnvironment = environment;
        } 

        public async Task UploadFileAsync(IFormFile productImage, string uniqueFileName)
        { 
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
            var filePath = Path.Combine(uploads, uniqueFileName);
            await productImage.CopyToAsync(new FileStream(filePath, FileMode.Create),CancellationToken.None); 

        }

        public void DeleteFile(string filePath)
        {
            // Deletion of a file can only be implemented after the right system permissions.
            // Need also to account for concurrent operations on the image.
            //if (!filePath.Contains("image-not-found"))
            //{
            //    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");

            //    var file = Path.Combine(uploads, filePath);

            //    System.IO.File.Delete(file);
            //}

        }
    }
}
