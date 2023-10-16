using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RestSharp;

namespace MFaaP.MFWSClient
{
	/// <summary>
	/// Event argument for the <see cref="MFWSClientBase.BeforeExecuteRequest"/> event.
	/// </summary>
	public class RestRequestEventArgs
		: EventArgs
	{
		/// <summary>
		/// The request being executed.
		/// </summary>
		public RestRequest RestRequest { get; private set; }

		/// <summary>
		/// Instantiates a <see cref="RestRequestEventArgs"/> for the supplied request.
		/// </summary>
		/// <param name="restRequest">The request being executed.</param>
		public RestRequestEventArgs(RestRequest restRequest)
		{
			this.RestRequest = restRequest;
		}
	}
}