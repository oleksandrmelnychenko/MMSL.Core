using System;

namespace MMSL.Common.Exceptions {
    public class ExceptionCreator<TException> where TException : IException, new() {

        private IException _exception;

        public string GetUserMessage => _exception.GetUserMessageException;

        public static ExceptionCreator<TException> Create(string errorMessage, object body = null) {
            var instance = new ExceptionCreator<TException>();

            TException exception = new TException();
            exception.SetUserMessage(errorMessage);
            exception.SetBody(body);

            instance._exception = exception;
            return instance;
        }

        public void Throw() {
            throw (Exception)_exception;
        }

        protected ExceptionCreator() { }
    }
}
