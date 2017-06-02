using System;
using System.Collections.Generic;
using AutoMapper;
using WikiSite.BLL.Abstract;
using WikiSite.DI.Provider;
using WikiSite.Entities;

namespace WikiSite.PL.ASP.Models
{
	public class AVersionVoteVM
	{
		#region Instance

		public Guid Id { get; set; }
		public Guid ArticleVersionId { get; set; }
		public Guid UserId { get; set; }
		public bool IsLike { get; set; }

		/// <summary>
		/// Constructor for model binder
		/// </summary>
		public AVersionVoteVM()
		{
			Id = Guid.NewGuid();
		}

		public AVersionVoteVM(Guid articleVersionId, Guid userId, bool isLike)
			: this()
		{
			ArticleVersionId = articleVersionId;
			UserId = userId;
			IsLike = isLike;
		}

		public AVersionVoteVM(Guid id, Guid articleVersionId, Guid userId, bool isLike)
			: this(articleVersionId, userId, isLike)
		{
			Id = id;
		}

		#endregion

		#region Static

		private static IVersionVotesBLL _bll = Provider.VersionVotesBLO;

		/// <summary>
		/// Makes/ changes vote on certain article (id is in vm)
		/// </summary>
		/// <param name="vote">vote VM</param>
		/// <returns>true if vote added or changed, otherwise false</returns>
		public static bool Vote(AVersionVoteVM vote)
		{
			return _bll.Vote(Mapper.Map<VersionVoteDTO>(vote));
		}

		/// <summary>
		/// Removes vote from article. IsLike parameter is not important.
		/// </summary>
		/// <param name="disVote"></param>
		/// <returns>Whether vote was removed (false if vote didn't exist)</returns>
		public static bool UnVote(AVersionVoteVM disVote)
		{
			return _bll.UnVote(Mapper.Map<VersionVoteDTO>(disVote));
		}

		/// <summary>
		/// Returns vote VM data
		/// </summary>
		/// <param name="voteId">vote id</param>
		/// <returns>vote VM</returns>
		public static AVersionVoteVM GetVote(Guid voteId)
		{
			return Mapper.Map<AVersionVoteVM>(_bll.GetVote(voteId));
		}

		/// <summary>
		/// Returns vote VM data
		/// </summary>
		/// <param name="userId">user who made a vote</param>
		/// <param name="articleVersionId">id for article version which was voted</param>
		/// <returns>vote VM, null if doesn't exist</returns>
		public static AVersionVoteVM GetVote(Guid userId, Guid articleVersionId)
		{
			return Mapper.Map<AVersionVoteVM>(_bll.GetVote(userId, articleVersionId));
		}

		/// <summary>
		/// Returns vote VM data for all votes about article version
		/// </summary>
		/// <param name="articleVersionId"></param>
		/// <returns></returns>
		public static IEnumerable<AVersionVoteVM> GetVotesForVersion(Guid articleVersionId)
		{
			return Mapper.Map<IEnumerable<AVersionVoteVM>>(_bll.GetVotesForVersion(articleVersionId));
		}

		/// <summary>
		/// Counts votes "for" about article version
		/// </summary>
		/// <param name="articleVersionId">id for article version which was voted</param>
		/// <returns>Amount votes "for"</returns>
		public static int CountVotesFor(Guid articleVersionId)
		{
			return _bll.CountVotesFor(articleVersionId);
		}

		/// <summary>
		/// Counts votes "against" about article version
		/// </summary>
		/// <param name="articleVersionId">id for article version which was voted</param>
		/// <returns>Amount votes "against"</returns>
		public static int CountVotesAgainst(Guid articleVersionId)
		{
			return _bll.CountVotesAgainst(articleVersionId);
		}

		#endregion
	}
}