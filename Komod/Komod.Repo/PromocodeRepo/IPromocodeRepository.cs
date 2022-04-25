using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.PromocodeRepo
{
    public interface IPromocodeRepository : IRepository<Promocode>
    {
        void UpdateArray(List<Promocode> entity);//BdController
    }
}
