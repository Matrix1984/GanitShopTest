using GanitShop.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace GanitShop.Models
{
    public class BaseCreateUpdateProductDto
    {
   

        public string? Description { get; set; }

        public IFormFile? ProductImage { set; get; }

        [NotMapped]
        public string UniqueImageFileName
        {
            get
            {

                if (_uniqueFileName == null)
                    if (ProductImage != null && ProductImage.FileName != null)
                        _uniqueFileName = FileNameGenerator.GetUniqueFileName(ProductImage.FileName);
                    else
                        _uniqueFileName = String.Empty;

                return _uniqueFileName;
            }
        }

        string _uniqueFileName;
    }
}
