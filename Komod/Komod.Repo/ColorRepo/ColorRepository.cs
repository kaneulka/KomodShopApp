using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Komod.Data;
using System.Linq;

namespace Komod.Repo.ColorRepo
{
    public class ColorRepository : IColorRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Color> entities;
        string errorMessage = string.Empty;

        public ColorRepository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<Color>();
        }
        public IEnumerable<Color> GetAll()
        {
            return entities.AsEnumerable();
        }

        public Color Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Color entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Color entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Color entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Color entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public bool IsColorExist(Color entity)
        {
            if (entities.ToList().Exists(c => c.Name == entity.Name && c.ColorCode == entity.ColorCode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}