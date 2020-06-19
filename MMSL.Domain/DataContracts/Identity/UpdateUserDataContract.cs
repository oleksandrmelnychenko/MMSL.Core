using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts.Identity {
    public sealed class UpdateUserDataContract {

        public long Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime PasswordExpiresAt { get; set; }

        public bool ForceChangePassword { get; set; }
    }
}
