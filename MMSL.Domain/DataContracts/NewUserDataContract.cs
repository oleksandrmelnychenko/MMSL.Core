using System;

namespace MMSL.Domain.DataContracts
{
    public sealed class NewUserDataContract
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime PasswordExpiresAt { get; set; }
    }
}
