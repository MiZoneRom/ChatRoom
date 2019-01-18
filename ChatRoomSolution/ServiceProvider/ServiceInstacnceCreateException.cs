using Cores;
using System;

namespace ServiceProvider
{
	public class ServiceInstacnceCreateException : MiException
	{
		public ServiceInstacnceCreateException()
		{
		}

		public ServiceInstacnceCreateException(string message) : base(message)
		{
		}

		public ServiceInstacnceCreateException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}