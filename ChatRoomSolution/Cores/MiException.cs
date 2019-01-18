using Common;
using System;

namespace Cores
{
	public class MiException : ApplicationException
	{
		public MiException()
		{
			Log.Info(Message);
		}

		public MiException(string message) : base(message)
		{
			Log.Info(message);
		}

		public MiException(string message, Exception inner) : base(message, inner)
		{
			Log.Info(message);
		}
	}
}