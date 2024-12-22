using System.Collections.Generic;
using System.Reflection.Emit;

namespace BlazorApp1.Infrastructure
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Warehouse> warehouseList { get; set; } = new();
        public List<ProductWarehouse> ProductsWarehouses { get; set; } = new();
    }
}
