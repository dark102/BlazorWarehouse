using BlazorApp1.Infrastructure;
using BlazorApp1.ViewsModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlazorApp1.Service
{
    public class CashService
    {
        private ConcurrentDictionary<WarehouseViewModel, ConcurrentQueue<ProductViewModel>> _cache = new ConcurrentDictionary<WarehouseViewModel, ConcurrentQueue<ProductViewModel>>();
        private readonly ILogger<CashService> _loger;
        private int _counter = 0;

        public CashService(ILogger<CashService> loger)
        {
            _loger = loger;
        }
        public int GetCounter()
        {
            return _counter;
        }
        public ConcurrentDictionary<WarehouseViewModel, ConcurrentQueue<ProductViewModel>> GetCash()
        {
            return _cache;
        }
        public void InitCash(ApplicationDbContext dbContext)
        {
            try
            {
                if (_cache.Count == 0)
                {
                    var warehouseList = dbContext.Warehouses.Include(x => x.productsList).ToList();
                    foreach (var warehouse in warehouseList)
                    {
                        var warehouseViewModel = new WarehouseViewModel
                        {
                            WarehouseId = warehouse.id,
                            WarehouseName = warehouse.name
                        };
                        foreach (var product in warehouse.productsList)
                        {
                            var productViewModel = new ProductViewModel
                            {
                                ProdyctCount = product.count,
                                ProdyctId = product.id,
                                ProdyctName = product.name,
                            };
                            if (_cache.ContainsKey(warehouseViewModel))
                            {
                                UppdateItemInCash(warehouseViewModel, productViewModel);
                            }
                            else
                            {
                                CreateItemInCash(warehouseViewModel, productViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message);
            }
        }
        public void CreateItemInCash(WarehouseViewModel warehouseItem, ProductViewModel productItem)
        {
            try
            {
                ConcurrentQueue<ProductViewModel> val = new ConcurrentQueue<ProductViewModel>(new List<ProductViewModel>() { productItem });
                if (!_cache.TryAdd(warehouseItem, val))
                    throw new Exception("Ошибка при добавлении сущности в кешь");
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseItem, productItem });
            }
        }
        public void UppdateItemInCash(WarehouseViewModel warehouseItem, ProductViewModel productItem)
        {
            try
            {
                ConcurrentQueue<ProductViewModel> val = new ConcurrentQueue<ProductViewModel>();
                if (_cache.TryGetValue(warehouseItem, out val))
                {
                    val.Enqueue(productItem);
                };
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseItem, productItem });
            }
        }
        public void IncrementCount(WarehouseViewModel warehouseItem, ProductViewModel productItem)
        {
            try
            {
                ConcurrentQueue<ProductViewModel> val = new ConcurrentQueue<ProductViewModel>();
                if (_cache.TryGetValue(warehouseItem, out val))
                {
                    var prod = val.FirstOrDefault(x => x.ProdyctId == productItem.ProdyctId);
                    if (prod != null)
                    {
                        prod.ProdyctCount++;
                    }
                }
                Interlocked.Increment(ref _counter);
                GetCash();

            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseItem, productItem });
            }
        }
        public void DecrementCount(WarehouseViewModel warehouseItem, ProductViewModel productItem)
        {
            try
            {
                ConcurrentQueue<ProductViewModel> val = new ConcurrentQueue<ProductViewModel>();
                if (_cache.TryGetValue(warehouseItem, out val))
                {

                    var prod = val.FirstOrDefault(x => x.ProdyctId == productItem.ProdyctId);
                    if (prod != null)
                    {
                        if (prod.ProdyctCount != 0)
                            prod.ProdyctCount--;
                    }
                }
                Interlocked.Increment(ref _counter);
                GetCash();

            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseItem, productItem });
            }

        }
        public void Clear() {
            _cache = new ConcurrentDictionary<WarehouseViewModel, ConcurrentQueue<ProductViewModel>>();
        }
    }
}
