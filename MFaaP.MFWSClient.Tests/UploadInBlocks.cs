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
	/// Tests for uploading temporary files in blocks identified by upload IDs.
	/// </summary>
	[TestClass]
	public class UploadInBlocks
	{
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
		/// Creates a client which records the upload requests and returns the given upload ID.
		/// </summary>
		private static MFaaP.MFWSClient.MFWSClient CreateClient(List<RestRequest> requests, int uploadId = 7)
		{
			var mock = new Mock<IRestClient>();
			mock
				.Setup(c => c.ExecuteAsync<UploadInfo>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => requests.Add(r))
				.Returns((RestRequest r, CancellationToken t) => Task.FromResult(new RestResponse<UploadInfo>(r)
				{
					Data = new UploadInfo() { UploadID = uploadId },
					StatusCode = HttpStatusCode.OK,
					ResponseUri = new Uri(r.Resource, UriKind.Relative)
				}));
			return MFWSClient.GetMFWSClient(mock);
		}

		/// <summary>
		/// Uploads the content in blocks and checks the block requests.
		/// </summary>
		private static async Task AssertUploadedInBlocks(int contentLength, int blockSize, int[] expectedBlockSizes)
		{
			var content = Enumerable.Range(0, contentLength).Select(i => (byte)(i + 1)).ToArray();
			var requests = new List<RestRequest>();
			var client = CreateClient(requests);

			// Execute.
			using (var stream = new MemoryStream(content))
			{
				await client.ObjectFileOperations.UploadTemporaryFileInBlocksAsync("test.txt", stream, blockSize: blockSize);
			}

			// The upload is started, then the blocks are uploaded.
			Assert.AreEqual(expectedBlockSizes.Length + 1, requests.Count);
			Assert.AreEqual("/REST/files/upload.aspx", requests[0].Resource);
			var offset = 0;
			for (var i = 0; i < expectedBlockSizes.Length; i++)
			{
				var request = requests[i + 1];
				Assert.AreEqual("/REST/files/upload/7.aspx", request.Resource);
				Assert.AreEqual((i + 1).ToString(), GetQueryParameter(request, "resumableChunkNumber"));
				Assert.AreEqual(expectedBlockSizes.Length.ToString(), GetQueryParameter(request, "resumableTotalChunks"));
				// A file smaller than the block size is one block of its own size.
				Assert.AreEqual(Math.Min(blockSize, contentLength).ToString(), GetQueryParameter(request, "resumableChunkSize"));

				// The block contains only its own data.
				var body = (byte[])request.Parameters.First(p => p.Type == ParameterType.RequestBody).Value;
				CollectionAssert.AreEqual(content.Skip(offset).Take(expectedBlockSizes[i]).ToArray(), body);
				offset += expectedBlockSizes[i];
			}
		}

		/// <summary>
		/// Ensures that the last block is sent with only its own data, not the whole buffer.
		/// </summary>
		[TestMethod]
		public async Task UploadTemporaryFileInBlocksAsync_ShortLastBlock()
		{
			await AssertUploadedInBlocks(10, 4, new[] { 4, 4, 2 });
		}

		/// <summary>
		/// Ensures that the number of blocks is correct when the size divides evenly by the block size.
		/// </summary>
		[TestMethod]
		public async Task UploadTemporaryFileInBlocksAsync_EvenlyDivisible()
		{
			await AssertUploadedInBlocks(8, 4, new[] { 4, 4 });
		}

		/// <summary>
		/// Ensures that a file smaller than the block size is uploaded in one block.
		/// </summary>
		[TestMethod]
		public async Task UploadTemporaryFileInBlocksAsync_SmallerThanBlock()
		{
			await AssertUploadedInBlocks(3, 4, new[] { 3 });
		}

		/// <summary>
		/// Ensures that the file name is split into a title and an extension without the dot, which are sent when the
		/// upload is started and returned with the upload ID and size.
		/// </summary>
		private static async Task AssertFileNameSplit(string fileName, string expectedTitle, string expectedExtension)
		{
			var requests = new List<RestRequest>();
			var client = CreateClient(requests);

			// Execute.
			UploadInfo upload;
			using (var stream = new MemoryStream(new byte[10]))
			{
				upload = await client.ObjectFileOperations.UploadTemporaryFileInBlocksAsync(fileName, stream, blockSize: 4);
			}

			// The start sends the title and the extension without the dot.
			var started = (UploadInfo)requests[0].Parameters.First(p => p.Type == ParameterType.RequestBody).Value;
			Assert.AreEqual(expectedTitle, started.Title);
			Assert.AreEqual(expectedExtension, started.Extension);

			// The result can be used to create an object: it has the upload ID, the file name and the size.
			Assert.AreEqual(7, upload.UploadID);
			Assert.AreEqual(expectedTitle, upload.Title);
			Assert.AreEqual(expectedExtension, upload.Extension);
			Assert.AreEqual(10, upload.Size);
		}

		/// <summary>
		/// Ensures that <see cref="MFaaP.MFWSClient.MFWSVaultObjectFileOperations.UploadTemporaryFileBlockBeginAsync(FileInfo, CancellationToken)"/>
		/// sends the extension without the dot, also for a file without an extension.
		/// </summary>
		private static async Task AssertFileInfoExtension(string fileName, string expectedExtension)
		{
			var requests = new List<RestRequest>();
			var client = CreateClient(requests);
			var directory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
			try
			{
				var file = new FileInfo(Path.Combine(directory.FullName, fileName));
				File.WriteAllBytes(file.FullName, new byte[3]);

				// Execute.
				await client.ObjectFileOperations.UploadTemporaryFileBlockBeginAsync(file);

				// Verify.
				var started = (UploadInfo)requests[0].Parameters.First(p => p.Type == ParameterType.RequestBody).Value;
				Assert.AreEqual(expectedExtension, started.Extension);
			}
			finally
			{
				directory.Delete(true);
			}
		}

		[TestMethod]
		public async Task UploadTemporaryFileInBlocksAsync_FileName_WithExtension()
		{
			await AssertFileNameSplit("report.pdf", "report", "pdf");
		}

		[TestMethod]
		public async Task UploadTemporaryFileInBlocksAsync_FileName_SeveralDots()
		{
			await AssertFileNameSplit("archive.tar.gz", "archive.tar", "gz");
		}

		[TestMethod]
		public async Task UploadTemporaryFileInBlocksAsync_FileName_NoExtension()
		{
			await AssertFileNameSplit("README", "README", "");
		}

		[TestMethod]
		public async Task UploadTemporaryFileInBlocksAsync_FileName_OnlyExtension()
		{
			await AssertFileNameSplit(".gitignore", "", "gitignore");
		}

		[TestMethod]
		public async Task UploadTemporaryFileBlockBeginAsync_FileInfo_WithExtension()
		{
			await AssertFileInfoExtension("upload.txt", "txt");
		}

		[TestMethod]
		public async Task UploadTemporaryFileBlockBeginAsync_FileInfo_NoExtension()
		{
			await AssertFileInfoExtension("upload", "");
		}

		/// <summary>
		/// Ensures that the overload that takes the block size from the block counts the blocks correctly
		/// when the size divides evenly by the block size.
		/// </summary>
		[TestMethod]
		public async Task UploadTemporaryFileBlockAsync_BlockSizeFromBlock()
		{
			var requests = new List<RestRequest>();
			var client = CreateClient(requests);

			// Execute.
			await client.ObjectFileOperations.UploadTemporaryFileBlockAsync(7, 8, 4, new byte[4]);

			// Verify.
			Assert.AreEqual(1, requests.Count);
			Assert.AreEqual("2", GetQueryParameter(requests[0], "resumableChunkNumber"));
			Assert.AreEqual("2", GetQueryParameter(requests[0], "resumableTotalChunks"));
			Assert.AreEqual("4", GetQueryParameter(requests[0], "resumableChunkSize"));
		}

		/// <summary>
		/// Ensures that an offset which is not at a block boundary is rejected.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task UploadTemporaryFileBlockAsync_OffsetNotAtBlockBoundary_Throws()
		{
			var client = CreateClient(new List<RestRequest>());
			await client.ObjectFileOperations.UploadTemporaryFileBlockAsync(7, 10, 6, new byte[4], 4);
		}

		/// <summary>
		/// Ensures that a block larger than the block size is rejected.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task UploadTemporaryFileBlockAsync_BlockLargerThanBlockSize_Throws()
		{
			var client = CreateClient(new List<RestRequest>());
			await client.ObjectFileOperations.UploadTemporaryFileBlockAsync(7, 10, 0, new byte[5], 4);
		}
	}
}
