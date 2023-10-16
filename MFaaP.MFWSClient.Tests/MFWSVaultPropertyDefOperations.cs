using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MFaaP.MFWSClient.Tests
{
	[TestClass]
	public class MFWSVaultPropertyDefOperations
	{

		#region Property definition alias to ID resolution

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefIDByAliasAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetPropertyDefIDByAliasAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/properties/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello world" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.PropertyDefOperations.GetPropertyDefIDByAliasAsync("hello world");

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefIDsByAliasesAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetPropertyDefIDsByAliasesAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/properties/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello", "world", "third option" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.PropertyDefOperations.GetPropertyDefIDsByAliasesAsync(aliases: new string[] { "hello", "world", "third option" });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefIDsByAliases"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetPropertyDefIDsByAliases()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/properties/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello", "world", "third option" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.PropertyDefOperations.GetPropertyDefIDsByAliases(aliases: new string[] {  "hello", "world", "third option" });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefIDByAlias"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetPropertyDefIDByAlias()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/properties/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello world" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.PropertyDefOperations.GetPropertyDefIDByAlias("hello world");

			// Verify.
			runner.Verify();
		}

		#endregion

		#region GetPropertyDefs

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefsAsync"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task GetPropertyDefsAsync_CorrectResource()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<List<PropertyDef>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<List<PropertyDef>>>
                        (
                            m => m.Data == new List<PropertyDef>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.PropertyDefOperations.GetPropertyDefsAsync();

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<List<PropertyDef>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/structure/properties", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefs"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void GetPropertyDefs_CorrectResource()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<List<PropertyDef>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<List<PropertyDef>>>
                        (
                            m => m.Data == new List<PropertyDef>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.PropertyDefOperations.GetPropertyDefs();

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<List<PropertyDef>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/structure/properties", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefsAsync"/>
		/// uses the correct Http method.
		/// </summary>
		[TestMethod]
		public async Task GetPropertyDefsAsync_CorrectMethod()
		{
			/* Arrange */

			// The method.
			Method? methodUsed = null;

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<List<PropertyDef>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					methodUsed = r.Method;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<List<PropertyDef>>>
                        (
                            m => m.Data == new List<PropertyDef>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.PropertyDefOperations.GetPropertyDefsAsync();

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<List<PropertyDef>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Method must be correct.
			Assert.AreEqual(Method.Get, methodUsed);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefs"/>
		/// uses the correct Http method.
		/// </summary>
		[TestMethod]
		public void GetPropertyDefs_CorrectMethod()
		{
			/* Arrange */

			// The method.
			Method? methodUsed = null;

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<List<PropertyDef>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					methodUsed = r.Method;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<List<PropertyDef>>>
                        (
                            m => m.Data == new List<PropertyDef>()
                        )
                    );
				});

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.PropertyDefOperations.GetPropertyDefs();

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<List<PropertyDef>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Method must be correct.
			Assert.AreEqual(Method.Get, methodUsed);
		}

		#endregion

		#region GetPropertyDef

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefAsync"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task GetPropertyDefAsync_CorrectResource()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<PropertyDef>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<PropertyDef>>
                        (
                            m => m.Data == new PropertyDef()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.PropertyDefOperations.GetPropertyDefAsync(123);

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<PropertyDef>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/structure/properties/123", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDef"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void GetPropertyDef_CorrectResource()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<PropertyDef>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<PropertyDef>>
                        (
                            m => m.Data == new PropertyDef()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.PropertyDefOperations.GetPropertyDef(567);

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<PropertyDef>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/structure/properties/567", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDefAsync"/>
		/// uses the correct Http method.
		/// </summary>
		[TestMethod]
		public async Task GetPropertyDefAsync_CorrectMethod()
		{
			/* Arrange */

			// The method.
			Method? methodUsed = null;

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<PropertyDef>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					methodUsed = r.Method;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<PropertyDef>>
                        (
                            m => m.Data == new PropertyDef()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.PropertyDefOperations.GetPropertyDefAsync(123);

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<PropertyDef>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Method must be correct.
			Assert.AreEqual(Method.Get, methodUsed);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultPropertyDefOperations.GetPropertyDef"/>
		/// uses the correct Http method.
		/// </summary>
		[TestMethod]
		public void GetPropertyDef_CorrectMethod()
		{
			/* Arrange */

			// The method.
			Method? methodUsed = null;

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<PropertyDef>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					methodUsed = r.Method;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<PropertyDef>>
                        (
                            m => m.Data == new PropertyDef()
                        )
                    );
				});

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.PropertyDefOperations.GetPropertyDef(567);

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<PropertyDef>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Method must be correct.
			Assert.AreEqual(Method.Get, methodUsed);
		}

		#endregion

	}
}
