using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.UserModels
{
    public class RolesViewModel
    {
        public IEnumerable<RoleViewModel> Roles { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
