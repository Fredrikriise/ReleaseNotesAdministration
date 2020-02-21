namespace Services.Repository.Models.DatabaseModels
{
    public class Product
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }

        public void AddProductId(int? productId)
        {
            ProductId = productId;
        }
    }
}
