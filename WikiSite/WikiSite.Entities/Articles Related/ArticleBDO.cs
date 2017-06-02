using System;

namespace WikiSite.Entities
{
    public class ArticleBDO
    {
        //ArticleDTO
        public Guid Id { get; set; }
        public string ShortUrl { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreationDate { get; set; }

        //ArticleVersionDTO
        public Guid VersionId { get; set; }
        public DateTime LastEditDate { get; set; }
        public Guid EditionAuthorId { get; set; }
        public bool IsApproved { get; set; }

        //ArticleContentDTO
        public Guid ContentId { get; set; }
        public string Heading { get; set; }
        public string Definition { get; set; }
        public string Text { get; set; }
        public Guid MainImage { get; set; }

	    /// <summary>Determines whether the specified object is equal to the current object.</summary>
	    /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
	    /// <param name="obj">The object to compare with the current object. </param>
	    public override bool Equals(object obj)
	    {
		    var bdo = obj as ArticleBDO;
			if (bdo == null) return false;
		    return Equals(bdo);
	    }

	    protected bool Equals(ArticleBDO other)
	    {
		    return Id.Equals(other.Id) && AuthorId.Equals(other.AuthorId) && VersionId.Equals(other.VersionId) && EditionAuthorId.Equals(other.EditionAuthorId) && ContentId.Equals(other.ContentId) && MainImage.Equals(other.MainImage);
	    }

	    /// <summary>Serves as the default hash function. </summary>
	    /// <returns>A hash code for the current object.</returns>
	    public override int GetHashCode()
	    {
		    unchecked
		    {
			    var hashCode = Id.GetHashCode();
			    hashCode = (hashCode * 397) ^ AuthorId.GetHashCode();
			    hashCode = (hashCode * 397) ^ VersionId.GetHashCode();
			    hashCode = (hashCode * 397) ^ EditionAuthorId.GetHashCode();
			    hashCode = (hashCode * 397) ^ ContentId.GetHashCode();
			    hashCode = (hashCode * 397) ^ MainImage.GetHashCode();
			    return hashCode;
		    }
	    }
    }
}