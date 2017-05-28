using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.BLL.Abstract
{
	public interface IVersionVotesBLL
	{
		/// <summary>
		/// Makes/ changes vote on certain article (id is in dto)
		/// </summary>
		/// <param name="voteDto"> vote DTO</param>
		/// <returns>true if vote added or changed, otherwise false</returns>
		bool Vote(VersionVoteDTO voteDto);
		/// <summary>
		/// Removes vote from article. IsVoteFor parameter is not important.
		/// </summary>
		/// <param name="voteDto"></param>
		/// <returns>Whether vote was removed (false if vote didn't exist)</returns>
		bool UnVote(VersionVoteDTO voteDto);
		
		/// <summary>
		/// Returns vote DTO data
		/// </summary>
		/// <param name="voteId">vote id</param>
		/// <returns>vote DTO</returns>
		VersionVoteDTO GetVote(Guid voteId);
		/// <summary>
		/// Returns vote DTO data
		/// </summary>
		/// <param name="userId">user who made a vote</param>
		/// <param name="articleVersionId">id for article version which was voted</param>
		/// <returns>vote DTO, null if doesn't exist</returns>
		VersionVoteDTO GetVote(Guid userId, Guid articleVersionId);
		/// <summary>
		/// Returns vote DTO data for all votes about article version
		/// </summary>
		/// <param name="articleVersionId"></param>
		/// <returns></returns>
		IEnumerable<VersionVoteDTO> GetVotesForVersion(Guid articleVersionId);

		/// <summary>
		/// Counts votes "for" about article version
		/// </summary>
		/// <param name="articleVersionId">id for article version which was voted</param>
		/// <returns>Amount votes "for"</returns>
		int CountVotesFor(Guid articleVersionId);
		/// <summary>
		/// Counts votes "against" about article version
		/// </summary>
		/// <param name="articleVersionId">id for article version which was voted</param>
		/// <returns>Amount votes "against"</returns>
		int CountVotesAgainst(Guid articleVersionId);
	}
}