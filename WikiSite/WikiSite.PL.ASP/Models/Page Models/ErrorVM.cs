namespace WikiSite.PL.ASP.Models
{
	public class ErrorVM
	{
		private string _header;
		private string _title;
		private string _message;

		public string Header
		{
			get { return _header; }
			set
			{
				Caretakers.ErrorGuard.Check(value);
				_header = value;
			}
		}

		public string Title
		{
			get { return _title; }
			set
			{
				Caretakers.ErrorGuard.Check(value);
				_title = value;
			}
		}

		public string Message
		{
			get { return _message; }
			set
			{
				Caretakers.ErrorGuard.Check(value);
				_message = value;
			}
		}

		public string ExceptionDetails { get; set; }

		public ErrorVM(string header, string title, string message)
		{
			Header = header;
			Title = title;
			Message = message;
		}

		public ErrorVM(string header, string title, string message, string exceptionDetails)
			: this(header, title, message)
		{
			ExceptionDetails = exceptionDetails;
		}
	}
}