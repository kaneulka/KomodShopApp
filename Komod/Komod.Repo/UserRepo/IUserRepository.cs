using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.UserRepo
{
    public interface IUserRepository
    {
        User GetUser(string id);
        void Update(User entity);
        void Delete(User entity);
        void Remove(User entity);
        void SaveChanges();
    }
}
