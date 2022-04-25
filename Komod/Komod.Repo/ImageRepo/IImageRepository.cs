using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.ImageRepo
{
    public interface IImageRepository : IRepository<Image>
    {
        Image GetByProductId(long id);
    }
}
