using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.ColorRepo
{
    public interface IColorRepository : IRepository<Color>
    {
        bool IsColorExist(Color entity);
        public IEnumerable<Color> GetAll();
        public Color Get(long id);
        public void Insert(Color entity);
        public void Update(Color entity);
        public void Delete(Color entity);
    }
}
