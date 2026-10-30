using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace MFaaP.MFWSClient
{
	public abstract partial class MFWSClientBase
	{
		/// <summary>
		/// The name of the querystring parameter with which uploads ask for file references instead of upload IDs.
		/// </summary>
		internal const string StatelessUploadQueryParameterName = "statelessUpload";

		/// <summary>
		/// Whether the server supports the stateless file upload, once retrieved.
		/// </summary>
		private bool? statelessFileUploadSupported;

		/// <summary>
		/// Retrieves the capabilities of the server.
		/// </summary>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the capabilities.</returns>
		public async Task<ServerVaultCapabilities> GetServerCapabilitiesAsync(CancellationToken token = default)
		{
			// Build up the request.
			var request = new RestRequest("/REST/server/capabilities");

			// Execute the request.
			var response = await this.Get<ServerVaultCapabilities>(request, token)
				.ConfigureAwait(false);

			// Return the content.
			return response?.Data;
		}

		/// <summary>
		/// Retrieves the capabilities of the server.
		/// </summary>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The capabilities.</returns>
		public ServerVaultCapabilities GetServerCapabilities(CancellationToken token = default)
		{
			// Execute the async method.
			return this.GetServerCapabilitiesAsync(token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Checks whether uploads should ask for file references instead of upload IDs: the server advertises the
		/// <see cref="ServerVaultCapabilities.FileUploadStatelessSupported"/> capability.
		/// </summary>
		/// <remarks>
		/// An upload ID is valid only in the M-Files Server process that created it, so every request of the upload
		/// must reach the same server (sticky session). A file reference can be used by any server instance.
		/// </remarks>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>True if uploads should ask for file references.</returns>
		internal async Task<bool> ShouldUseStatelessFileUploadAsync(CancellationToken token = default)
		{
			// Retrieve the capability once. An older server without it returns no data, so upload IDs are used.
			if (false == this.statelessFileUploadSupported.HasValue)
			{
				try
				{
					var capabilities = await this.GetServerCapabilitiesAsync(token)
						.ConfigureAwait(false);
					this.statelessFileUploadSupported = capabilities?.FileUploadStatelessSupported ?? false;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch
				{
					// Use upload IDs for this upload and retry the check on the next one.
					return false;
				}
			}
			return this.statelessFileUploadSupported.Value;
		}

		/// <summary>
		/// Asks for a file reference instead of an upload ID if the server supports the stateless file upload.
		/// </summary>
		/// <param name="request">The upload request.</param>
		/// <param name="token">A cancellation token for the request.</param>
		internal async Task AddStatelessUploadParameterIfSupportedAsync(RestRequest request, CancellationToken token = default)
		{
			// Sanity.
			if (null == request)
				throw new ArgumentNullException(nameof(request));

			if (await this.ShouldUseStatelessFileUploadAsync(token).ConfigureAwait(false))
				request.AddQueryParameter(StatelessUploadQueryParameterName, "true");
		}
	}
}
