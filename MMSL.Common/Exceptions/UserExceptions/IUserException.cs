
namespace MMSL.Common.Exceptions.UserExceptions
{
    public interface IUserException 
    {
        string GetUserMessageException { get; }

        object Body { get; }

        void SetUserMessage(string message);

        void SetBody(object body);
    }
}
