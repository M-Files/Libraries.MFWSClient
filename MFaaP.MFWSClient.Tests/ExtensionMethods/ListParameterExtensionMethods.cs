using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace MFaaP.MFilesAPI.Tests.ExtensionMethods
{
	public static class ListParameterExtensionMethods
	{
#pragma warning disable CS0618 // Type or member is obsolete
		public static string GetQuerystringParameter(this List<Parameter> parameters, string name)
#pragma warning restore CS0618 // Type or member is obsolete
		{
			return parameters?
						.Where(p => p.Type == ParameterType.QueryString)
						.Where(p => p.Name == name)
						.Select(p => p.Value as string)
						.FirstOrDefault();
		}

#pragma warning disable CS0618 // Type or member is obsolete
		public static string GetMethodQuerystringParameter(this List<Parameter> parameters)
#pragma warning restore CS0618 // Type or member is obsolete
		{
			return parameters.GetQuerystringParameter("_method");
		}
	}
}
