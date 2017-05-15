using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WikiSite.BLL.Abstract;
using WikiSite.DI.Provider;
using WikiSite.Entities;

namespace WikiSite.PL.ASP.Models
{
	public class UserVM
	{
		#region Instance

		private Guid _id;
		private Guid _credentialsId;
		private string _nickname;
		private Guid _roleId;

		public Guid Id
		{
			get { return _id; }
			set
			{
				if (value == Guid.Empty) throw new ArgumentException("Empty id");
				_id = value;
			}
		}

		public int ShortId { get; private set; }

		public Guid CredentialsId
		{
			get { return _credentialsId; }
			set
			{
				if (value == Guid.Empty) throw new ArgumentException("Empty id for credentials");
				_credentialsId = value;
			}
		}

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Никнейм")]
		[RegularExpression("^[0-9a-zA-ZА-Яа-яёЁ_ ]{3,50}$", 
			ErrorMessage = "Никнейм может состоять только из латинских и русских букв, цифр, пробела и знака подчеркивания, " +
			               "а также не может быть короче 3 и длиннее 50 символов")]
		public string Nickname
		{
			get { return _nickname; }
			set
			{ 
				if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Nickname mustn't be empty");
				_nickname = value;
			}
		}

		[DataType(DataType.MultilineText)]
		[MaxLength(1500, ErrorMessage = "Максимальное количество символов - 1500")]
		[Display(Name = "О себе")]
		public string About { get; set; }

		[Required][Display(Name = "Роль")]
		public Guid RoleId
		{
			get { return _roleId; }
			set
			{
				if (value == Guid.Empty) throw new ArgumentException("Empty id for role");
				_roleId = value;
			}
		}


		/// <summary>
		/// ONLY for model binder
		/// </summary>
		public UserVM()
		{
			CredentialsId = Guid.NewGuid();
			Id = Guid.NewGuid();
		}
		public UserVM(Guid credentialsId, string nickname, Guid roleId)
		{
			Id = Guid.Empty;
			CredentialsId = credentialsId;
			Nickname = nickname;
			RoleId = roleId;
		}
		public UserVM(Guid id, Guid credentialsId, string nickname, Guid roleId)
		{
			Id = id;
			CredentialsId = credentialsId;
			Nickname = nickname;
			RoleId = roleId;
		}


		public static implicit operator UserDTO(UserVM vm)
			=> new UserDTO { Id = vm.Id, RoleId = vm.RoleId, CredentialsId = vm.CredentialsId, Nickname = vm._nickname, About = vm.About, ShortId = vm.ShortId};

		public static explicit operator UserVM(UserDTO dto) => 
			new UserVM(dto.Id, dto.CredentialsId, dto.Nickname, dto.RoleId)
			{
				About = dto.About,
				ShortId = dto.ShortId
			};


		public bool IsValid()
		{
			return Id != Guid.Empty &&
			       CredentialsId != Guid.Empty &&
			       RoleId != Guid.Empty &&
			       !string.IsNullOrWhiteSpace(Nickname);
		}

		#endregion

		#region Static

		private static IUsersBLL _bll;

		static UserVM()
		{
			_bll = Provider.UsersBLO;
		}

		/// <summary>
		/// Returns all users
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<UserVM> GetAllUsers(Guid role)
		{
			return _bll.GetUsers(role).Select(dto => (UserVM) dto).ToArray();
		}
		
		/// <summary>
		/// Returns all users
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<UserVM> GetAllUsers()
		{
			return _bll.GetUsers().Select(dto => (UserVM) dto).ToArray();
		}

		/// <summary>
		/// Adds user to a database
		/// </summary>
		/// <param name="user">User to add</param>
		/// <param name="credentials">Credentials of user to add</param>
		/// <returns>Whether addition was successful</returns>
		public static bool AddUser(UserVM user, UserCredentialsVM credentials)
		{
			return _bll.AddUser(user, credentials);
		}

		/// <summary>
		/// Returns user by it's short id
		/// </summary>
		/// <param name="shortId"></param>
		/// <returns></returns>
		public static UserVM GetUser(int shortId)
		{
			return (UserVM) _bll.GetUser(shortId);
		}

		/// <summary>
		/// Returns user by it's guid
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static UserVM GetUser(Guid id)
		{
			return (UserVM) _bll.GetUser(id);
		}

		/// <summary>
		/// Returns user by it's credentials, if exist
		/// </summary>
		/// <remarks>
		/// There guid is not necessary, empty guid can be passed.
		/// </remarks>
		/// <param name="credentials"></param>
		/// <returns></returns>
		public static UserVM GetUser(UserCredentialsVM credentials)
		{
			var user = _bll.GetUser(credentials);
			return user == null ? null : (UserVM)user;
		}

		/// <summary>
		/// Removes user from db
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool RemoveUser(Guid id)
		{
			return _bll.RemoveUser(id);
		}

		/// <summary>
		/// Updates info of a user
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool UpdateUser(UserVM user)
		{
			return _bll.UpdateUser(user);
		}

		public static IEnumerable<UserVM> SearchUsers(string searchInput)
		{
			return _bll.SearchUsers(searchInput).Select(dto => (UserVM) dto);
		}

		#endregion
	}
}