using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext context;
        private DbSet<User> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<User>();
        }

        public User GetUser(string id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public void Update(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(User entity)
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
    }
}
