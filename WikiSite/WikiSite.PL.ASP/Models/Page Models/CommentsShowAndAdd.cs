namespace WikiSite.PL.ASP.Models
{
	public class CommentsShowAndAdd : CommentVM
	{
		public CommentArticleVersionModel[] AllCommentElements { get; set; }

		public CommentsShowAndAdd() : base()
		{
			
		}

		public CommentsShowAndAdd(CommentVM model, CommentArticleVersionModel[] commentElems)
			: base(model.Id, model.ArticleId, model.AuthorId, model.Text, model.DateOfCreation)
		{
			AllCommentElements = commentElems;
		}
	}
}