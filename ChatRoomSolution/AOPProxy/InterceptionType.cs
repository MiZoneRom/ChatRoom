using System;

namespace AOPProxy
{
	public enum InterceptionType
	{
		OnEntry,
		OnExit,
		OnSuccess,
		OnException,
		OnLogException
	}
}