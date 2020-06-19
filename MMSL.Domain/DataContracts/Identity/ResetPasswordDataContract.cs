using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts.Identity {
    public class ResetPasswordDataContract : AuthenticationDataContract {
        public string NewPassword { get; set; }
    }
}
