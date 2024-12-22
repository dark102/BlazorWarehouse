using System;
using System.Collections.Concurrent;
using BlazorApp1.Infrastructure;
using BlazorApp1.ViewsModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace BlazorApp1.Service
{
    public class WarehouseService
    {
        object obj = new object();
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<WarehouseService> _loger;
        private readonly CashService _cashService;
        public event Action OnChange;

        public WarehouseService(ILogger<WarehouseService> loger, ApplicationDbContext dbContext, CashService cashService)
        {
            _dbContext = dbContext;
            _loger = loger;
            _dbContext = dbContext;
            _cashService = cashService;
        }
        public void InitDictionary()
        {
            try
            {
                _cashService.InitCash(_dbContext);
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { });
                SaveCash();
            }
        }
        public void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            try
            {
                if (!e.Location.Contains("/warehouseAndProductManagement"))
                {
                    Console.WriteLine("Сохраняем");
                    SaveCash();
                }
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { sender, e });
                SaveCash();
            }

        }
        public void IncrementCount(WarehouseViewModel warehouseItem, ProductViewModel productItem)
        {
            try
            {
                _cashService.IncrementCount(warehouseItem, productItem);
                if(GetCounter() % 1000 == 0)
                {
                    SaveCash();
                }
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseItem, productItem });
                SaveCash();
            }
        }
        public void DecrementCount(WarehouseViewModel warehouseItem, ProductViewModel productItem)
        {
            try
            {
                _cashService.DecrementCount(warehouseItem, productItem);
                if (GetCounter() % 1000 == 0)
                {
                    SaveCash();
                }

            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseItem, productItem });
                SaveCash();
            }

        }
        public int GetCounter()
        {
            try
            {
                return _cashService.GetCounter();
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message);
                SaveCash();
                return 0;

            }
        }
        public ConcurrentDictionary<WarehouseViewModel, ConcurrentQueue<ProductViewModel>> GetCash()
        {
            try
            {
                return _cashService.GetCash();
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message);
                SaveCash();
                return null;

            }
        }
        private void SaveCash()
        {
            try
            {
                lock (obj)
                {
                    foreach (var item in GetCash())
                    {
                        var warehouse = _dbContext.Warehouses.Include(x => x.productsList).FirstOrDefault(x => x.id == item.Key.WarehouseId);
                        foreach (var product in warehouse.productsList)
                        {
                            var prod = item.Value.FirstOrDefault(x => x.ProdyctId == product.id);
                            if (prod != null)
                            {
                                product.count = prod.ProdyctCount;
                                _dbContext.Products.Update(product);
                            }
                        }
                    }
                    _dbContext.SaveChanges();
                    _cashService.Clear();
                    InitDictionary();
                }
            }
            catch(Exception ex)
            {
                _loger.LogError(ex, ex.Message);
                SaveInNotepad();
            }
        }
        private void SaveInNotepad()
        {
           //TODO если с базой какая то проблема то сохраняем в фаил. что бы не потерять данные. и при инициализации нужен метод проверки наличия файла и пасинг его с занесением в базу
        }
    }
}
