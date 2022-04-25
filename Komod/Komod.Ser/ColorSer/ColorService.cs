using Komod.Data;
using Komod.Repo.ColorRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ColorSer
{
    public class ColorService : IColorService
    {
        private IColorRepository colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            this.colorRepository = colorRepository;
        }

        public IEnumerable<Color> GetColors()
        {
            return colorRepository.GetAll();
        }

        public Color GetColor(long id)
        {
            return colorRepository.Get(id);
        }

        public void InsertColor(Color color)
        {
            colorRepository.Insert(color);
        }
        public void UpdateColor(Color color)
        {
            colorRepository.Update(color);
        }

        public void DeleteColor(long id)
        {
            Color color = GetColor(id);
            colorRepository.Remove(color);
            colorRepository.SaveChanges();
        }
        public bool IsColorExist(Color entity)
        {
            return colorRepository.IsColorExist(entity);
        }
    }
}
