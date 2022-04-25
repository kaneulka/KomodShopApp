using Komod.Data;
using Komod.Repo.PromocodeRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PromocodeSer
{
    public class PromocodeService : IPromocodeService
    {
        private IPromocodeRepository promocodeRepository;

        public PromocodeService(IPromocodeRepository promocodeRepository)
        {
            this.promocodeRepository = promocodeRepository;
        }

        public IEnumerable<Promocode> GetPromocodes()
        {
            return promocodeRepository.GetAll();
        }

        public Promocode GetPromocode(long id)
        {
            return promocodeRepository.Get(id);
        }

        public void InsertPromocode(Promocode promocode)
        {
            promocodeRepository.Insert(promocode);
        }
        public void UpdatePromocode(Promocode promocode)
        {
            promocodeRepository.Update(promocode);
        }

        public void DeletePromocode(long id)
        {
            Promocode promocode = GetPromocode(id);
            promocodeRepository.Remove(promocode);
            promocodeRepository.SaveChanges();
        }
    }
}
