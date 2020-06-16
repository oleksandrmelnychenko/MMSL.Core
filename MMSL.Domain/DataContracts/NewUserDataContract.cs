using MMSL.Domain.Entities.Identity;
using System;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts
{
    public sealed class NewUserDataContract
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime PasswordExpiresAt { get; set; }

        public List<RoleType> Roles { get; set; } = new List<RoleType>();
    }
}
