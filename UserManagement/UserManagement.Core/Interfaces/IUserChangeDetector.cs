using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.Request;
using UserManagement.Core.Entities;

namespace UserManagement.Core.Interfaces
{
    public interface IUserChangeDetector
    {
        bool HasChanges(UpdateUserRequest updateUserRequest, User user);
    }

}
