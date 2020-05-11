using System;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Domain.Repositories.Identity.Contracts {
    public interface IIdentityRepository {

        int UpdateUserPassword(UserIdentity user);

        bool IsEmailAvailable(string email);

        void UpdateUserLastLoggedInDate(long userId, DateTime current);

        void UpdateUserExperationDate(long userId, bool isExpired);

        UserIdentity GetUserByEmail(string email);

        UserIdentity GetUserById(long userId);

        UserIdentity NewUser(string name, string description, string email, string passwordHash, string passwordSalt);

        long NewUser(UserIdentity userIdentity);

        UserAccount GetAccountByUserId(long userId);
    }
}
