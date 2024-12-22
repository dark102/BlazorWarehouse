using BlazorApp1.Infrastructure;
using BlazorApp1.ViewsModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlazorApp1.Service
{
    public class CashService
    {
        private ConcurrentDictionary<ProductWarehouseIds, int> _dinmickCache = new ConcurrentDictionary<ProductWarehouseIds, int>();
        private Dictionary<WarehouseViewModel, List<ProductViewModel>> _statickCache = new Dictionary<WarehouseViewModel, List<ProductViewModel>>();

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
        public ConcurrentDictionary<ProductWarehouseIds, int> GetDinamickCash()
        {
            return _dinmickCache;
        }
        public Dictionary<WarehouseViewModel, List<ProductViewModel>> GetStatickCash()
        {
            return _statickCache;
        }
        public int GetCountByIds(ProductWarehouseIds productWarehouseIds)
        {
            var itemKey = _dinmickCache.FirstOrDefault(x => x.Key.ProductId == productWarehouseIds.ProductId && x.Key.WarehouseId == productWarehouseIds.WarehouseId).Key;
            _dinmickCache.TryGetValue(itemKey, out int count);
            return count;
        }
        public void InitDinamickCash(ApplicationDbContext dbContext)
        {
            try
            {
                if (_dinmickCache.Count == 0)
                {
                    var productsWarehousesList = dbContext.ProductsWarehouses.ToList();
                    foreach (var productsWarehouse in productsWarehousesList)
                    {
                        if (!_dinmickCache.TryAdd(new ProductWarehouseIds { WarehouseId = productsWarehouse.WarehouseId, ProductId = productsWarehouse.ProductId }, productsWarehouse.ProductCount))
                            throw new Exception("Ошибка при добавлении сущности в кешь");
                    }
                }
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message);
            }
        }
        public void InitStatickCash(ApplicationDbContext dbContext)
        {
            try
            {
                if (_statickCache.Count == 0)
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
                                ProdyctId = product.id,
                                ProdyctName = product.name,
                            };
                            if (_statickCache.ContainsKey(warehouseViewModel))
                            {
                                _statickCache[warehouseViewModel].Add(productViewModel);
                            }
                            else
                            {
                                _statickCache.Add(warehouseViewModel, new List<ProductViewModel> { productViewModel });
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
        public void IncrementCount(int warehouseId, int productId)
        {
            try
            {
                var itemKey = _dinmickCache.FirstOrDefault(x => x.Key.ProductId == productId && x.Key.WarehouseId == warehouseId).Key;
                _dinmickCache.AddOrUpdate(itemKey, 0, (key, value) => Interlocked.Increment(ref value));
                Interlocked.Increment(ref _counter);

            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseId, productId });
            }
        }
        public void DecrementCount(int warehouseId, int productId)
        {
            try
            {
                var itemKey = _dinmickCache.FirstOrDefault(x => x.Key.ProductId == productId && x.Key.WarehouseId == warehouseId).Key;
                _dinmickCache.AddOrUpdate(itemKey, 0, (key, value) => Interlocked.Decrement(ref value));
                Interlocked.Increment(ref _counter);
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseId, productId });
            }
        }
        public void Clear()
        {
            _dinmickCache = new ConcurrentDictionary<ProductWarehouseIds, int>();
            _statickCache = new Dictionary<WarehouseViewModel, List<ProductViewModel>>();
        }
    }
}
