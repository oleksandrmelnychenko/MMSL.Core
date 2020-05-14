using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Common.Exceptions.UserExceptions {
    public class NotFoundValueException : Exception, IUserException {

        public const string VALUE_NOT_FOUND = "value not found";

        public string GetUserMessageException { get; private set; }
        public object Body { get; private set; }

        public void SetBody(object body) {
            Body = body;
        }

        public void SetUserMessage(string message) {
            GetUserMessageException = message;
        }
    }
}
