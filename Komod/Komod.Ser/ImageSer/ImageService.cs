using Komod.Data;
using Komod.Repo.ImageRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ImageSer
{
    public class ImageService : IImageService
    {
        private IImageRepository imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        public IEnumerable<Image> GetImages()
        {
            return imageRepository.GetAll();
        }

        public Image GetImage(long id)
        {
            return imageRepository.Get(id);
        }

        public Image GetMainImageByProductId(long id)
        {
            return imageRepository.Get(id);
        }

        public void InsertImage(Image image)
        {
            imageRepository.Insert(image);
        }
        public void UpdateImage(Image image)
        {
            imageRepository.Update(image);
        }

        public void DeleteImage(long id)
        {
            Image image = GetImage(id);
            imageRepository.Remove(image);
            imageRepository.SaveChanges();
        }
    }
}
