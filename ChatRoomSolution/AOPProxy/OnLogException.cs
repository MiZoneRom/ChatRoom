using System;
using System.Collections.Generic;

namespace AOPProxy
{
	public delegate void OnLogException(string methodName, Dictionary<string, object> parameters, Exception ex);
}