using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace MFaaP.MFWSClient.Tests
{
	public partial class MFWSClient
	{

		#region AuthenticateUsingSingleSignOn

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSClient.AuthenticateUsingSingleSignOn"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void AuthenticateUsingSingleSignOn()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner(Method.Get, "/WebServiceSSO.aspx?popup=1&vault=e1ce61eb-b255-41c3-b64c-faa9feb070f0");

			// The ASP.NET session Id (dummy value).
			var sessionId = Guid.NewGuid().ToString();

			// We need to set a base url so that we can check the session ID values.
			runner.RestClientMock.SetupAllProperties();
            runner.RestClientMock.Setup(c => c.BaseUrl)
                .Returns(new Uri("http://example.org/"));
            {
                var options = new RestClientOptions();
                options.BaseUrl = runner.RestClientMock.Object.BaseUrl;
                runner.RestClientMock.Setup(m => m.Options).Returns(options);
                var cookieContainer = new CookieContainer();
                runner.RestClientMock.Setup(m => m.CookieContainer).Returns(cookieContainer);
            }

            // Set up the response to include the session Id.
            runner.RestClientMock
				.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) =>
				{
					Assert.AreEqual(Method.Get, r.Method, $"HTTP method {Method.Get} expected, but {r.Method} was executed.");

					var vaultParameter = r.Parameters.FirstOrDefault(p => p.Type == ParameterType.QueryString && p.Name == "vault");
					Assert.IsNotNull(vaultParameter, "Vault parameter was not found on the request.");
					Assert.AreEqual("E1CE61EB-B255-41C3-B64C-FAA9FEB070F0", vaultParameter.Value, $"Wrong vault guid {vaultParameter.Value} in a querystring parameter.");
				})
				// Return a mock response.
				.Returns(() =>
                {
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse>
                        (
                            r => r.Cookies == new CookieCollection()
                            {
                                new Cookie()
                                {
                                    Name = "ASP.NET_SessionId",
                                    Value = sessionId,
                                    Path = "/",
                                    Domain = "example.org"
                                }
                            }
                        )
                    );
                });
			
			// Execute.
			runner.MFWSClient.AuthenticateUsingSingleSignOn(Guid.Parse("e1ce61eb-b255-41c3-b64c-faa9feb070f0"));

			// Verify.
			runner.Verify();

			// Ensure cookie is in default cookie container.
			var requestSessionCookie = runner.MFWSClient
				.CookieContainer
				.GetCookies(new Uri("http://example.org"))
				.Cast<Cookie>()
				.FirstOrDefault(c => c.Name == "ASP.NET_SessionId");
			Assert.IsNotNull(requestSessionCookie);
			Assert.AreEqual(requestSessionCookie.Value, sessionId);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSClient.AuthenticateUsingSingleSignOnAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task AuthenticateUsingSingleSignOnAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner(Method.Get, "/WebServiceSSO.aspx?popup=1&vault=e1ce61eb-b255-41c3-b64c-faa9feb070f0");

			// The ASP.NET session Id (dummy value).
			var sessionId = Guid.NewGuid().ToString();

			// We need to set a base url so that we can check the session ID values.
			runner.RestClientMock.SetupAllProperties();
            runner.RestClientMock.Setup(c => c.BaseUrl)
                .Returns(new Uri("http://example.org/"));
            {
                var options = new RestClientOptions();
                options.BaseUrl = runner.RestClientMock.Object.BaseUrl;
                runner.RestClientMock.Setup(m => m.Options).Returns(options);
                var cookieContainer = new CookieContainer();
                runner.RestClientMock.Setup(m => m.CookieContainer).Returns(cookieContainer);
            }

            // Set up the response to include the session Id.
            runner.RestClientMock
				.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) =>
				{
					Assert.AreEqual(Method.Get, r.Method, $"HTTP method {Method.Get} expected, but {r.Method} was executed.");

					var vaultParameter = r.Parameters.FirstOrDefault(p => p.Type == ParameterType.QueryString && p.Name == "vault");
					Assert.IsNotNull(vaultParameter, "Vault parameter was not found on the request.");
					Assert.AreEqual("E1CE61EB-B255-41C3-B64C-FAA9FEB070F0", vaultParameter.Value, $"Wrong vault guid {vaultParameter.Value} in a querystring parameter.");
				})
				// Return a mock response.
				.Returns(() =>
				{
					return Task.FromResult
					(
						Mock.Of<RestResponse>
						(
							r => r.Cookies == new CookieCollection()
                            {
								new Cookie()
								{
									Name = "ASP.NET_SessionId",
									Value = sessionId,
									Path = "/",
									Domain = "example.org"
	                            }
							}
                        )
					);
				});

			// Execute.
			await runner.MFWSClient.AuthenticateUsingSingleSignOnAsync(Guid.Parse("e1ce61eb-b255-41c3-b64c-faa9feb070f0"));

			// Verify.
			runner.Verify();

			// Ensure cookie is in default cookie container.
			var requestSessionCookie = runner.MFWSClient
				.CookieContainer
				.GetCookies(new Uri("http://example.org"))
				.Cast<Cookie>()
				.FirstOrDefault(c => c.Name == "ASP.NET_SessionId");
			Assert.IsNotNull(requestSessionCookie);
			Assert.AreEqual(requestSessionCookie.Value, sessionId);
		}

		#endregion

		#region Log out

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSClient.LogOut(CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task LogOutAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner(Method.Delete, "/REST/session.aspx");

			// Execute.
			await runner.MFWSClient.LogOutAsync();

			// Verify.
			runner.Verify();
		}

		#endregion

		#region AuthenticateUsingCredentials

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSClient.AuthenticateUsingCredentials(System.Nullable{System.Guid},string,string,string,System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void AuthenticateUsingCredentials()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<string>>(Method.Post, "/REST/server/authenticationtokens");

			// When the execute method is called, return a dummy authentication token.
			runner.ResponseData = new PrimitiveType<string>()
			{
				Value = "hello world"
			};

			// Create the object to send in the body.
			var body = new Authentication
			{
				VaultGuid = Guid.NewGuid(),
				Username = "my username",
				Password = "my password",
				SessionID = "mySessionId",
				Expiration = new DateTime(2017, 01, 01, 0, 0, 0, DateTimeKind.Utc)
			};
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.AuthenticateUsingCredentials(body.VaultGuid.Value, body.Username, body.Password, body.Expiration.Value, body.SessionID);

			// Verify.
			runner.Verify();

			// Authentication header must exist.
			var authenticationHeader = runner.MFWSClient
				.DefaultParameters
				.FirstOrDefault(h => h.Type == ParameterType.HttpHeader && h.Name == "X-Authentication");
			Assert.IsNotNull(authenticationHeader);
			Assert.AreEqual("hello world", authenticationHeader.Value);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSClient.AuthenticateUsingCredentialsAsync(System.Nullable{System.Guid},string,string,System.Nullable{System.DateTime},string,System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task AuthenticateUsingCredentialsAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<string>>(Method.Post, "/REST/server/authenticationtokens");

			// When the execute method is called, return a dummy authentication token.
			runner.ResponseData = new PrimitiveType<string>()
			{
				Value = "hello world"
			};

			// Create the object to send in the body.
			var body = new Authentication
			{
				VaultGuid = Guid.NewGuid(),
				Username = "my username",
				Password = "my password",
				SessionID = "mySessionId",
				Expiration = new DateTime(2017, 01, 01, 0, 0, 0, DateTimeKind.Utc)
			};
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.AuthenticateUsingCredentialsAsync(body.VaultGuid.Value, body.Username, body.Password, body.Expiration.Value, body.SessionID);

			// Verify.
			runner.Verify();

			// Authentication header must exist.
			var authenticationHeader = runner.MFWSClient
				.DefaultParameters
				.FirstOrDefault(h => h.Type == ParameterType.HttpHeader && h.Name == "X-Authentication");
			Assert.IsNotNull(authenticationHeader);
			Assert.AreEqual("hello world", authenticationHeader.Value);
		}

		#endregion

		#region GetOnlineVaults

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSClient.GetOnlineVaults"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void GetOnlineVaults()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<Vault>>(Method.Get, "/REST/server/vaults?online=true");

			// Execute.
			runner.MFWSClient.GetOnlineVaults();

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSClient.GetOnlineVaultsAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task GetOnlineVaultsAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<Vault>>(Method.Get, "/REST/server/vaults?online=true");

			// Execute.
			await runner.MFWSClient.GetOnlineVaultsAsync();

			// Verify.
			runner.Verify();
		}

		#endregion

	}
}
