using System.Web;
using System.Web.Mvc;

namespace WikiSite.PL.ASP.Classes
{
	public class AllowAnonymousOnlyAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			// make sure the user is not authenticated. If it's not, return true. Otherwise, return false
			return !httpContext.User.Identity.IsAuthenticated;
		}
	}
}