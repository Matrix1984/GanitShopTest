using GanitShop.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace GanitShop.Models
{
    public class CreateProductDto : BaseCreateUpdateProductDto
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }
}
