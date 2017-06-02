using System;
using System.Collections.Generic;
using System.Linq;
using WikiSite.BLL.Abstract;
using WikiSite.Caretakers;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.BLL.Default
{
	public class VersionVotesBLO : IVersionVotesBLL
	{
		private readonly IVersionVotesDAL _versionVotesDal;
		private readonly IArticleVersionsDAL _articlesDal;

		public VersionVotesBLO(IVersionVotesDAL versionVotesDal, IArticleVersionsDAL articlesDal)
		{
			if (versionVotesDal == null) throw new ArgumentNullException(nameof(versionVotesDal), "VersionVotes DAL instance is null");
			if (articlesDal == null) throw new ArgumentNullException(nameof(articlesDal), "Articles DAL instance is null");
			_versionVotesDal = versionVotesDal;
			_articlesDal = articlesDal;
		}

		public bool Vote(VersionVoteDTO voteDto)
		{
			CheckThrowDTO(voteDto);

			var success = false;
			var userVote = GetVote(voteDto.UserId, voteDto.ArticleVersionId);

				 /* if user's vote is the same as one that comes then false, 
				  * otherwise (if user didn't vote or his vote wasn't the same), actually change/add it */
			if (userVote != null && userVote.IsVoteFor == voteDto.IsVoteFor) 
				return false;
			else if (userVote == null) // didn't vote
				success = _versionVotesDal.AddVote(voteDto);
			else // other vote value, revote
			{
				voteDto.Id = userVote.Id;
				success = _versionVotesDal.UpdateVote(voteDto);
			}

			CheckVotes(voteDto.ArticleVersionId); // check votes and change IsApproved flag if needed

			return success;
		}

		public bool UnVote(VersionVoteDTO voteDto)
		{
			CheckThrowDTO(voteDto);

			var success = false;
			var userVote = GetVote(voteDto.UserId, voteDto.ArticleVersionId); // getting vote's existence

			if (userVote == null) return false;
			else success = _versionVotesDal.RemoveVote(userVote.Id);

			CheckVotes(voteDto.ArticleVersionId);

			return success;
		}

		public VersionVoteDTO GetVote(Guid voteId)
		{
			ErrorGuard.Check(voteId);

			return _versionVotesDal.GetVote(voteId);
		}

		public VersionVoteDTO GetVote(Guid userId, Guid articleVersionId)
		{
			ErrorGuard.Check(userId);
			ErrorGuard.Check(articleVersionId);

			var votes = GetVotesForVersion(articleVersionId); // getting all votes for article
			var userVote = votes?.FirstOrDefault(dto => dto.UserId == userId); // getting user's vote if exist

			return userVote;
		}

		public IEnumerable<VersionVoteDTO> GetVotesForVersion(Guid articleVersionId)
		{
			ErrorGuard.Check(articleVersionId);

			return _versionVotesDal.GetVotesForVersion(articleVersionId).ToArray(); // getting all votes for article
		}

		public int CountVotesFor(Guid articleVersionId)
		{
			ErrorGuard.Check(articleVersionId);

			return GetVotesForVersion(articleVersionId).Count(dto => dto.IsVoteFor);
		}

		public int CountVotesAgainst(Guid articleVersionId)
		{
			ErrorGuard.Check(articleVersionId);

			return GetVotesForVersion(articleVersionId).Count(dto => !dto.IsVoteFor); // "!" - against
		}

		private void CheckVotes(Guid articleVersionId)
		{
			var @for = CountVotesFor(articleVersionId);
			var against = CountVotesAgainst(articleVersionId);
			var isApproved = GetIsApproval(articleVersionId);

			//if (@for + against < 3) return;

			if (@for >= against && !isApproved) // if it's not approved but needs to
				ChangeApproval(articleVersionId, true);
			else if (against > @for && isApproved) // if it's approved but doesn't deserve it
				ChangeApproval(articleVersionId, false);
		}

		private bool GetIsApproval(Guid articleVersionId)
		{
			var version = _articlesDal.GetVersion(articleVersionId);
			return version.IsApproved;
		}

		private void ChangeApproval(Guid versionId, bool toApprove)
		{
			_articlesDal.ApproveVersion(versionId, toApprove);
		}

		private void CheckThrowDTO(VersionVoteDTO dto)
		{
			ErrorGuard.Check(dto.Id, "DTO's id can't be empty");
			ErrorGuard.Check(dto.ArticleVersionId, "ArticleVersion id can't be empty");
			ErrorGuard.Check(dto.UserId, "User id can't be empty");
		}
	}
}