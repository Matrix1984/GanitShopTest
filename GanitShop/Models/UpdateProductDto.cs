namespace GanitShop.Models
{
    public class UpdateProductDto : BaseCreateUpdateProductDto
    {
        public string? Name { get; set; }

        public string? Code { get; set; }
    }
}
