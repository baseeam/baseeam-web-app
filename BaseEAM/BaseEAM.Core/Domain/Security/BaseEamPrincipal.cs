using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Security;

namespace BaseEAM.Core.Domain.Security
{
    public class BaseEamIdentity : ClaimsIdentity
    {
        [SecuritySafeCritical]
        public BaseEamIdentity(long userId, string name, string type)
            : base(new GenericIdentity(name, type))
        {
            UserId = userId;
        }

        public long UserId { get; private set; }

        public override bool IsAuthenticated { get { return UserId != 0; } }
    }


    public class BaseEamPrincipal : IPrincipal
    {
        public BaseEamPrincipal(User user, string type)
        {
            this.Identity = new BaseEamIdentity(user.Id, user.LoginName, type);
        }

        public bool IsInRole(string role)
        {
            return (Identity != null && Identity.IsAuthenticated && !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role));
        }

        public IIdentity Identity { get; private set; }
    }
}
