using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL.GlobalExceptions.Forbidden_Exception
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public IEnumerable<string> Roles { get; }
        public RoleRequirement(IEnumerable<string> role)
        {
            Roles = role.ToList();
        }
    }
}
