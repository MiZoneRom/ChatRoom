using System;

namespace Cores
{
	public class CacheRegisterException : MiException
	{
		public CacheRegisterException()
		{
		}

		public CacheRegisterException(string message) : base(message)
		{
		}

		public CacheRegisterException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}