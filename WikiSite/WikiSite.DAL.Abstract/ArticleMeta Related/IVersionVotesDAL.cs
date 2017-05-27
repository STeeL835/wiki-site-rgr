using System;
using System.Collections.Generic;
using WikiSite.Entities.ArticleMeta_Related;

namespace WikiSite.DAL.Abstract
{
	public interface IVersionVotesDAL
	{
		/// <summary>
		/// Adds a vote of user in db
		/// </summary>
		/// <param name="vote">vote DTO</param>
		/// <returns>Whether operation was successful</returns>
		bool AddVote(VersionVoteDTO vote);
		/// <summary>
		/// Removes user's vote from db
		/// </summary>
		/// <param name="voteId">Id of vote dto</param>
		/// <returns>Whether operation was successful</returns>
		bool RemoveVote(Guid voteId);
		/// <summary>
		/// Changes vote of user
		/// </summary>
		/// <param name="vote">Id of vote dto</param>
		/// <returns>Whether operation was successful</returns>
		bool UpdateVote(VersionVoteDTO vote);

		/// <summary>
		/// Gets vote from db by it's id
		/// </summary>
		/// <param name="voteId">Id of vote DTO</param>
		/// <returns>Vote DTO</returns>
		VersionVoteDTO GetVote(Guid voteId);
		/// <summary>
		/// Gets all votes made by user from db
		/// </summary>
		/// <param name="userId">id of a user</param>
		/// <returns>Votes DTO</returns>
		IEnumerable<VersionVoteDTO> GetVotesOfUser(Guid userId);
		/// <summary>
		/// Gets all votes made about certain article
		/// </summary>
		/// <param name="articleVersionId">Id of an article</param>
		/// <returns>Votes DTO</returns>
		IEnumerable<VersionVoteDTO> GetVotesForVersion(Guid articleVersionId);
	}
}