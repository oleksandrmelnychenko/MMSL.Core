namespace MMSL.Common.Exceptions {
    public interface IException {
        string GetUserMessageException { get; }

        object Body { get; }

        void SetUserMessage(string message);

        void SetBody(object body);
    }
}
