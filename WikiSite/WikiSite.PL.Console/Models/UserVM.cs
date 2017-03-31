using System;
using WikiSite.Entities;

namespace WikiSite.PL.Console.Models
{
	public class UserVM
	{
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
		public int SmallId { get; private set; }
		public Guid CredentialsId
		{
			get { return _credentialsId; }
			set
			{
				if (value == Guid.Empty) throw new ArgumentException("Empty id for credentials");
				_credentialsId = value;
			}
		}
		public string Nickname
		{
			get { return _nickname; }
			set
			{ 
				if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Nickname mustn't be empty");
				_nickname = value;
			}
		}
		public string About { get; set; }
		public Guid RoleId
		{
			get { return _roleId; }
			set
			{
				if (value == Guid.Empty) throw new ArgumentException("Empty id for role");
				_roleId = value;
			}
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
			=> new UserDTO { Id = vm.Id, RoleId = vm.RoleId, CredentialsId = vm.CredentialsId, Nickname = vm._nickname, About = vm.About, SmallId = vm.SmallId};

		public static explicit operator UserVM(UserDTO dto) => 
			new UserVM(dto.Id, dto.CredentialsId, dto.Nickname, dto.RoleId)
			{
				About = dto.About,
				SmallId = dto.SmallId
			};
	}
}