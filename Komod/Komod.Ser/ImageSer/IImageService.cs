using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ImageSer
{
    public interface IImageService
    {
        IEnumerable<Image> GetImages();
        Image GetImage(long id);
        Image GetMainImageByProductId(long id);
        void InsertImage(Image image);
        void UpdateImage(Image image);
        void DeleteImage(long id);
    }
}
