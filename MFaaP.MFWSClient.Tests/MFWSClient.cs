﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace MFaaP.MFWSClient.Tests
{
	[TestClass]
	public partial class MFWSClient
	{

		/// <summary>
		/// Ensures that a call to <see cref="MFWSClientBase.GetMetadataStructureIDsByAliasesAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetMetadataStructureIDsByAliasesAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<VaultStructureAliasResponse>(Method.Post, "/REST/structure/metadatastructure/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new VaultStructureAliasRequest();
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.GetMetadataStructureIDsByAliasesAsync(body);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFWSClientBase.GetMetadataStructureIDsByAliases"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetMetadataStructureIDsByAliases()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<VaultStructureAliasResponse>(Method.Post, "/REST/structure/metadatastructure/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new VaultStructureAliasRequest();
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.GetMetadataStructureIDsByAliases(body);

			// Verify.
			runner.Verify();
		}

		[TestMethod]
		public void InstantiationAlsoPopulatesPropertiesAndFields()
		{
			var client = new MFaaP.MFWSClient.MFWSClient("http://localhost");
			Assert.IsNotNull(client.AutomaticMetadataOperations);
			Assert.IsNotNull(client.ClassGroupOperations);
			Assert.IsNotNull(client.ClassOperations);
			Assert.IsNotNull(client.ExtensionAuthenticationOperations);
			Assert.IsNotNull(client.ExtensionMethodOperations);
			Assert.IsNotNull(client.ExternalObjectOperations);
			Assert.IsNotNull(client.ObjectFileOperations);
			Assert.IsNotNull(client.ObjectOperations);
			Assert.IsNotNull(client.ObjectPropertyOperations);
			Assert.IsNotNull(client.ObjectSearchOperations);
			Assert.IsNotNull(client.ObjectTypeOperations);
			Assert.IsNotNull(client.PropertyDefOperations);
			Assert.IsNotNull(client.ValueListItemOperations);
			Assert.IsNotNull(client.ValueListOperations);
			Assert.IsNotNull(client.WorkflowOperations);
			Assert.IsNotNull(client.CookieContainer);
		}

		/// <summary>
		/// Creates a MFWSClient using the supplied mock request client.
		/// </summary>
		/// <param name="restClientMoq">The mocked rest client.</param>
		/// <returns></returns>
		public static MFaaP.MFWSClient.MFWSClient GetMFWSClient(Moq.Mock<IRestClient> restClientMoq)
		{
			// Sanity.
			if(null == restClientMoq)
				throw new ArgumentNullException(nameof(restClientMoq));

			// Ensure that we have a default parameter collection, if it's not been mocked already.
			if (null == restClientMoq.Object.DefaultParameters)
			{
                var defaultParameters = new DefaultParameters(new ReadOnlyRestClientOptions(new RestClientOptions()));
                restClientMoq
                    .SetupGet(p => p.DefaultParameters)
					.Returns(defaultParameters);
			}

			// Return our proxy.
			return new MFWSClientProxy(restClientMoq.Object);
		}

		/// <summary>
		/// A proxy around <see cref="MFaaP.MFWSClient.MFWSClient"/>
		/// to allow provision of a mocked <see cref="IRestClient"/>.
		/// </summary>
		private class MFWSClientProxy
			: MFaaP.MFWSClient.MFWSClient
		{
			/// <inheritdoc />
			public MFWSClientProxy(IRestClient restClient) 
				: base(restClient)
			{
			}
			protected override void OnAfterExecuteRequest(RestResponse e)
			{
				// If the response is null it's because we were testing for the wrong endpoint details.
				if (null == e)
				{
					Assert.Fail("Incorrect HTTP request (either method, endpoint address, or return type was invalid).");
					return;
				}

				// Base implementation.
				base.OnAfterExecuteRequest(e);
			}

            protected override IRestClient GenerateClientForSingleSignOn()
            {
				// We use the current mock client
				return RestClient;
            }
        }
	}
}
