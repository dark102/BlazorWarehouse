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
                _cashService.InitDinamickCash(_dbContext);
                _cashService.InitStatickCash(_dbContext);
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
        public int GetCountByIds(int warehouseId, int productId)
        {
            try
            {
                return _cashService.GetCountByIds(new ProductWarehouseIds() { ProductId = productId, WarehouseId = warehouseId });
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseId, productId });
                SaveCash();
                return 0;
            }
        }
        public void IncrementCount(int warehouseId, int productId)
        {
            try
            {
                _cashService.IncrementCount(warehouseId, productId);
                if(GetCounter() % 1000 == 0)
                {
                    SaveCash();
                }
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseId, productId });
                SaveCash();
            }
        }
        public void DecrementCount(int warehouseId, int productId)
        {
            try
            {
                _cashService.DecrementCount(warehouseId, productId);
                if (GetCounter() % 1000 == 0)
                {
                    SaveCash();
                }

            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseId, productId });
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
        public ConcurrentDictionary<ProductWarehouseIds, int> GetDinamickCash()
        {
            try
            {
                return _cashService.GetDinamickCash();
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message);
                SaveCash();
                return null;

            }
        }
        public Dictionary<WarehouseViewModel, List<ProductViewModel>> GetStatickCash()
        {
            try
            {
                return _cashService.GetStatickCash();
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
                    foreach (var item in GetDinamickCash())
                    {
                        var warehouse = _dbContext.ProductsWarehouses.FirstOrDefault(x => x.WarehouseId == item.Key.WarehouseId && x.ProductId == item.Key.ProductId );
                        warehouse.ProductCount = item.Value;
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
