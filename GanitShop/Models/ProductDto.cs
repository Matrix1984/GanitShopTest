namespace GanitShop.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime CreationTime { get; set; }

        public string ImagePath { get; set; }
    }
}
