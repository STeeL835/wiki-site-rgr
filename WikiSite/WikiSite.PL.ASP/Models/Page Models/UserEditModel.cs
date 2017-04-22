using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace WikiSite.PL.ASP.Models
{
	//[MetadataType(typeof(UserEditModelMeta))] // needs partial according to MSDN
	public class UserEditModel : SignupModel
	{
		#region VM

		/* Nickname */

		/* About */

		/* Role */


		[Display(Name = "Изменить пароль")]
		public bool ChangePassword { get; set; }

		[RequiredIf("ChangePassword", true)]
		[DataType(DataType.Password)]
		[Display(Name = "Текущий пароль")]
		[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$")]
		public string OldPassword { get; set; }

		[RequiredIf("ChangePassword", true)] // new
		[DataType(DataType.Password)]
		[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$")]
		[Display(Name = "Новый пароль")] // new 
		public override string Password { get; set; }

		[RequiredIf("ChangePassword", true)] // new
		[DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("Password")]
		[Display(Name = "Новый пароль еще раз")] // new 
		public override string ConfirmPassword { get; set; }

		#endregion

		public UserEditModel() : base()
		{
			
		}

		public UserEditModel(UserVM user)
		{
			About = user.About;
			Nickname = user.Nickname;
			RoleId = user.RoleId;
		}

		/// <summary>
		/// Returns new and updated VM.
		/// Needs the original because id part gets lost from form,
		/// because it's never sent there
		/// </summary>
		/// <param name="user">current VM</param>
		/// <returns>New and updated VM.</returns>		
		public UserVM GetUserVM(UserVM user)
		{
			return new UserVM(user.Id, user.CredentialsId, Nickname, RoleId)
			{
				About = About
			};
		}

		/// <summary>
		/// Returns new and updated VM.
		/// Needs the original because some values get lost from form,
		/// because they're never sent there
		/// </summary>
		/// <param name="credentials">current VM</param>
		/// <returns>New and updated VM.</returns>
		public UserCredentialsVM GetCredentialsVM(UserCredentialsVM credentials)
		{
			return new UserCredentialsVM(credentials.Id, credentials.Login, UserCredentialsVM.ComputeHashForPassword(Password));
		}


		#region Metadata

		//static UserEditModel()
		//{
		//	TypeDescriptor.AddProviderTransparent(
		//		new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UserEditModel), typeof(UserEditModelMeta)),
		//		typeof(UserEditModel)); // w/o this meta won't work
		//}

		///* About changing dataattribute attributes
		// * https://msdn.microsoft.com/en-us/library/ff664465%28v=pandp.50%29.aspx
		// */

		//public class UserEditModelMeta
		//{
		//	[RequiredWhen("ChangePassword", true)] // new
		//	[DataType(DataType.Password)]
		//	[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$")]
		//	[Display(Name = "Новый пароль")] // new 
		//	public object Password { get; set; }

		//	[RequiredWhen("ChangePassword", true)] // new
		//	[DataType(DataType.Password)]
		//	[System.ComponentModel.DataAnnotations.Compare("Password")]
		//	[Display(Name = "Новый пароль еще раз")] // new 
		//	public object ConfirmPassword { get; set; }
		//}

		#endregion
	}

	
}