using Komod.Data;
using Komod.Repo.UserRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.UserSer
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User GetUser(string id)
        {
            return userRepository.GetUser(id);
        }

        public void UpdateUser(User user)
        {
            userRepository.Update(user);
        }

        public void DeleteUser(string id)
        {
            User user = GetUser(id);
            userRepository.Remove(user);
            userRepository.SaveChanges();
        }
    }
}
