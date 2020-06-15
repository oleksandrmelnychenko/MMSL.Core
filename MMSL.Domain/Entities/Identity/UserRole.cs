namespace MMSL.Domain.Entities.Identity {
    public class UserRole : EntityBase {

        public long UserRoleTypeId { get; set; }

        public UserIdentityRoleType UserRoleType { get; set; }

        public long UserIdentityId { get; set; }

        public UserIdentity UserIdentity { get; set; }
    }
}
