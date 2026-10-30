using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace MFaaP.MFWSClient
{
	/// <summary>
	/// Methods to retrieve IML automatic metadata suggestions.
	/// </summary>
	public class MFWSVaultAutomaticMetadataOperations
		: MFWSVaultOperationsBase
	{
		/// <summary>
		/// Creates a new <see cref="MFWSVaultAutomaticMetadataOperations"/> object.
		/// </summary>
		/// <param name="client">The client to interact with the server.</param>
		internal MFWSVaultAutomaticMetadataOperations(MFWSClientBase client)
			: base(client)
		{
		}

		#region Retrieve automatic metadata for objects and temporary files

		/// <summary>
		/// Returns the IML metadata suggestions for the given temporary files and object version.
		/// </summary>
		/// <remarks>
		/// For stateless uploads set <see cref="AutomaticMetadataRequestInfo.UploadedFiles"/> (the information returned by
		/// <see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) instead of the obsolete
		/// <see cref="AutomaticMetadataRequestInfo.UploadIds"/>; the two cannot be combined.
		/// </remarks>
		/// <param name="requestInfo">The request for automatic metadata suggestions.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		public async Task<List<PropertyValueSuggestion>> GetAutomaticMetadataAsync(AutomaticMetadataRequestInfo requestInfo,
			CancellationToken token = default)
		{
			// Create the request.
			var request = new RestRequest($"/REST/objects/automaticmetadata.aspx");

			// Add the body to the request.
			request.AddJsonBody(requestInfo);

			// Make the request and get the response.
			var response = await this.MFWSClient.Post<List<PropertyValueSuggestion>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given temporary files and object version.
		/// </summary>
		/// <remarks>
		/// The <paramref name="temporaryFileIds"/> parameter is obsolete: upload IDs identify upload sessions, which need a
		/// sticky session. Use <see cref="GetAutomaticMetadataForUploads(int, CancellationToken, UploadInfo[])"/> with the
		/// information of the stateless uploads (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) instead.
		/// </remarks>
		/// <param name="temporaryFileIds">The ids of any temporary files (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) to include when producing metadata suggestions.</param>
		/// <param name="objectTypeId">The type of the object type the metadata suggestions are for.</param>
		/// <param name="objVer">The version of the object to retrieve suggestions for (may be null).</param>
		/// <param name="propertyValues">The property values to include when producing metadata suggestions.</param>
		/// <param name="metadataSuggestionProviders">The ids of the intelligence services to include (leave empty for all).</param>
		/// <param name="customData">Custom data to provide to the metadata suggestion providers.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		public Task<List<PropertyValueSuggestion>> GetAutomaticMetadataAsync(
			int objectTypeId = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
			ObjVer objVer = null,
			int[] temporaryFileIds = null,
			PropertyValue[] propertyValues = null,
			string[] metadataSuggestionProviders = null,
			string customData = null,
			CancellationToken token = default)
		{
			// Build up the request.
#pragma warning disable CS0618 // Upload IDs are obsolete; the temporaryFileIds parameter still sends them.
			var request = new AutomaticMetadataRequestInfo()
			{
				ObjVer = objVer,
				CustomData = customData,
				MetadataProviderIds = new List<string>(metadataSuggestionProviders ?? new string[0]),
				ObjectType = objectTypeId,
				PropertyValues = new List<PropertyValue>(propertyValues ?? new PropertyValue[0]),
				UploadIds = new List<int>(temporaryFileIds ?? new int[0])
			};
#pragma warning restore CS0618

			// Use the other overload.
			return this.GetAutomaticMetadataAsync(request, token);
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given temporary files and object version.
		/// </summary>
		/// <remarks>
		/// For stateless uploads set <see cref="AutomaticMetadataRequestInfo.UploadedFiles"/> (the information returned by
		/// <see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) instead of the obsolete
		/// <see cref="AutomaticMetadataRequestInfo.UploadIds"/>; the two cannot be combined.
		/// </remarks>
		/// <param name="requestInfo">The request for automatic metadata suggestions.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Any metadata suggestions.</returns>
		public List<PropertyValueSuggestion> GetAutomaticMetadata(AutomaticMetadataRequestInfo requestInfo,
			CancellationToken token = default)
		{
			// Execute the async method.
			return this.GetAutomaticMetadataAsync(requestInfo, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given temporary files and object version.
		/// </summary>
		/// <remarks>
		/// The <paramref name="temporaryFileIds"/> parameter is obsolete: upload IDs identify upload sessions, which need a
		/// sticky session. Use <see cref="GetAutomaticMetadataForUploads(int, CancellationToken, UploadInfo[])"/> with the
		/// information of the stateless uploads (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) instead.
		/// </remarks>
		/// <param name="temporaryFileIds">The ids of any temporary files (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) to include when producing metadata suggestions.</param>
		/// <param name="objectTypeId">The type of the object type the metadata suggestions are for.</param>
		/// <param name="objVer">The version of the object to retrieve suggestions for (may be null).</param>
		/// <param name="propertyValues">The property values to include when producing metadata suggestions.</param>
		/// <param name="metadataSuggestionProviders">The ids of the intelligence services to include (leave empty for all).</param>
		/// <param name="customData">Custom data to provide to the metadata suggestion providers.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		public List<PropertyValueSuggestion> GetAutomaticMetadata(
			int objectTypeId = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
			ObjVer objVer = null,
			int[] temporaryFileIds = null,
			PropertyValue[] propertyValues = null,
			string[] metadataSuggestionProviders = null,
			string customData = null,
			CancellationToken token = default)
		{
			// Execute the async method.
			return this.GetAutomaticMetadataAsync(objectTypeId, objVer, temporaryFileIds, propertyValues, metadataSuggestionProviders, customData, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region Retrieve suggestions for existing objects (no temporary files)

		/// <summary>
		/// Returns the IML metadata suggestions for the given object version.
		/// </summary>
		/// <param name="objVer">The version of the object to retrieve suggestions for (may be null).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		public Task<List<PropertyValueSuggestion>> GetAutomaticMetadataForObjectAsync(ObjVer objVer,
			CancellationToken token = default)
		{
			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));

			// Execute the async method.
			return this.GetAutomaticMetadataAsync(objVer.Type, objVer, token: token);
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given object version.
		/// </summary>
		/// <param name="objVer">The version of the object to retrieve suggestions for (may be null).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		public List<PropertyValueSuggestion> GetAutomaticMetadataForObject(ObjVer objVer,
			CancellationToken token = default)
		{
			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));

			// Execute the async method.
			return this.GetAutomaticMetadataAsync(objVer.Type, objVer, token: token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region Retrieve suggestions for temporary files (no object data)

		/// <summary>
		/// Returns the IML metadata suggestions for the given temporary file.
		/// </summary>
		/// <remarks>
		/// Obsolete: upload IDs identify upload sessions, which need a sticky session. Use
		/// <see cref="GetAutomaticMetadataForUploads(int, CancellationToken, UploadInfo[])"/> with the information of the
		/// stateless uploads (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) instead.
		/// </remarks>
		/// <param name="temporaryFileId">The id of a temporary file (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>).</param>
		/// <param name="objectTypeId">The type of the object type the metadata suggestions are for.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		[Obsolete("Upload IDs are obsolete: use GetAutomaticMetadataForUploads with the information of stateless uploads.")]
		public List<PropertyValueSuggestion> GetAutomaticMetadataForTemporaryFile(int temporaryFileId,
			int objectTypeId = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
			CancellationToken token = default)
		{
			// Execute the async method.
			return this.GetAutomaticMetadataAsync(objectTypeId: objectTypeId, temporaryFileIds: new int[] { temporaryFileId }, token: token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given temporary file.
		/// </summary>
		/// <remarks>
		/// Obsolete: upload IDs identify upload sessions, which need a sticky session. Use
		/// <see cref="GetAutomaticMetadataForUploads(int, CancellationToken, UploadInfo[])"/> with the information of the
		/// stateless uploads (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) instead.
		/// </remarks>
		/// <param name="temporaryFileId">The id of a temporary file (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>).</param>
		/// <param name="objectTypeId">The type of the object type the metadata suggestions are for.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		[Obsolete("Upload IDs are obsolete: use GetAutomaticMetadataForUploads with the information of stateless uploads.")]
		public Task<List<PropertyValueSuggestion>> GetAutomaticMetadataForTemporaryFileAsync(int temporaryFileId,
			int objectTypeId = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
			CancellationToken token = default)
		{
			// Execute the async method.
			return this.GetAutomaticMetadataAsync(objectTypeId: objectTypeId, temporaryFileIds: new int[] { temporaryFileId }, token: token);
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given temporary files.
		/// </summary>
		/// <remarks>
		/// Obsolete: upload IDs identify upload sessions, which need a sticky session. Use
		/// <see cref="GetAutomaticMetadataForUploads(int, CancellationToken, UploadInfo[])"/> with the information of the
		/// stateless uploads (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) instead.
		/// </remarks>
		/// <param name="temporaryFileIds">The ids of any temporary files (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) to include when producing metadata suggestions.</param>
		/// <param name="objectTypeId">The type of the object type the metadata suggestions are for.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		[Obsolete("Upload IDs are obsolete: use GetAutomaticMetadataForUploads with the information of stateless uploads.")]
		public List<PropertyValueSuggestion> GetAutomaticMetadataForTemporaryFiles(
			int objectTypeId = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
			CancellationToken token = default,
			params int[] temporaryFileIds)
		{
			// Execute the async method.
			return this.GetAutomaticMetadataAsync(objectTypeId: objectTypeId, temporaryFileIds: temporaryFileIds, token: token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given temporary files.
		/// </summary>
		/// <remarks>
		/// Obsolete: upload IDs identify upload sessions, which need a sticky session. Use
		/// <see cref="GetAutomaticMetadataForUploads(int, CancellationToken, UploadInfo[])"/> with the information of the
		/// stateless uploads (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) instead.
		/// </remarks>
		/// <param name="temporaryFileIds">The ids of any temporary files (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>) to include when producing metadata suggestions.</param>
		/// <param name="objectTypeId">The type of the object type the metadata suggestions are for.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		[Obsolete("Upload IDs are obsolete: use GetAutomaticMetadataForUploads with the information of stateless uploads.")]
		public Task<List<PropertyValueSuggestion>> GetAutomaticMetadataForTemporaryFilesAsync(
			int objectTypeId = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
			CancellationToken token = default,
			params int[] temporaryFileIds)
		{
			// Execute the async method.
			return this.GetAutomaticMetadataAsync(objectTypeId: objectTypeId, temporaryFileIds: temporaryFileIds, token: token);
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given uploaded files.
		/// </summary>
		/// <param name="uploads">The uploaded files (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>).
		/// Stateless uploads are sent by their file references, others by their upload IDs; the two cannot be mixed.</param>
		/// <param name="objectTypeId">The type of the object type the metadata suggestions are for.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for any metadata suggestions.</returns>
		public Task<List<PropertyValueSuggestion>> GetAutomaticMetadataForUploadsAsync(
			int objectTypeId = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
			CancellationToken token = default,
			params UploadInfo[] uploads)
		{
			// Sanity.
			if (null == uploads)
				throw new ArgumentNullException(nameof(uploads));

			// Build up the request.
			var request = new AutomaticMetadataRequestInfo()
			{
				ObjectType = objectTypeId
			};
			var statelessUploads = uploads.Where(u => null != u && u.IsStateless()).ToList();
			if (statelessUploads.Count > 0)
			{
				// The server identifies these by their file references and extensions.
				if (statelessUploads.Count != uploads.Length)
					throw new ArgumentException("Stateless uploads cannot be mixed with uploads identified by upload IDs.", nameof(uploads));
				request.UploadedFiles = statelessUploads;
			}
			else
			{
#pragma warning disable CS0618 // Upload IDs are obsolete; servers without the stateless upload still need them.
				request.UploadIds = uploads.Where(u => null != u).Select(u => u.UploadID).ToList();
#pragma warning restore CS0618
			}

			// Use the other overload.
			return this.GetAutomaticMetadataAsync(request, token);
		}

		/// <summary>
		/// Returns the IML metadata suggestions for the given uploaded files.
		/// </summary>
		/// <param name="uploads">The uploaded files (<see cref="MFWSVaultObjectFileOperations.UploadFiles(System.IO.FileInfo[])"/>).
		/// Stateless uploads are sent by their file references, others by their upload IDs; the two cannot be mixed.</param>
		/// <param name="objectTypeId">The type of the object type the metadata suggestions are for.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Any metadata suggestions.</returns>
		public List<PropertyValueSuggestion> GetAutomaticMetadataForUploads(
			int objectTypeId = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument,
			CancellationToken token = default,
			params UploadInfo[] uploads)
		{
			// Execute the async method.
			return this.GetAutomaticMetadataForUploadsAsync(objectTypeId, token, uploads)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

	}
}
