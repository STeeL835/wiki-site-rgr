using WikiSite.BLL.Abstract;

namespace WikiSite.PL.Console
{
	internal static class BLOs
	{
		public static IUserCredentialsBLL CredentialsBLO { get; set; }
		public static IUsersBLL UsersBLO { get; set; }
	}
}