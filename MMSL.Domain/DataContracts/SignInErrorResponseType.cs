namespace MMSL.Domain.DataContracts
{
    public enum SignInErrorResponseType
    {
        InvalidEmail,
        InvalidCredentials,
        PasswordExpired,
        NotAllowed,
        InvalidToken,
        TokenExpired,
        UserDeleted
    }
}
