using System;
using System.Collections.Concurrent;
using BlazorApp1.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace BlazorApp1.Service
{
    public class WarehouseService
    {
        //public ConcurrentDictionary<int, Warehouse>? warehouseDictionary;
        public List<Warehouse>? warehouseList;
        private readonly ILogger<WarehouseService> _loger;
        public WarehouseService(ILogger<WarehouseService> loger)
        {
            //dbContext = Utils.GetDbContext();
            _loger = loger;
            GetWarehouseList();
        }

        public List<Warehouse>? GetWarehouseList()
        {
            try
            {
                //if (warehouseList == null)
                //{
                //    warehouseList = dbContext.Warehouses.Include(x => x.productsList).ToList();
                //}
                //return warehouseList.Count() > 0 ? warehouseList : null;
                return warehouseList;
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { });
                return null;
            }

        }
        public void IncrementCount(int warehouseId, int productId)
        {
            try
            {
                throw new Exception("А вот!!!!!!!!!!!!!!!!!!");
                //var prod = warehouseList.FirstOrDefault(x => x.id == warehouseId).productsList.FirstOrDefault(x => x.id == productId);
                //prod.count++;
                //dbContext.Products.Entry(prod).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseId, productId });
                //dbContext.SaveChanges();
            }
        }
        public void DecrementCount(int warehouseId, int productId)
        {
            try
            {
                    //var prod = warehouseList.FirstOrDefault(x => x.id == warehouseId).productsList.FirstOrDefault(x => x.id == productId);
                    //prod.count--;
                    //dbContext.Products.Entry(prod).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _loger.LogError(ex, ex.Message, new object[] { warehouseId, productId });
                //dbContext.SaveChanges();
            }

        }
        public void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
        {

            //lock (obj)
            //{
            //    Console.WriteLine(i);
            //    i++;
            //}
            try
            {
                if (!e.Location.Contains("/warehouseAndProductManagement"))
                {
                    Console.WriteLine("Сохраняем");
                    //dbContext.SaveChanges();
                }
            }
            catch(Exception ex) {
                _loger.LogError(ex, ex.Message, new object[] { sender, e });
            }
            
        }
    }
}
