using System;

namespace MMSL.Common.Exceptions {
    public abstract class MMSLExceptionBase : Exception, IException {
        public string GetUserMessageException { get; private set; }

        public object Body { get; private set; }

        public void SetUserMessage(string message) {
            GetUserMessageException = message;
        }

        public void SetBody(object body) {
            Body = body;
        }
    }
}
