using System;
using WikiSite.Entities;

namespace WikiSite.Caretakers
{
    public static class ErrorGuard
    {
        public static DateTime DefaultSqlDateTime = new DateTime(1753, 1, 1);

        public static void Check(Guid id, string message = "Id is empty.")
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id), message);
        }

        public static void Check(string line, string message = "Text doesn't make sense.")
        {
            if (string.IsNullOrWhiteSpace(line)) throw new ArgumentNullException(nameof(line), message);
        }

        public static void Check(DateTime date, string message = "default(SqlDateTime)")
        {
            if (message == "default(SqlDateTime)")
            {
                message =
                    $"Date is less than or equal to default value for SQL Server ({DefaultSqlDateTime.ToShortDateString()}).";
            }
            if (DateTime.Compare(date, DefaultSqlDateTime) <= 0) throw new ArgumentNullException(nameof(date), message);
        }

        public static void Check(int number, string message = "Number is default value.")
        {
            if (number <= default(int)) throw new ArgumentNullException(nameof(number), message);
        }

        public static void Check(ArticleBDO bdo)
        {
            if (bdo == null) throw new ArgumentNullException(nameof(bdo), "Article BDO is null.");
            Check(bdo.Id, "Article BDO doesn't contain ID.");
            Check(bdo.ShortUrl, "Article BDO doesn't contain a short url.");
            Check(bdo.AuthorId, "Article BDO doesn't contain author id.");
            Check(bdo.Heading, "Article BDO doesn't contain a heading.");
            Check(bdo.CreationDate,
                $"Creation date of Article BDO is less than or equal to default value ({DefaultSqlDateTime.ToShortDateString()}).");
            Check(bdo.LastEditDate,
                $"Last edit date of Article BDO is less than or equal to default value ({DefaultSqlDateTime.ToShortDateString()}).");
            Check(bdo.EditionAuthorId, "Article BDO doesn't contain edition author id.");
            Check(bdo.Definition, "Article BDO doesn't contain a definition.");
            Check(bdo.Text, "Article BDO doesn't contain a text.");
        }

        public static void Check(byte[] bytes, string message = "Array of bytes is null.")
        {
            if(bytes == null) throw new ArgumentNullException(nameof(bytes), message);
        }
    }
}
