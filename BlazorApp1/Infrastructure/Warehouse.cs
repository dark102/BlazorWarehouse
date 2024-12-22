namespace BlazorApp1.Infrastructure
{
    public class Warehouse
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Product> productsList { get; set; } = new();
    }
}