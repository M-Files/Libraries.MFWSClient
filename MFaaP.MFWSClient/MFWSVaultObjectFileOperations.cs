using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Extensions;

namespace MFaaP.MFWSClient
{
	/// <summary>
	/// Object file operations.
	/// </summary>
    public class MFWSVaultObjectFileOperations
        : MFWSVaultOperationsBase
    {
        /// <inheritdoc />
        internal MFWSVaultObjectFileOperations(MFWSClientBase client)
            : base(client)
        {
        }
		
		#region Downloading files

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objVer">The object that contains the file to download.</param>
		/// <param name="fileVer">The file to download.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public async Task<byte[]> DownloadFileAsync(ObjVer objVer, FileVer fileVer, CancellationToken token = default)
		{
			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));
			if (null == fileVer)
				throw new ArgumentNullException(nameof(fileVer));

			// Extract the URI elements.
			int objectTypeId;
			string objectId, objectVersionId;
			objVer.GetUriParameters(out objectTypeId, out objectId, out objectVersionId);
			string fileId, fileVersionId;
			fileVer.GetUriParameters(out fileId, out fileVersionId);

			// Build up the request.
			var request = new RestRequest($"/REST/objects/{objectTypeId}/{objectId}/{objectVersionId}/files/{fileId}/content");

			// Execute the request.
			var response = await this.MFWSClient.Get(request, token)
				.ConfigureAwait(false);

			// Return the content.
			return response?.RawBytes;
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objVer">The object that contains the file to download.</param>
		/// <param name="fileVer">The file to download.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public byte[] DownloadFile(ObjVer objVer, FileVer fileVer, CancellationToken token = default)
		{
			// Execute the async method.
			return this.DownloadFileAsync(objVer, fileVer, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objectType">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="fileId">The Id of the file.</param>
		/// <param name="objectVersion">The version of the object, or null for the latest.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public Task<byte[]> DownloadFileAsync(int objectType, int objectId, int fileId, int? objectVersion = null, CancellationToken token = default)
        {
			// Use the other overload.
			return this.DownloadFileAsync(new ObjVer()
			{
				Type = objectType,
				ID = objectId,
				Version =  objectVersion ?? 0
			}, new FileVer()
			{
				ID = fileId
			}, token);
        }

        /// <summary>
        /// Initiates the download of a specific file.
        /// </summary>
        /// <param name="objectType">The Id of the object type.</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="fileId">The Id of the file.</param>
        /// <param name="objectVersion">The version of the object, or null for the latest.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>The raw response from the HTTP request.</returns>
        public byte[] DownloadFile(int objectType, int objectId, int fileId, int? objectVersion = null, CancellationToken token = default)
        {
            // Execute the async method.
            return this.DownloadFileAsync(objectType, objectId, fileId, objectVersion, token)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objVer">The object that contains the file to download.</param>
		/// <param name="fileVer">The file to download.</param>
		/// <param name="outputStream">The output stream for the response to be written to.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public async Task DownloadFileAsync(ObjVer objVer, FileVer fileVer, System.IO.Stream outputStream, CancellationToken token = default)
		{
			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));
			if (null == fileVer)
				throw new ArgumentNullException(nameof(fileVer));

			// Extract the URI elements.
			int objectTypeId;
			string objectId, objectVersionId;
			objVer.GetUriParameters(out objectTypeId, out objectId, out objectVersionId);
			string fileId, fileVersionId;
			fileVer.GetUriParameters(out fileId, out fileVersionId);

			// Build up the request.
			var request = new RestRequest($"/REST/objects/{objectTypeId}/{objectId}/{objectVersionId}/files/{fileId}/content")
			{
                ResponseWriter = (responseStream) => outputStream
            };

			// Execute the request.
			await this.MFWSClient.Get(request, token)
				.ConfigureAwait(false);
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objVer">The object that contains the file to download.</param>
		/// <param name="fileVer">The file to download.</param>
		/// <param name="outputStream">The output stream for the response to be written to.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public void DownloadFile(ObjVer objVer, FileVer fileVer, System.IO.Stream outputStream, CancellationToken token = default)
		{
			// Execute the async method.
			this.DownloadFileAsync(objVer, fileVer, outputStream, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objectType">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="fileId">The Id of the file.</param>
		/// <param name="outputStream">The output stream for the response to be written to.</param>
		/// <param name="objectVersion">The version of the object, or null for the latest.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public Task DownloadFileAsync(int objectType, int objectId, int fileId, System.IO.Stream outputStream, int? objectVersion = null, CancellationToken token = default)
		{
			// Use the other overload.
			return this.DownloadFileAsync(new ObjVer()
				{
					Type = objectType,
					ID = objectId,
					Version = objectVersion ?? 0
				}, new FileVer()
				{
					ID = fileId
				},
				outputStream,
				token);
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objectType">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="fileId">The Id of the file.</param>
		/// <param name="outputStream">The output stream for the response to be written to.</param>
		/// <param name="objectVersion">The version of the object, or null for the latest.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public void DownloadFile(int objectType, int objectId, int fileId, System.IO.Stream outputStream, int? objectVersion = null, CancellationToken token = default)
        {
            // Execute the async method.
            this.DownloadFileAsync(objectType, objectId, fileId, outputStream, objectVersion, token)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Initiates the download of a specific file.
        /// </summary>
        /// <param name="objectType">The Id of the object type.</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="fileId">The Id of the file.</param>
        /// <param name="outputFileName">The full path to the file to output to.</param>
        /// <param name="objectVersion">The version of the object, or null for the latest.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>An awaitable task for the download process.</returns>
        public Task DownloadFileAsync(int objectType, int objectId, int fileId, string outputFileName, int? objectVersion = null, CancellationToken token = default)
        {
            // Create a FileInfo for the output.
            var outputFileInfo = new System.IO.FileInfo(outputFileName);

            // Use the other overload to download it.
            return this.DownloadFileAsync(objectType, objectId, fileId, outputFileInfo, objectVersion, token);
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objVer">The object that contains the file to download.</param>
		/// <param name="fileVer">The file to download.</param>
		/// <param name="outputFileName">The full path to the file to output to.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public void DownloadFile(ObjVer objVer, FileVer fileVer, string outputFileName, CancellationToken token = default)
		{
			// Create a FileInfo for the output.
			var outputFileInfo = new System.IO.FileInfo(outputFileName);

			// Execute the async method.
			this.DownloadFileAsync(objVer, fileVer, outputFileInfo, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objectType">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="fileId">The Id of the file.</param>
		/// <param name="outputFileName">The full path to the file to output to.</param>
		/// <param name="objectVersion">The version of the object, or null for the latest.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the download process.</returns>
		public void DownloadFile(int objectType, int objectId, int fileId, string outputFileName, int? objectVersion = null, CancellationToken token = default)
        {
            // Execute the async method.
            this.DownloadFileAsync(objectType, objectId, fileId, outputFileName, objectVersion, token)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Initiates the download of a specific file.
        /// </summary>
        /// <param name="objectType">The Id of the object type.</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="fileId">The Id of the file.</param>
        /// <param name="outputFileInfo">The file to output the content to (will be overwritten if exists).</param>
        /// <param name="objectVersion">The version of the object, or null for the latest.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>An awaitable task for the download process.</returns>
        public Task DownloadFileAsync(int objectType, int objectId, int fileId, System.IO.FileInfo outputFileInfo, int? objectVersion = null,
            CancellationToken token = default)
        {
            // Sanity.
            if (null == outputFileInfo)
                throw new ArgumentNullException(nameof(outputFileInfo));

			// Use the other overload.
			return this.DownloadFileAsync(new ObjVer()
			{
				Type = objectType,
				ID = objectId,
				Version = objectVersion ?? 0
			}, new FileVer()
			{
				ID = fileId
			}, outputFileInfo, token);
		}

        /// <summary>
        /// Initiates the download of a specific file.
        /// </summary>
        /// <param name="objectType">The Id of the object type.</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="fileId">The Id of the file.</param>
        /// <param name="outputFileInfo">The file to output the content to (will be overwritten if exists).</param>
        /// <param name="objectVersion">The version of the object, or null for the latest.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>An awaitable task for the download process.</returns>
        public void DownloadFile(int objectType, int objectId, int fileId, System.IO.FileInfo outputFileInfo, int? objectVersion = null,
            CancellationToken token = default)
        {
            // Execute the async method.
            this.DownloadFileAsync(objectType, objectId, fileId, outputFileInfo, objectVersion, token)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objVer">The object that contains the file to download.</param>
		/// <param name="fileVer">The file to download.</param>
		/// <param name="outputFileInfo">The file to output the content to (will be overwritten if exists).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the download process.</returns>
		public async Task DownloadFileAsync(ObjVer objVer, FileVer fileVer, System.IO.FileInfo outputFileInfo,
			CancellationToken token = default)
		{
			// Sanity.
			if (null == outputFileInfo)
				throw new ArgumentNullException(nameof(outputFileInfo));

			// Delete the file if it already exists.
			if (outputFileInfo.Exists)
				outputFileInfo.Delete();

			// Open a stream to the file.
			using (var stream = outputFileInfo.Create())
			{
				// Download the file to disk.
				await this.DownloadFileAsync(objVer, fileVer, stream, token)
					.ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Initiates the download of a specific file.
		/// </summary>
		/// <param name="objVer">The object that contains the file to download.</param>
		/// <param name="fileVer">The file to download.</param>
		/// <param name="outputFileInfo">The file to output the content to (will be overwritten if exists).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The raw response from the HTTP request.</returns>
		public void DownloadFile(ObjVer objVer, FileVer fileVer, System.IO.FileInfo outputFileInfo, CancellationToken token = default)
		{
			// Execute the async method.
			this.DownloadFileAsync(objVer, fileVer, outputFileInfo, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region Uploading files

		/// <summary>
		/// Uploads files to the temporary location.
		/// </summary>
		/// <remarks>
		/// If the server supports the stateless file upload (<see cref="ServerVaultCapabilities.FileUploadStatelessSupported"/>),
		/// each upload is identified by a file reference (<see cref="UploadInfo.FileRef"/>) with the MD5 checksum of its content
		/// (<see cref="UploadInfo.Checksum"/>), which any server instance can use. Otherwise it is identified by an upload ID
		/// (<see cref="UploadInfo.UploadID"/>), which is obsolete and needs a sticky session. Pass the returned information as is
		/// when creating an object or adding the files to an object.
		/// </remarks>
		/// <param name="files">The files to upload.</param>
		/// <returns>Information on the upload.</returns>
		public Task<UploadInfo[]> UploadFilesAsync(params FileInfo[] files)
        {
            return this.UploadFilesAsync(CancellationToken.None, files);
        }

        /// <summary>
        /// Uploads files to the temporary location.
        /// </summary>
        /// <remarks>
        /// If the server supports the stateless file upload (<see cref="ServerVaultCapabilities.FileUploadStatelessSupported"/>),
        /// each upload is identified by a file reference (<see cref="UploadInfo.FileRef"/>) with the MD5 checksum of its content
        /// (<see cref="UploadInfo.Checksum"/>), which any server instance can use. Otherwise it is identified by an upload ID
        /// (<see cref="UploadInfo.UploadID"/>), which is obsolete and needs a sticky session. Pass the returned information as is
        /// when creating an object or adding the files to an object.
        /// </remarks>
        /// <param name="files">The files to upload.</param>
        /// <returns>Information on the upload.</returns>
        public UploadInfo[] UploadFiles(params FileInfo[] files)
        {
            // Execute the async method.
            return this.UploadFilesAsync(files)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Uploads files to the temporary location.
        /// </summary>
        /// <remarks>
        /// If the server supports the stateless file upload (<see cref="ServerVaultCapabilities.FileUploadStatelessSupported"/>),
        /// each upload is identified by a file reference (<see cref="UploadInfo.FileRef"/>) with the MD5 checksum of its content
        /// (<see cref="UploadInfo.Checksum"/>), which any server instance can use. Otherwise it is identified by an upload ID
        /// (<see cref="UploadInfo.UploadID"/>), which is obsolete and needs a sticky session. Pass the returned information as is
        /// when creating an object or adding the files to an object.
        /// </remarks>
        /// <param name="files">The files to upload.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>Information on the upload.</returns>
        public async Task<UploadInfo[]> UploadFilesAsync(CancellationToken token, params FileInfo[] files)
        {
            // Sanity.
            if (null == files)
                throw new ArgumentNullException(nameof(files));
            if (files.Length == 0)
                return new UploadInfo[0];

            // Create the request.
            // TODO: Possibly split this into multiple requests.
            // TODO: Can this be monitored?
            var request = new RestRequest("/REST/files.aspx");
            foreach (var file in files)
            {
                request.AddFile(file.Name, file.FullName);
            }

			// Ask for file references instead of upload IDs if the server supports it.
			// The server returns the file references with the checksums of the content.
			await this.MFWSClient.AddStatelessUploadParameterIfSupportedAsync(request, token)
				.ConfigureAwait(false);

			// If we are only uploading one file then
			// the server responds with a single item, not an array.
			List<UploadInfo> data = new List<UploadInfo>();
			if (files.Length == 1)
			{
				{
					// Make the request and get the response.
					var item = (await this.MFWSClient.Post<UploadInfo>(request, token)
						.ConfigureAwait(false))?.Data;
					if (null != item)
						data.Add(item);
				}
            }
			else
			{
				// Make the request and get the response.
				data = (await this.MFWSClient.Post<List<UploadInfo>>(request, token)
					.ConfigureAwait(false))?.Data;
            }

            // Ensure the upload info is updated.
            for (var i = 0; i < data?.Count; i++)
            {
                var uploadInfo = data[i];
                var file = files[i];
                uploadInfo.Title = file.Name;
                uploadInfo.Extension = file.Extension.Substring(1); // Remove the dot.
                uploadInfo.Size = file.Length;
            }

            // Return the data.
            return data?.ToArray();

        }

        /// <summary>
        /// Uploads files to the temporary location.
        /// </summary>
        /// <remarks>
        /// If the server supports the stateless file upload (<see cref="ServerVaultCapabilities.FileUploadStatelessSupported"/>),
        /// each upload is identified by a file reference (<see cref="UploadInfo.FileRef"/>) with the MD5 checksum of its content
        /// (<see cref="UploadInfo.Checksum"/>), which any server instance can use. Otherwise it is identified by an upload ID
        /// (<see cref="UploadInfo.UploadID"/>), which is obsolete and needs a sticky session. Pass the returned information as is
        /// when creating an object or adding the files to an object.
        /// </remarks>
        /// <param name="files">The files to upload.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>Information on the upload.</returns>
        public UploadInfo[] UploadFiles(CancellationToken token, params FileInfo[] files)
        {
            // Execute the async method.
            return this.UploadFilesAsync(token, files)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

        }

        #endregion

        #region Adding files to existing objects

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objectType">The Id of the object type.</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="files">The files to attach.</param>
        /// <param name="objectVersion">The version of the object, or null for the latest.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>The updated object version.</returns>
        public async Task<ExtendedObjectVersion> AddFilesAsync(int objectType, int objectId, int? objectVersion = null, CancellationToken token = default, params FileInfo[] files)
        {
            // Sanity.
            if (null == files)
                throw new ArgumentNullException(nameof(files));

            // Firstly, upload the temporary files.
            var uploadInfo = await this.UploadFilesAsync(token, files);

            // Remove the extension from the item title if it exists.
            foreach (var item in uploadInfo)
            {
                // Sanity.
                if (string.IsNullOrWhiteSpace(item.Title) || string.IsNullOrWhiteSpace(item.Extension))
                    continue;

                // If the title ends with the extension then remove it.
                if (true == item.Title?.EndsWith("." + item.Extension))
                {
                    // Note the +1 is because we want to remove the dot as well.
                    item.Title = item.Title.Substring(0, item.Title.Length - (item.Extension.Length + 1));
                }
            }

            // Create the version string to be used for the uri segment.
            var versionString = objectVersion?.ToString() ?? "latest";

            // Build up the request.
            var request = new RestRequest($"/REST/objects/{objectType}/{objectId}/{versionString}/files/upload");
            request.AddJsonBody(uploadInfo);

            // Execute the request.
            var response = await this.MFWSClient.Post<ExtendedObjectVersion>(request, token)
                .ConfigureAwait(false);

            // Return the content.
            return response?.Data;
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objectType">The Id of the object type.</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="files">The files to attach.</param>
        /// <param name="objectVersion">The version of the object, or null for the latest.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>The updated object version.</returns>
        public ExtendedObjectVersion AddFiles(int objectType, int objectId, int? objectVersion = null, CancellationToken token = default, params FileInfo[] files)
        {
            // Execute the async method.
            return this.AddFilesAsync(objectType, objectId, objectVersion, token, files)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objectType">The Id of the object type.</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="files">The files to attach.</param>
        /// <param name="objectVersion">The version of the object, or null for the latest.</param>
        /// <returns>The updated object version.</returns>
        public Task<ExtendedObjectVersion> AddFilesAsync(int objectType, int objectId, int? objectVersion = null, params FileInfo[] files)
        {
            // Execute the other overload.
            return this.AddFilesAsync(objectType, objectId, objectVersion, default, files);
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objectType">The Id of the object type.</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="files">The files to attach.</param>
        /// <param name="objectVersion">The version of the object, or null for the latest.</param>
        /// <returns>The updated object version.</returns>
        public ExtendedObjectVersion AddFiles(int objectType, int objectId, int? objectVersion = null, params FileInfo[] files)
        {
            // Execute the other overload.
            return this.AddFiles(objectType, objectId, objectVersion, default, files);
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objId">The object to add the file to.</param>
        /// <param name="files">The files to attach.</param>
        /// <returns>The updated object version.</returns>
        public Task<ExtendedObjectVersion> AddFilesAsync(ObjID objId, params FileInfo[] files)
        {
            // Sanity.
            if (null == objId)
                throw new ArgumentNullException(nameof(objId));

            // Execute the other overload.
            return this.AddFilesAsync(objId.Type, objId.ID, objectVersion: null, token: default, files: files);
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objId">The object to add the file to.</param>
        /// <param name="files">The files to attach.</param>
        /// <returns>The updated object version.</returns>
        public ExtendedObjectVersion AddFiles(ObjID objId, params FileInfo[] files)
        {
            // Sanity.
            if (null == objId)
                throw new ArgumentNullException(nameof(objId));

            // Execute the other overload.
            return this.AddFiles(objId.Type, objId.ID, objectVersion: null, token: default, files: files);
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objId">The object to add the file to.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <param name="files">The files to attach.</param>
        /// <returns>The updated object version.</returns>
        public Task<ExtendedObjectVersion> AddFilesAsync(ObjID objId, CancellationToken token = default, params FileInfo[] files)
        {
            // Sanity.
            if (null == objId)
                throw new ArgumentNullException(nameof(objId));

            // Execute the other overload.
            return this.AddFilesAsync(objId.Type, objId.ID, objectVersion: null, token: token, files: files);
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objId">The object to add the file to.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <param name="files">The files to attach.</param>
        /// <returns>The updated object version.</returns>
        public ExtendedObjectVersion AddFiles(ObjID objId, CancellationToken token = default, params FileInfo[] files)
        {
            // Sanity.
            if (null == objId)
                throw new ArgumentNullException(nameof(objId));

            // Execute the other overload.
            return this.AddFiles(objId.Type, objId.ID, objectVersion: null, token: token, files: files);
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objVer">The object to add the file to.</param>
        /// <param name="files">The files to attach.</param>
        /// <returns>The updated object version.</returns>
        public Task<ExtendedObjectVersion> AddFilesAsync(ObjVer objVer, params FileInfo[] files)
        {
            // Sanity.
            if (null == objVer)
                throw new ArgumentNullException(nameof(objVer));

            // Execute the other overload.
            return this.AddFilesAsync(objVer.Type, objVer.ID, objVer.Version, default, files);
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objVer">The object to add the file to.</param>
        /// <param name="files">The files to attach.</param>
        /// <returns>The updated object version.</returns>
        public ExtendedObjectVersion AddFiles(ObjVer objVer, params FileInfo[] files)
        {
            // Sanity.
            if (null == objVer)
                throw new ArgumentNullException(nameof(objVer));

            // Execute the other overload.
            return this.AddFiles(objVer.Type, objVer.ID, objVer.Version, default, files);
        }

        /// <summary>
        /// Adds files to an existing object.
        /// </summary>
        /// <param name="objVer">The object to add the file to.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <param name="files">The files to attach.</param>
        /// <returns>The updated object version.</returns>
        public Task<ExtendedObjectVersion> AddFilesAsync(ObjVer objVer, CancellationToken token = default, params FileInfo[] files)
        {
            // Sanity.
            if (null == objVer)
                throw new ArgumentNullException(nameof(objVer));

            // Execute the other overload.
            return this.AddFilesAsync(objVer.Type, objVer.ID, objVer.Version, token, files);
		}

		/// <summary>
		/// Adds files to an existing object.
		/// </summary>
		/// <param name="objVer">The object to add the file to.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <param name="files">The files to attach.</param>
		/// <returns>The updated object version.</returns>
		public ExtendedObjectVersion AddFiles(ObjVer objVer, CancellationToken token = default, params FileInfo[] files)
		{
			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));

			// Execute the other overload.
			return this.AddFiles(objVer.Type, objVer.ID, objVer.Version, token, files);
		}

		/// <summary>
		/// Replaces the content of a(n existing) single file.
		/// </summary>
		/// <param name="objVer">The object that the file belongs to.</param>
		/// <param name="fileVer">The file to replace.</param>
		/// <param name="filePath">The path to the file to upload in its replacement.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The updated object version.</returns>
		public async Task<ExtendedObjectVersion> UploadFileAsync(ObjVer objVer, FileVer fileVer, string filePath, CancellationToken token = default)
		{
			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));
			if (null == fileVer)
				throw new ArgumentNullException(nameof(fileVer));
			try
			{
				if (false == System.IO.File.Exists(filePath))
					throw new ArgumentException("The file path must exist.", nameof(filePath));
			}
			catch (Exception e)
			{
				throw new ArgumentException("Could not confirm file path location.", nameof(filePath), e);
			}

			// Extract the URI elements.
			int objectTypeId;
			string objectId, objectVersionId;
			objVer.GetUriParameters(out objectTypeId, out objectId, out objectVersionId);
			string fileId, fileVersionId;
			fileVer.GetUriParameters(out fileId, out fileVersionId);

			// Build up the request.
			var request = new RestRequest($"/REST/objects/{objectTypeId}/{objectId}/{objectVersionId}/files/{fileId}/content.aspx");

			// Add the file as a body.
			var fileInfo = new System.IO.FileInfo(filePath);
			request.AddFile(fileInfo.Name, fileInfo.FullName);

			// Upload statelessly if the server supports it. The server commits the content with the checksum it calculates.
			await this.MFWSClient.AddStatelessUploadParameterIfSupportedAsync(request, token)
				.ConfigureAwait(false);

			// Execute the request.
			var response = await this.MFWSClient.Put<ExtendedObjectVersion>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;

		}

		/// <summary>
		/// Replaces the content of a(n existing) single file.
		/// </summary>
		/// <param name="objVer">The object that the file belongs to.</param>
		/// <param name="fileVer">The file to replace.</param>
		/// <param name="filePath">The path to the file to upload in its replacement.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The updated object version.</returns>
		public ExtendedObjectVersion UploadFile(ObjVer objVer, FileVer fileVer, string filePath, CancellationToken token = default)
		{
			// Use the other overload.
			return this.UploadFileAsync(objVer, fileVer, filePath, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region Renaming a file

		/// <summary>
		/// Renames a specific file.
		/// </summary>
		/// <param name="objectType">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="fileId">The Id of the file to rename.</param>
		/// <param name="newFileName">The (new) file name.</param>
		/// <param name="objectVersion">The version of the object, or null for the latest.</param>
		/// <param name="fileVersion">The version of the file, or null for the latest.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The updated object version.</returns>
		public async Task<ObjectVersion> RenameFileAsync(int objectType,
			int objectId,
			int fileId,
			string newFileName,
			int? objectVersion = null,
			int? fileVersion = null,
			CancellationToken token = default)
        {
			// Create the version strings to be used for the uri segment.
			var objectVersionString = objectVersion?.ToString() ?? "latest";
			var fileVersionString = fileVersion?.ToString() ?? "latest";

			// Build up the request.
			var request = new RestRequest($"/REST/objects/{objectType}/{objectId}/{objectVersionString}/files/{fileId}/{fileVersionString }/title");

			// Pass in the new file name.
			// ref: http://www.m-files.com/mfws/resources/objects/type/objectid/version/files/file/title.html
			request.AddJsonBody(new { Value = newFileName });
			
			// Execute the request.
			var response = await this.MFWSClient.Put<ObjectVersion>(request, token)
                .ConfigureAwait(false);

			// Return the content.
			return response?.Data;

		}

		/// <summary>
		/// Renames a specific file.
		/// </summary>
		/// <param name="objectType">The Id of the object type.</param>
		/// <param name="objectId">The Id of the object.</param>
		/// <param name="fileId">The Id of the file to rename.</param>
		/// <param name="newFileName">The (new) file name.</param>
		/// <param name="objectVersion">The version of the object, or null for the latest.</param>
		/// <param name="fileVersion">The version of the file, or null for the latest.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The updated object version.</returns>
		public ObjectVersion RenameFile(int objectType,
			int objectId,
			int fileId,
			string newFileName,
			int? objectVersion = null,
			int? fileVersion = null,
			CancellationToken token = default)
		{
			// Execute the async method.
			return this.RenameFileAsync(objectType, objectId, fileId, newFileName, objectVersion, fileVersion, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();

		}

		/// <summary>
		/// Renames a specific file.
		/// </summary>
		/// <param name="objId">The object containing the file to rename.</param>
		/// <param name="fileVer">The file to rename.</param>
		/// <param name="newFileName">The (new) file name.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The updated object version.</returns>
		public ObjectVersion RenameFile(ObjID objId,
			FileVer fileVer,
			string newFileName,
			CancellationToken token = default)
		{
			// Sanity.
			if (null == objId)
				throw new ArgumentNullException(nameof(objId));
			if (null == fileVer)
				throw new ArgumentNullException(nameof(fileVer));

			// Call the other overload.
			return this.RenameFileAsync(objId.Type, objId.ID, fileVer.ID, newFileName, null, fileVer.Version, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();

		}

		/// <summary>
		/// Renames a specific file.
		/// </summary>
		/// <param name="objVer">The object containing the file to rename.</param>
		/// <param name="fileVer">The file to rename.</param>
		/// <param name="newFileName">The (new) file name.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The updated object version.</returns>
		public ObjectVersion RenameFile(ObjVer objVer,
			FileVer fileVer,
			string newFileName,
			CancellationToken token = default)
		{
			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));
			if (null == fileVer)
				throw new ArgumentNullException(nameof(fileVer));

			// Call the other overload.
			return this.RenameFileAsync(objVer.Type, objVer.ID, fileVer.ID, newFileName, objVer.Version, fileVer.Version, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();

		}

		#endregion

		#region Uploading files in chunks

		#region Start the temporary file upload

		/// <summary>
		/// Begins the upload of a file to a temporary location.
		/// </summary>
		/// <param name="fileInfo">The file to upload.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public Task<UploadInfo> UploadTemporaryFileBlockBeginAsync(FileInfo fileInfo, CancellationToken token = default)
		{
			// Sanity.
			if (null == fileInfo)
				throw new ArgumentNullException(nameof(fileInfo));
			if (false == fileInfo.Exists)
				throw new ArgumentException("The file must exist.", nameof(fileInfo));

			// Add the request body.
			var uploadInfo = new UploadInfo()
			{
				Extension = fileInfo.Extension.StartsWith(".")
					? fileInfo.Extension.Substring(1) // Remove the dot.
					: fileInfo.Extension,
				Size = fileInfo.Length,
				Title = fileInfo.Name
			};

			// Use the other overload.
			return UploadTemporaryFileBlockBeginAsync(uploadInfo, token);

		}

		/// <summary>
		/// Begins the upload of a file to a temporary location.
		/// </summary>
		/// <param name="fileInfo">The file to upload.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public async Task<UploadInfo> UploadTemporaryFileBlockBeginAsync(UploadInfo uploadInfo, CancellationToken token = default)
		{
			// Sanity.
			if(null == uploadInfo)
				throw new ArgumentNullException(nameof(uploadInfo));

			// Create the request.
			var request = new RestRequest("/REST/files/upload.aspx");
			request.AddJsonBody(uploadInfo);

			// Ask for a file reference instead of an upload ID if the server supports it. The response then has
			// the FileRef, MaxBlockSize and RequireSequentialBlocks; upload the blocks with UploadStatelessFileBlockAsync.
			await this.MFWSClient.AddStatelessUploadParameterIfSupportedAsync(request, token)
				.ConfigureAwait(false);

			// Execute the request.
			var response = await this.MFWSClient.Post<UploadInfo>(request, token)
				.ConfigureAwait(false);

			// Return the deserialised response data.
			return response.Data;

		}

		/// <summary>
		/// Begins the upload of a file to a temporary location.
		/// </summary>
		/// <param name="fileInfo">The file to upload.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public UploadInfo UploadTemporaryFileBlockBegin(FileInfo fileInfo, CancellationToken token = default)
		{
			// Execute the async method.
			return this.UploadTemporaryFileBlockBeginAsync(fileInfo, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();

		}

		#endregion

		#region Upload blocks of data to an existing temporary file

		/// <summary>
		/// Uploads a block of data to a temporary file.
		/// </summary>
		/// <param name="uploadId">The ID of the temporary upload returned from <see cref="UploadTemporaryFileBlockBeginAsync"/>.</param>
		/// <param name="totalSizeInBytes">The total size of the file (in bytes).</param>
		/// <param name="offset">The offset (in bytes) to start writing this chunk.</param>
		/// <param name="block">The binary data for the chunk.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public async Task<UploadInfo> UploadTemporaryFileBlockAsync(int uploadId, long totalSizeInBytes, long offset, byte[] block,
			CancellationToken token = default)
		{
			// Create the request.
			var request = new RestRequest($"/REST/files/upload/{uploadId}.aspx");

			// Calculate the block data.
			var chunkSize = block.Length; // All chunks must be the same size, aside from the last one.
			var resumableChunkNumber = offset / chunkSize + 1; // Chunks start at 1.
			var totalChunks = (totalSizeInBytes / chunkSize) + 1; // Chunks start at 1.

			// Add the binary data to the request body.
			request.AddParameter("application/octet-stream", block, ParameterType.RequestBody);

			// Add the querystring arguments.
			request.AddQueryParameter("resumableChunkNumber", resumableChunkNumber.ToString());
			request.AddQueryParameter("resumableTotalChunks", totalChunks.ToString());
			if (resumableChunkNumber > 0)
			{
				request.AddQueryParameter("resumableChunkSize", chunkSize.ToString());
			}

			// Execute the request.
			var response = await this.MFWSClient.Post<UploadInfo>(request, token)
				.ConfigureAwait(false);

			// Return the deserialised response data.
			return response.Data;

		}

		/// <summary>
		/// Uploads a block of data to a temporary file.
		/// </summary>
		/// <param name="uploadId">The ID of the temporary upload returned from <see cref="UploadTemporaryFileBlockBeginAsync"/>.</param>
		/// <param name="totalSizeInBytes">The total size of the file (in bytes).</param>
		/// <param name="offset">The offset (in bytes) to start writing this chunk.</param>
		/// <param name="block">The binary data for the chunk.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public UploadInfo UploadTemporaryFileBlock(int uploadId, long totalSizeInBytes, long offset, byte[] block,
			CancellationToken token = default)
		{
			// Execute the async method.
			return this.UploadTemporaryFileBlockAsync(uploadId, totalSizeInBytes, offset, block, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();

		}

		/// <summary>
		/// Uploads a block of a stateless upload started with <see cref="UploadTemporaryFileBlockBeginAsync(UploadInfo, CancellationToken)"/>.
		/// The last block ends the upload.
		/// </summary>
		/// <param name="fileRef">The file reference returned when the upload was started (<see cref="UploadInfo.FileRef"/>).</param>
		/// <param name="blockIndex">The zero-based index of the block.</param>
		/// <param name="blockCount">The number of blocks in the upload.</param>
		/// <param name="blockSize">The size of every block but the last, at most <see cref="UploadInfo.MaxBlockSize"/>.</param>
		/// <param name="block">The data of the block.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>
		/// Every block but the last must be of the same size. If <see cref="UploadInfo.RequireSequentialBlocks"/> was returned,
		/// upload the blocks in order, one at a time. Committing the file reference requires the MD5 checksum of the whole content
		/// (<see cref="UploadInfo.Checksum"/>), which the caller calculates.
		/// </remarks>
		public async Task<UploadInfo> UploadStatelessFileBlockAsync(string fileRef, long blockIndex, long blockCount, long blockSize, byte[] block,
			CancellationToken token = default)
		{
			// Sanity.
			if (string.IsNullOrEmpty(fileRef))
				throw new ArgumentException("The file reference must be provided.", nameof(fileRef));
			if (null == block)
				throw new ArgumentNullException(nameof(block));
			if (blockIndex < 0 || blockIndex >= blockCount)
				throw new ArgumentOutOfRangeException(nameof(blockIndex), "The block index must be between zero and the number of the blocks.");
			if (blockSize <= 0)
				throw new ArgumentOutOfRangeException(nameof(blockSize), "The block size must be greater than zero.");

			// Create the request.
			var request = new RestRequest($"/REST/files/fileref/{Uri.EscapeDataString(fileRef)}/upload.aspx");

			// Add the binary data to the request body.
			request.AddParameter("application/octet-stream", block, ParameterType.RequestBody);

			// Identify the block. The chunk numbers start from one.
			request.AddQueryParameter("resumableChunkNumber", (blockIndex + 1).ToString(CultureInfo.InvariantCulture));
			request.AddQueryParameter("resumableTotalChunks", blockCount.ToString(CultureInfo.InvariantCulture));
			request.AddQueryParameter("resumableChunkSize", blockSize.ToString(CultureInfo.InvariantCulture));

			// Execute the request.
			var response = await this.MFWSClient.Post<UploadInfo>(request, token)
				.ConfigureAwait(false);

			// Return the deserialised response data.
			return response.Data;
		}

		/// <summary>
		/// Uploads a block of a stateless upload started with <see cref="UploadTemporaryFileBlockBegin(FileInfo, CancellationToken)"/>.
		/// The last block ends the upload.
		/// </summary>
		/// <param name="fileRef">The file reference returned when the upload was started (<see cref="UploadInfo.FileRef"/>).</param>
		/// <param name="blockIndex">The zero-based index of the block.</param>
		/// <param name="blockCount">The number of blocks in the upload.</param>
		/// <param name="blockSize">The size of every block but the last, at most <see cref="UploadInfo.MaxBlockSize"/>.</param>
		/// <param name="block">The data of the block.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		public UploadInfo UploadStatelessFileBlock(string fileRef, long blockIndex, long blockCount, long blockSize, byte[] block,
			CancellationToken token = default)
		{
			// Execute the async method.
			return this.UploadStatelessFileBlockAsync(fileRef, blockIndex, blockCount, blockSize, block, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region Complete/commit temporary file

		/// <summary>
		/// Marks the temporary file as committed so that it can be used from other processes.
		/// </summary>
		/// <param name="uploadInfo">The upload to commit.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public async Task<UploadInfo> UploadTemporaryFileCommitAsync(UploadInfo uploadInfo, CancellationToken token = default)
		{
			// Sanity.
			if (null == uploadInfo)
				throw new ArgumentNullException(nameof(uploadInfo));

			// A stateless upload is ended by its last block, and any server instance can use it. Nothing to commit.
			if (uploadInfo.IsStateless())
				return uploadInfo;

			if (0 >= uploadInfo.Size)
				throw new ArgumentException("The file must have a size.", nameof(uploadInfo));

			// Create the request.
#pragma warning disable CS0618 // Upload IDs are obsolete; servers without the stateless upload still need them.
			var request = new RestRequest($"/REST/files/{uploadInfo.UploadID}/{uploadInfo.Size}/uploadtemporarycommit.aspx");
#pragma warning restore CS0618

			// Execute the request.
			var response = await this.MFWSClient.Post<int>(request, token)
				.ConfigureAwait(false);

			// Return the deserialised response data.
			return uploadInfo;

		}

		/// <summary>
		/// Marks the temporary file as committed so that it can be used from other processes.
		/// </summary>
		/// <param name="uploadInfo">The upload to commit.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public UploadInfo UploadTemporaryFileCommit(UploadInfo uploadInfo, CancellationToken token = default)
		{
			// Execute the async method.
			return this.UploadTemporaryFileCommitAsync(uploadInfo, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();

		}

		#endregion

		#region Helper for uploading files in blocks

		/// <summary>
		/// Uploads a temporary file to M-Files using blocks of the given size.
		/// </summary>
		/// <param name="newFileName">The new file name, e.g. "hello.docx"</param>
		/// <param name="fileToUpload">The file to upload.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <param name="blockSize">The chunk size (bytes)to upload in.  Defaults to 2MB.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public async Task<UploadInfo> UploadTemporaryFileInBlocksAsync(
			string newFileName,
			Stream fileToUpload,
			CancellationToken token = default,
			int blockSize = 1024 * 1024 * 2)
		{
			// Sanity.
			if (null == fileToUpload)
				throw new ArgumentNullException(nameof(fileToUpload));
			if (null == fileToUpload)
				throw new ArgumentNullException("The file must exist.", nameof(fileToUpload));
			if (false == fileToUpload.CanRead)
				throw new ArgumentException("The stream cannot be read.", nameof(fileToUpload));
			if (fileToUpload.Length == 0)
				throw new ArgumentException("The stream length cannot be zero.", nameof(fileToUpload));
			if (blockSize <= 0)
				throw new ArgumentOutOfRangeException(nameof(blockSize), "The block size must be greater than zero.");

			// Start the upload.
			var uploadInfo = new UploadInfo()
			{
				Title = newFileName.Contains(".")
					? newFileName.Substring(0, newFileName.LastIndexOf("."))
					: newFileName,
				Extension = newFileName.Contains(".")
					? newFileName.Substring(newFileName.LastIndexOf("."))
					: "",
				Size = fileToUpload.Length
			};
			uploadInfo = await this.UploadTemporaryFileBlockBeginAsync(uploadInfo, token);

			// A stateless upload is identified by its file reference instead of an upload ID.
			if (uploadInfo.IsStateless())
				return await this.UploadStatelessFileInBlocksAsync(uploadInfo, fileToUpload, blockSize, token)
					.ConfigureAwait(false);

			// Iterate over the file blocks and upload them.
			long offset = 0;
			long totalSizeInBytes = fileToUpload.Length;
			if (blockSize > totalSizeInBytes)
				blockSize = (int)totalSizeInBytes;

			var buffer = new byte[blockSize];
			do
			{
				// Read the data from the stream.
				var bytesRead = await fileToUpload.ReadAsync(buffer, 0, blockSize, token);

				// Upload the block.
#pragma warning disable CS0618 // Upload IDs are obsolete; servers without the stateless upload still need them.
				var ui = await this.UploadTemporaryFileBlockAsync(uploadInfo.UploadID, totalSizeInBytes, offset, buffer, token);
#pragma warning restore CS0618
				uploadInfo = ui;

				// Move onwards.
				offset += bytesRead;
			} while (offset < totalSizeInBytes);

			// Commit the file.
			// FILE SHOULD BE COMMITTED AS PART OF THE OBJECT CREATION.
			//await this.UploadTemporaryFileCommitAsync(uploadInfo, token);

			// Return the upload information.
			return uploadInfo;

		}

		/// <summary>
		/// Uploads the content of a started stateless upload in blocks and sets the checksum of the content.
		/// </summary>
		/// <param name="uploadInfo">The started upload, with its file reference.</param>
		/// <param name="fileToUpload">The content.</param>
		/// <param name="blockSize">The requested block size (bytes).</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The upload information with the checksum of the content.</returns>
		private async Task<UploadInfo> UploadStatelessFileInBlocksAsync(
			UploadInfo uploadInfo,
			Stream fileToUpload,
			int blockSize,
			CancellationToken token)
		{
			// Every block but the last must be of the same size, at most the size the server allows.
			long totalSizeInBytes = fileToUpload.Length;
			long size = blockSize;
			if (uploadInfo.MaxBlockSize.HasValue && uploadInfo.MaxBlockSize.Value > 0 && uploadInfo.MaxBlockSize.Value < size)
				size = uploadInfo.MaxBlockSize.Value;
			long blockCount = (totalSizeInBytes + size - 1) / size;

			// Upload the blocks in order, one at a time, and calculate the checksum of the content
			// while reading them. Committing the file reference requires the checksum.
			using (var md5 = MD5.Create())
			{
				for (long blockIndex = 0; blockIndex < blockCount; blockIndex++)
				{
					// Read the block. Only the last block may be shorter.
					var block = new byte[(int)Math.Min(size, totalSizeInBytes - blockIndex * size)];
					await ReadExactlyAsync(fileToUpload, block, block.Length, token)
						.ConfigureAwait(false);
					md5.TransformBlock(block, 0, block.Length, null, 0);

					// Upload the block.
					await this.UploadStatelessFileBlockAsync(uploadInfo.FileRef, blockIndex, blockCount, size, block, token)
						.ConfigureAwait(false);
				}
				md5.TransformFinalBlock(new byte[0], 0, 0);

				// The server expects 32 hexadecimal characters.
				uploadInfo.Checksum = BitConverter.ToString(md5.Hash).Replace("-", "").ToLowerInvariant();
			}

			// Return the upload information.
			return uploadInfo;
		}

		/// <summary>
		/// Reads exactly the requested number of bytes from a stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="buffer">The buffer to read into, from its start.</param>
		/// <param name="count">The number of bytes to read.</param>
		/// <param name="token">A cancellation token.</param>
		private static async Task ReadExactlyAsync(Stream stream, byte[] buffer, int count, CancellationToken token)
		{
			var read = 0;
			while (read < count)
			{
				var readNow = await stream.ReadAsync(buffer, read, count - read, token)
					.ConfigureAwait(false);
				if (readNow == 0)
					throw new EndOfStreamException("The stream ended before its length.");
				read += readNow;
			}
		}

		/// <summary>
		/// Uploads a temporary file to M-Files using blocks of the given size.
		/// </summary>
		/// <param name="fileToUpload">The file to upload.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <param name="blockSize">The chunk size (bytes)to upload in.  Defaults to 2MB.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public async Task<UploadInfo> UploadTemporaryFileInBlocksAsync(
			FileInfo fileToUpload,
            CancellationToken token = default,
			int blockSize = 1024 * 1024 * 2)
		{
			// Sanity.
			if (null == fileToUpload)
				throw new ArgumentNullException(nameof(fileToUpload));
			if (false == fileToUpload.Exists)
				throw new ArgumentException("The file must exist.", nameof(fileToUpload));
			if(blockSize <= 0)
				throw new ArgumentOutOfRangeException(nameof(blockSize), "The block size must be greater than zero.");

			// Use the other overload.
			using (var stream = fileToUpload.OpenRead())
			{
				return await this.UploadTemporaryFileInBlocksAsync(fileToUpload.Name, stream, token, blockSize);
			}

		}

		/// <summary>
		/// Uploads a temporary file to M-Files using blocks of the given size.
		/// </summary>
		/// <param name="fileToUpload">The file to upload.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <param name="blockSize">The chunk size (bytes)to upload in.  Defaults to 2MB.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public UploadInfo UploadTemporaryFileInBlocks(
			FileInfo fileToUpload,
			CancellationToken token = default,
			int blockSize = 1024 * 1024 * 2)
		{
			// Execute the async method.
			return this.UploadTemporaryFileInBlocksAsync(fileToUpload, token, blockSize)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Uploads a temporary file to M-Files using blocks of the given size.
		/// </summary>
		/// <param name="fileToUpload">The file to upload.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <param name="blockSize">The chunk size (bytes)to upload in.  Defaults to 2MB.</param>
		/// <returns>Information on the upload.</returns>
		/// <remarks>THIS METHOD IS NOT DOCUMENTED SO DO NOT USE IN PRODUCTION ENVIRONMENTS.</remarks>
		public UploadInfo UploadTemporaryFileInBlocks(
			string newFileName,
			Stream fileToUpload,
			CancellationToken token = default,
			int blockSize = 1024 * 1024 * 2)
		{
			// Execute the async method.
			return this.UploadTemporaryFileInBlocksAsync(newFileName, fileToUpload, token, blockSize)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#endregion

		#region Removing files
		public ExtendedObjectVersion RemoveFile(ObjVer objVer, FileVer fileVer, CancellationToken token = default)
        {
			return this.RemoveFileAsync(objVer, fileVer)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}
		public async Task<ExtendedObjectVersion> RemoveFileAsync(ObjVer objVer, FileVer fileVer, CancellationToken token = default)
		{
			// Sanity.
			if (null == objVer)
				throw new ArgumentNullException(nameof(objVer));
			if (null == fileVer)
				throw new ArgumentNullException(nameof(fileVer));

			// Extract the URI elements.
			int objectTypeId;
			string objectId, objectVersionId;
			objVer.GetUriParameters(out objectTypeId, out objectId, out objectVersionId);
			string fileId, fileVersionId;
			fileVer.GetUriParameters(out fileId, out fileVersionId);

			// Build up the request.
			var request = new RestRequest($"/REST/objects/{objectTypeId}/{objectId}/{objectVersionId}/files/{fileId}");
			request.Method = Method.Delete;

			// Execute the request.
			var response = await this.MFWSClient.Delete<ExtendedObjectVersion>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;

		}
			#endregion
	}
}
