using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PromocodeSer
{
    public interface IPromocodeService
    {
        IEnumerable<Promocode> GetPromocodes();
        Promocode GetPromocode(long id);
        void InsertPromocode(Promocode Promocode);
        void UpdatePromocode(Promocode Promocode);
        void DeletePromocode(long id);
    }
}
