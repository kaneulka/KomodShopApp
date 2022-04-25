using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.UserSer
{
    public interface IUserService
    {
        User GetUser(string id);
        void UpdateUser(User User);
        void DeleteUser(string id);
    }
}
