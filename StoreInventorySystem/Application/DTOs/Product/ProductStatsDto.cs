namespace StoreInventorySystem.Application.DTOs.Product
{
    public class ProductStatsDto
    {
        public int TotalProducts { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
