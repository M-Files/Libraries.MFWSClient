using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace MFaaP.MFWSClient.Tests
{
	/// <summary>
	/// Tests for the stateless file upload, which identifies the uploads by file references instead of upload IDs.
	/// </summary>
	[TestClass]
	public class StatelessFileUpload
	{
		/// <summary>
		/// Creates a temporary file with the given content.
		/// </summary>
		private static FileInfo CreateTemporaryFile(byte[] content)
		{
			var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".txt");
			File.WriteAllBytes(path, content);
			return new FileInfo(path);
		}

		/// <summary>
		/// Deletes a temporary file if possible. The request of a mocked client may leave the file open.
		/// </summary>
		private static void DeleteTemporaryFile(FileInfo file)
		{
			try
			{
				file.Delete();
			}
			catch (IOException)
			{
			}
		}

		/// <summary>
		/// Gets the value of a querystring parameter of the request, or null.
		/// </summary>
		private static string GetQueryParameter(RestRequest request, string name)
		{
			return request.Parameters
				.FirstOrDefault(p => p.Type == ParameterType.QueryString && p.Name == name)?
				.Value as string;
		}

		/// <summary>
		/// Sets up the capabilities request of the mocked client to return the given capabilities.
		/// </summary>
		private static void SetUpCapabilities(Mock<IRestClient> mock, ServerVaultCapabilities capabilities)
		{
			mock
				.Setup(c => c.ExecuteAsync<ServerVaultCapabilities>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Returns((RestRequest r, CancellationToken t) => Task.FromResult(new RestResponse<ServerVaultCapabilities>(r)
				{
					Data = capabilities,
					StatusCode = HttpStatusCode.OK,
					ResponseUri = new Uri(r.Resource, UriKind.Relative)
				}));
		}

		/// <summary>
		/// Creates a client whose capabilities request returns the given capabilities, and which records the upload requests.
		/// </summary>
		private static MFaaP.MFWSClient.MFWSClient CreateClientWithCapabilities(
			Mock<IRestClient> mock,
			ServerVaultCapabilities capabilities,
			List<RestRequest> uploadRequests)
		{
			SetUpCapabilities(mock, capabilities);
			mock
				.Setup(c => c.ExecuteAsync<UploadInfo>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => uploadRequests.Add(r))
				.Returns((RestRequest r, CancellationToken t) => Task.FromResult(new RestResponse<UploadInfo>(r)
				{
					Data = new UploadInfo() { FileRef = "ref" },
					StatusCode = HttpStatusCode.OK,
					ResponseUri = new Uri(r.Resource, UriKind.Relative)
				}));
			return MFWSClient.GetMFWSClient(mock);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFWSClientBase.GetServerCapabilitiesAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetServerCapabilitiesAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ServerVaultCapabilities>(Method.Get, "/REST/server/capabilities");
			runner.ResponseData = new ServerVaultCapabilities() { FileUploadStatelessSupported = true };

			// Execute.
			var capabilities = await runner.MFWSClient.GetServerCapabilitiesAsync();

			// Verify.
			runner.Verify();
			Assert.IsTrue(capabilities.FileUploadStatelessSupported);
		}

		/// <summary>
		/// Ensures that <see cref="MFaaP.MFWSClient.MFWSVaultObjectFileOperations.UploadFilesAsync(FileInfo[])"/>
		/// asks for file references when the server supports the stateless upload, and keeps the returned file reference.
		/// </summary>
		[TestMethod]
		public async Task UploadFilesAsync_Supported_AsksForFileReferences()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<UploadInfo>(Method.Post, "/REST/files.aspx?statelessUpload=true");
			runner.ResponseData = new UploadInfo()
			{
				FileRef = "ref"
			};
			SetUpCapabilities(runner.RestClientMock, new ServerVaultCapabilities() { FileUploadStatelessSupported = true });
			var file = CreateTemporaryFile(new byte[] { 1, 2, 3 });
			try
			{
				// Execute.
				var uploads = await runner.MFWSClient.ObjectFileOperations.UploadFilesAsync(file);

				// Verify.
				runner.Verify();
				Assert.AreEqual(1, uploads.Length);
				Assert.IsTrue(uploads[0].IsStateless());
				Assert.AreEqual(0, uploads[0].UploadID);
				Assert.AreEqual("ref", uploads[0].FileRef);
				Assert.AreEqual(3, uploads[0].Size);
			}
			finally
			{
				DeleteTemporaryFile(file);
			}
		}

		/// <summary>
		/// Ensures that <see cref="MFaaP.MFWSClient.MFWSVaultObjectFileOperations.UploadFileAsync"/>
		/// asks for a stateless upload when the server supports the stateless upload.
		/// </summary>
		[TestMethod]
		public async Task UploadFileAsync_Supported_AsksForStatelessUpload()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Put, "/REST/objects/456/123/9/files/123/content.aspx?statelessUpload=true");
			SetUpCapabilities(runner.RestClientMock, new ServerVaultCapabilities() { FileUploadStatelessSupported = true });
			var file = CreateTemporaryFile(new byte[] { 1, 2, 3 });
			try
			{
				// Execute.
				await runner.MFWSClient.ObjectFileOperations.UploadFileAsync(
					new ObjVer() { Type = 456, ID = 123, Version = 9 },
					new FileVer() { ID = 123 },
					file.FullName);

				// Verify.
				runner.Verify();
			}
			finally
			{
				DeleteTemporaryFile(file);
			}
		}

		/// <summary>
		/// Ensures that uploads ask for file references when the server
		/// advertises the capability, and retrieves the capabilities only once.
		/// </summary>
		[TestMethod]
		public async Task Capability_Supported_AsksForFileReferences()
		{
			var mock = new Mock<IRestClient>();
			var uploadRequests = new List<RestRequest>();
			var client = CreateClientWithCapabilities(mock,
				new ServerVaultCapabilities() { FileUploadStatelessSupported = true }, uploadRequests);

			// Execute twice.
			await client.ObjectFileOperations.UploadTemporaryFileBlockBeginAsync(new UploadInfo() { Size = 10 });
			await client.ObjectFileOperations.UploadTemporaryFileBlockBeginAsync(new UploadInfo() { Size = 10 });

			// Verify.
			mock.Verify(c => c.ExecuteAsync<ServerVaultCapabilities>(
				It.Is<RestRequest>(r => r.Resource == "/REST/server/capabilities" && r.Method == Method.Get),
				It.IsAny<CancellationToken>()), Times.Once);
			Assert.AreEqual(2, uploadRequests.Count);
			foreach (var request in uploadRequests)
				Assert.AreEqual("true", GetQueryParameter(request, "statelessUpload"));
		}

		/// <summary>
		/// Ensures that uploads use upload IDs when the server
		/// does not advertise the capability.
		/// </summary>
		[TestMethod]
		public async Task Capability_NotSupported_UsesUploadIds()
		{
			var mock = new Mock<IRestClient>();
			var uploadRequests = new List<RestRequest>();
			var client = CreateClientWithCapabilities(mock,
				new ServerVaultCapabilities() { FileUploadStatelessSupported = false }, uploadRequests);

			// Execute.
			await client.ObjectFileOperations.UploadTemporaryFileBlockBeginAsync(new UploadInfo() { Size = 10 });

			// Verify.
			Assert.AreEqual(1, uploadRequests.Count);
			Assert.IsNull(GetQueryParameter(uploadRequests[0], "statelessUpload"));
		}

		/// <summary>
		/// Ensures that uploads use upload IDs when the server
		/// returns no capabilities, e.g. an older server without the endpoint.
		/// </summary>
		[TestMethod]
		public async Task Capability_NoCapabilities_UsesUploadIds()
		{
			var mock = new Mock<IRestClient>();
			var uploadRequests = new List<RestRequest>();
			var client = CreateClientWithCapabilities(mock, null, uploadRequests);

			// Execute.
			await client.ObjectFileOperations.UploadTemporaryFileBlockBeginAsync(new UploadInfo() { Size = 10 });

			// Verify.
			Assert.AreEqual(1, uploadRequests.Count);
			Assert.IsNull(GetQueryParameter(uploadRequests[0], "statelessUpload"));
		}

		/// <summary>
		/// Ensures that <see cref="MFaaP.MFWSClient.MFWSVaultObjectFileOperations.UploadTemporaryFileInBlocksAsync(string, Stream, CancellationToken, int)"/>
		/// uploads a stateless upload in blocks of the same size, at most the maximum block size.
		/// </summary>
		[TestMethod]
		public async Task UploadTemporaryFileInBlocksAsync_Stateless()
		{
			var content = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
			var requests = new List<RestRequest>();
			var mock = new Mock<IRestClient>();
			mock
				.Setup(c => c.ExecuteAsync<UploadInfo>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => requests.Add(r))
				.Returns((RestRequest r, CancellationToken t) => Task.FromResult(new RestResponse<UploadInfo>(r)
				{
					// The start returns the file reference and the maximum block size.
					Data = requests.Count == 1
						? new UploadInfo() { FileRef = "a/b", MaxBlockSize = 4, RequireSequentialBlocks = true, Size = content.Length }
						: new UploadInfo() { FileRef = "a/b" },
					StatusCode = HttpStatusCode.OK,
					ResponseUri = new Uri(r.Resource, UriKind.Relative)
				}));
			var client = MFWSClient.GetMFWSClient(mock);
			SetUpCapabilities(mock, new ServerVaultCapabilities() { FileUploadStatelessSupported = true });

			// Execute.
			UploadInfo upload;
			using (var stream = new MemoryStream(content))
			{
				upload = await client.ObjectFileOperations.UploadTemporaryFileInBlocksAsync("test.txt", stream, blockSize: 8);
			}

			// The start asks for a file reference.
			Assert.AreEqual(4, requests.Count);
			Assert.AreEqual("/REST/files/upload.aspx", requests[0].Resource);
			Assert.AreEqual("true", GetQueryParameter(requests[0], "statelessUpload"));

			// The blocks are of the maximum block size, except the last.
			var expectedBlockSizes = new[] { 4, 4, 2 };
			for (var i = 0; i < 3; i++)
			{
				var request = requests[i + 1];
				Assert.AreEqual("/REST/files/fileref/a%2Fb/upload.aspx", request.Resource);
				Assert.AreEqual((i + 1).ToString(), GetQueryParameter(request, "resumableChunkNumber"));
				Assert.AreEqual("3", GetQueryParameter(request, "resumableTotalChunks"));
				Assert.AreEqual("4", GetQueryParameter(request, "resumableChunkSize"));
				var body = (byte[])request.Parameters.First(p => p.Type == ParameterType.RequestBody).Value;
				CollectionAssert.AreEqual(content.Skip(i * 4).Take(expectedBlockSizes[i]).ToArray(), body);
			}

			// The upload has the file reference.
			Assert.AreEqual("a/b", upload.FileRef);
			Assert.AreEqual(content.Length, upload.Size);
		}

		/// <summary>
		/// Ensures that <see cref="MFaaP.MFWSClient.MFWSVaultObjectFileOperations.UploadTemporaryFileCommitAsync"/>
		/// makes no request for a stateless upload, which needs no commit.
		/// </summary>
		[TestMethod]
		public async Task UploadTemporaryFileCommitAsync_Stateless_NoRequest()
		{
			var mock = new Mock<IRestClient>();
			var client = MFWSClient.GetMFWSClient(mock);
			var upload = new UploadInfo() { FileRef = "ref", Size = 10 };

			// Execute.
			var result = await client.ObjectFileOperations.UploadTemporaryFileCommitAsync(upload);

			// Verify.
			Assert.AreSame(upload, result);
			mock.Verify(c => c.ExecuteAsync<int>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		/// <summary>
		/// Ensures that <see cref="MFaaP.MFWSClient.MFWSVaultAutomaticMetadataOperations.GetAutomaticMetadataForUploadsAsync"/>
		/// sends the stateless uploads as uploaded files.
		/// </summary>
		[TestMethod]
		public async Task GetAutomaticMetadataForUploadsAsync_Stateless()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<PropertyValueSuggestion>>(Method.Post, "/REST/objects/automaticmetadata.aspx");
			var upload = new UploadInfo() { FileRef = "ref", Extension = "pdf" };
			runner.SetExpectedRequestBody(new AutomaticMetadataRequestInfo()
			{
				ObjectType = 0,
				UploadedFiles = new List<UploadInfo>() { upload }
			});

			// Execute.
			await runner.MFWSClient.AutomaticMetadataOperations.GetAutomaticMetadataForUploadsAsync(uploads: upload);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that <see cref="MFaaP.MFWSClient.MFWSVaultAutomaticMetadataOperations.GetAutomaticMetadataForUploadsAsync"/>
		/// sends the uploads identified by upload IDs as upload IDs.
		/// </summary>
		[TestMethod]
		public async Task GetAutomaticMetadataForUploadsAsync_UploadIds()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<PropertyValueSuggestion>>(Method.Post, "/REST/objects/automaticmetadata.aspx");
			runner.SetExpectedRequestBody(new AutomaticMetadataRequestInfo()
			{
				ObjectType = 0,
				UploadIds = new List<int>() { 123, 456 }
			});

			// Execute.
			await runner.MFWSClient.AutomaticMetadataOperations.GetAutomaticMetadataForUploadsAsync(
				uploads: new[] { new UploadInfo() { UploadID = 123 }, new UploadInfo() { UploadID = 456 } });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that <see cref="MFaaP.MFWSClient.MFWSVaultAutomaticMetadataOperations.GetAutomaticMetadataForUploadsAsync"/>
		/// rejects mixing stateless uploads with uploads identified by upload IDs.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task GetAutomaticMetadataForUploadsAsync_Mixed_Throws()
		{
			var client = MFWSClient.GetMFWSClient(new Mock<IRestClient>());
			await client.AutomaticMetadataOperations.GetAutomaticMetadataForUploadsAsync(
				uploads: new[] { new UploadInfo() { FileRef = "ref" }, new UploadInfo() { UploadID = 123 } });
		}

		/// <summary>
		/// Ensures that the upload info of a stateless upload returned by MFWS, with a null upload ID, can be deserialized.
		/// </summary>
		[TestMethod]
		public void UploadInfo_StatelessResponse_Deserializes()
		{
			var json = "{\"UploadID\":null,\"FileRef\":\"ref\",\"MaxBlockSize\":4194304,\"RequireSequentialBlocks\":true,"
				+ "\"Title\":\"test\",\"Extension\":\"txt\",\"Size\":10,\"FileInformationType\":0}";
			var serializer = new RestSharp.Serializers.Json.SystemTextJsonSerializer();
			var upload = serializer.Deserialize<UploadInfo>(new RestResponse() { Content = json });
			Assert.AreEqual(0, upload.UploadID);
			Assert.AreEqual("ref", upload.FileRef);
			Assert.AreEqual(4194304, upload.MaxBlockSize);
			Assert.AreEqual(true, upload.RequireSequentialBlocks);
			Assert.AreEqual(10, upload.Size);
			Assert.IsTrue(upload.IsStateless());
		}
	}
}
