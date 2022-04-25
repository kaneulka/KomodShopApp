using Komod.Data;
using Komod.Repo.StockStatusRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.StockStatusSer
{
    public class StockStatusService : IStockStatusService
    {
        private IStockStatusRepository stockStatusRepository;

        public StockStatusService(IStockStatusRepository stockStatusRepository)
        {
            this.stockStatusRepository = stockStatusRepository;
        }

        public IEnumerable<StockStatus> GetStockStatuses()
        {
            return stockStatusRepository.GetAll();
        }

        public StockStatus GetStockStatus(long id)
        {
            return stockStatusRepository.Get(id);
        }

        public void InsertStockStatus(StockStatus stockStatus)
        {
            stockStatusRepository.Insert(stockStatus);
        }
        public void UpdateStockStatus(StockStatus stockStatus)
        {
            stockStatusRepository.Update(stockStatus);
        }

        public void DeleteStockStatus(long id)
        {
            StockStatus stockStatus = GetStockStatus(id);
            stockStatusRepository.Remove(stockStatus);
            stockStatusRepository.SaveChanges();
        }
    }
}
