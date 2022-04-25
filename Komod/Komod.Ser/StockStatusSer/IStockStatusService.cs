using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.StockStatusSer
{
    public interface IStockStatusService
    {
        IEnumerable<StockStatus> GetStockStatuses();
        StockStatus GetStockStatus(long id);
        void InsertStockStatus(StockStatus StockStatus);
        void UpdateStockStatus(StockStatus StockStatus);
        void DeleteStockStatus(long id);
    }
}
