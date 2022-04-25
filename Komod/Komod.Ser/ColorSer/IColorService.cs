using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ColorSer
{
    public interface IColorService
    {
        IEnumerable<Color> GetColors();
        Color GetColor(long id);
        void InsertColor(Color Color);
        void UpdateColor(Color Color);
        void DeleteColor(long id);
        bool IsColorExist(Color entity);
    }
}
