using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.ImageRepo
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Image> images;
        string errorMessage = string.Empty;

        public ImageRepository(ApplicationContext context)
        {
            this.context = context;
            images = context.Set<Image>();
        }
        public IEnumerable<Image> GetAll()
        {
            return images.Include(i => i.Product).AsEnumerable();
        }

        public Image Get(long id)
        {
            return images.Include(i => i.Product).SingleOrDefault(s => s.Id == id);
        }

        public Image GetByProductId(long id)
        {
            return images.Include(i => i.Product).SingleOrDefault(s => s.ProductId == id && s.MainImg == true);
        }
        public void Insert(Image entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            images.Add(entity);
            context.SaveChanges();
        }

        public void Update(Image entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Image entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            images.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Image entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            images.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
