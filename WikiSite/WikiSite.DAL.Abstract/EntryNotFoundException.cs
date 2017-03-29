using System;
using System.Runtime.Serialization;

namespace WikiSite.DAL.Abstract
{
	[Serializable]
	public class EntryNotFoundException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public EntryNotFoundException()
		{
		}

		public EntryNotFoundException(string message) : base(message)
		{
		}

		public EntryNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}

		protected EntryNotFoundException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}