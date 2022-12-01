using System;
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
	public class MFWSVaultObjectSearchOperations
	{

		#region SearchForObjectsByString

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByString"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByStringAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>(Method.Get, $"/REST/objects?q=hello+world");
			
			// Execute.
			await runner.MFWSClient.ObjectSearchOperations.SearchForObjectsByStringAsync("hello world");

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByString"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByString()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>(Method.Get, $"/REST/objects?q=hello+world");
			
			// Execute.
			runner.MFWSClient.ObjectSearchOperations.SearchForObjectsByString("hello world");

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByString"/>
		/// requests the correct resource address when used with an object type.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByStringAsync_WithObjectType()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>(Method.Get, $"/REST/objects?q=hello+world&o=0");
			
			// Execute.
			await runner.MFWSClient.ObjectSearchOperations.SearchForObjectsByStringAsync("hello world", 0);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByString"/>
		/// requests the correct resource address when used with an object type.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByString_WithObjectType()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>(Method.Get, $"/REST/objects?q=hello+world&o=0");
			
			// Execute.
			runner.MFWSClient.ObjectSearchOperations.SearchForObjectsByString("hello world", 0);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region QuickSearchCondition

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// uses the correct Http method.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_QuickSearchCondition()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?q=hello+world"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new QuickSearchCondition("hello world")
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// uses the correct Http method.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_QuickSearchCondition()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?q=hello+world"
			);
			
			// Execute.
			runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditions
				(
					new QuickSearchCondition("hello world")
				);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region ObjectTypeSearchCondition

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_ObjectTypeSearchCondition()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?o=123"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new ObjectTypeSearchCondition(123)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address when passing a limit to the number of records.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_ObjectTypeSearchConditionAndRecordLimit()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?o=123&limit=2"
			);

			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					2,
					new ObjectTypeSearchCondition(123)
				);

			// Verify.
			runner.Verify();
		}


		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address when passing a limit = 0 (unlimited numbers of records).
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_ObjectTypeSearchConditionAndUnlimited()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?o=123&limit=0"
			);

			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					0,
					new ObjectTypeSearchCondition(123)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address when passing a limit = -1 (default numbers (500) of records).
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_ObjectTypeSearchConditionDefaultLimit()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?o=123"
			);

			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					-1,
					new ObjectTypeSearchCondition(123)
				);

			// Verify.
			runner.Verify();
		}


		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_ObjectTypeSearchCondition_CorrectResource()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?o=123"
			);
			
			// Execute.
			runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditions
				(
					new ObjectTypeSearchCondition(123)
				);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region IncludeDeletedObjectsSearchCondition

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditionsAsync(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_IncludeDeletedObjectsSearchCondition()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?d=include"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new IncludeDeletedObjectsSearchCondition()
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditionsAsync(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_IncludeDeletedObjectsSearchCondition()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?d=include"
			);
			
			// Execute.
			runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditions
				(
					new IncludeDeletedObjectsSearchCondition()
				);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region BooleanPropertyValueSearchCondition

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_BooleanPropertyValueSearchCondition_True()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123=true"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new BooleanPropertyValueSearchCondition(123, true)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_BooleanPropertyValueSearchCondition_True()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123=true"
			);
			
			// Execute.
			runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditions
				(
					new BooleanPropertyValueSearchCondition(123, true)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_BooleanPropertyValueSearchCondition_False()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123=false"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new BooleanPropertyValueSearchCondition(123, false)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_BooleanPropertyValueSearchCondition_CorrectResource_False()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123=false"
			);
			
			// Execute.
			runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditions
				(
					new BooleanPropertyValueSearchCondition(123, false)
				);

			// Verify.
			runner.Verify();
		}

        #endregion

        #region TimestampPropertyValueSearchCondition

        /// <summary>
        /// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
        /// requests the correct resource address.
        /// </summary>
        [TestMethod]
        public async Task SearchForObjectsByConditionsAsync_TimestampPropertyValueSearchCondition_Zulu_Equals()
        {
            // Create our test runner.
            var runner = new RestApiTestRunner<Results<ObjectVersion>>
            (
                Method.Get,
                $"/REST/objects?p21=2017-01-01T11%3A12%3A03.0000000%2B00%3A00"
            );

            // Execute.
            await runner
                .MFWSClient
                .ObjectSearchOperations
                .SearchForObjectsByConditionsAsync
                (
                    new TimestampPropertyValueSearchCondition(21, new DateTimeOffset(2017, 01, 01, 11, 12, 03, TimeSpan.Zero))
                );

            // Verify.
            runner.Verify();
        }

        /// <summary>
        /// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
        /// requests the correct resource address.
        /// </summary>
        [TestMethod]
        public async Task SearchForObjectsByConditionsAsync_TimestampPropertyValueSearchCondition_WithTwoHourOffset_Equals()
        {
            // Create our test runner.
            var runner = new RestApiTestRunner<Results<ObjectVersion>>
            (
                Method.Get,
                $"/REST/objects?p21=2017-01-01T11%3A12%3A03.0000000%2B02%3A00"
            );

            // Execute.
            await runner
                .MFWSClient
                .ObjectSearchOperations
                .SearchForObjectsByConditionsAsync
                (
                    new TimestampPropertyValueSearchCondition(21, new DateTimeOffset(2017, 01, 01, 11, 12, 03, TimeSpan.FromHours(2)))
                );

            // Verify.
            runner.Verify();
        }

        #endregion

        #region DatePropertyValueSearchCondition

        /// <summary>
        /// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
        /// requests the correct resource address.
        /// </summary>
        [TestMethod]
		public async Task SearchForObjectsByConditionsAsync_DatePropertyValueSearchCondition_Equals()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123=2017-01-01"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01))
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_DatePropertyValueSearchCondition_Equals()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123=2017-01-01"
			);
			
			// Execute.
			runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditions
				(
					new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01))
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_DatePropertyValueSearchCondition_LessThan()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123<<=2017-01-01"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01), SearchConditionOperators.LessThan)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_DatePropertyValueSearchCondition_LessThan()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123<<=2017-01-01"
			);
			
			// Execute.
			runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditions
				(
					new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01), SearchConditionOperators.LessThan)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_DatePropertyValueSearchCondition_LessThanOrEqual()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123<=2017-01-01"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01), SearchConditionOperators.LessThanOrEqual)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_DatePropertyValueSearchCondition_LessThanOrEqual()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123<=2017-01-01"
			);
			
			// Execute.
			runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditions
				(
					new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01), SearchConditionOperators.LessThanOrEqual)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_DatePropertyValueSearchCondition_CorrectResource_GreaterThan()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Results<ObjectVersion>>
			(
				Method.Get,
				$"/REST/objects?p123>>=2017-01-01"
			);
			
			// Execute.
			await runner
				.MFWSClient
				.ObjectSearchOperations
				.SearchForObjectsByConditionsAsync
				(
					new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01), SearchConditionOperators.GreaterThan)
				);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_DatePropertyValueSearchCondition_CorrectResource_GreaterThan()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
					(
						Mock.Of<RestResponse<Results<ObjectVersion>>>
						(
							m => m.Data == new Results<ObjectVersion>()
						)
					);
				});

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01), SearchConditionOperators.GreaterThan));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123>>=2017-01-01", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_DatePropertyValueSearchCondition_CorrectResource_GreaterThanOrEqual()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.ObjectSearchOperations.SearchForObjectsByConditionsAsync(new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01), SearchConditionOperators.GreaterThanOrEqual));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123>=2017-01-01", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_DatePropertyValueSearchCondition_CorrectResource_GreaterThanOrEqual()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
                {
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new DatePropertyValueSearchCondition(123, new DateTime(2017, 01, 01), SearchConditionOperators.GreaterThanOrEqual));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123>=2017-01-01", resourceAddress);
		}

		#endregion

		#region LookupPropertyValueSearchCondition

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_LookupPropertyValueSearchCondition_CorrectResource()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.ObjectSearchOperations.SearchForObjectsByConditionsAsync(new LookupPropertyValueSearchCondition(123, lookupIds: 456));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123=456", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_LookupPropertyValueSearchCondition_CorrectResource()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new LookupPropertyValueSearchCondition(123, lookupIds: 456));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123=456", resourceAddress);
		}

		#endregion

		#region MultiSelectLookupPropertyValueSearchCondition

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_MultiSelectLookupPropertyValueSearchCondition_CorrectResource()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.ObjectSearchOperations.SearchForObjectsByConditionsAsync(new MultiSelectLookupPropertyValueSearchCondition(123, lookupIds: new[] { 456, 789 }));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123=456%2C789", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_MultiSelectLookupPropertyValueSearchCondition_CorrectResource()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new MultiSelectLookupPropertyValueSearchCondition(123, lookupIds: new[] { 456, 789 }));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123=456%2C789", resourceAddress);
		}

		#endregion

		#region TextPropertyValueSearchCondition

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_TextPropertyValueSearchCondition_CorrectResource_Equals()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.ObjectSearchOperations.SearchForObjectsByConditionsAsync(new TextPropertyValueSearchCondition(123, "hello"));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123=hello", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_TextPropertyValueSearchCondition_CorrectResource_Equals()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new TextPropertyValueSearchCondition(123, "hello"));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123=hello", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_TextPropertyValueSearchCondition_CorrectResource_MatchesWildcard()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.ObjectSearchOperations.SearchForObjectsByConditionsAsync(new TextPropertyValueSearchCondition(123, "hello*", SearchConditionOperators.MatchesWildcard));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123**=hello*", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_TextPropertyValueSearchCondition_CorrectResource_MatchesWildcard()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new TextPropertyValueSearchCondition(123, "hello*", SearchConditionOperators.MatchesWildcard));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123**=hello*", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_TextPropertyValueSearchCondition_CorrectResource_Contains()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.ObjectSearchOperations.SearchForObjectsByConditionsAsync(new TextPropertyValueSearchCondition(123, "hello", SearchConditionOperators.Contains));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123*=hello", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_TextPropertyValueSearchCondition_CorrectResource_Contains()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new TextPropertyValueSearchCondition(123, "hello", SearchConditionOperators.Contains));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123*=hello", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_TextPropertyValueSearchCondition_CorrectResource_StartsWith()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.ObjectSearchOperations.SearchForObjectsByConditionsAsync(new TextPropertyValueSearchCondition(123, "hello", SearchConditionOperators.StartsWith));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123^=hello", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditions_TextPropertyValueSearchCondition_CorrectResource_StartsWith()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new TextPropertyValueSearchCondition(123, "hello", SearchConditionOperators.StartsWith));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?p123^=hello", resourceAddress);
		}

		#endregion
		
		#region ValueListSearchCondition

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public async Task SearchForObjectsByConditionsAsync_ValueList_CorrectResource_InternalId()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			await mfwsClient.ObjectSearchOperations.SearchForObjectsByConditionsAsync(new ValueListSearchCondition(123, 123));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?vl123=123", resourceAddress);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectSearchOperations.SearchForObjectsByConditions(MFaaP.MFWSClient.ISearchCondition[])"/>
		/// requests the correct resource address.
		/// </summary>
		[TestMethod]
		public void SearchForObjectsByConditionsAsync_ValueList_CorrectResource_ExternalId()
		{
			/* Arrange */

			// The actual requested address.
			var resourceAddress = "";

			// Create our restsharp mock.
			var mock = new Mock<IRestClient>();

			// When the execute method is called, log the resource requested.
			mock
				.Setup(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
				.Callback((RestRequest r, CancellationToken t) => {
					resourceAddress = r.Resource;
				})
				// Return a mock response.
				.Returns(() =>
				{
                    // Create the mock response.
                    return Task.FromResult
                    (
                        Mock.Of<RestResponse<Results<ObjectVersion>>>
                        (
                            m => m.Data == new Results<ObjectVersion>()
                        )
                    );
                });

			/* Act */

			// Create our MFWSClient.
			var mfwsClient = MFWSClient.GetMFWSClient(mock);

			// Execute.
			mfwsClient.ObjectSearchOperations.SearchForObjectsByConditions(new ValueListSearchCondition(123, "hello"));

			/* Assert */

			// Execute must be called once.
			mock.Verify(c => c.ExecuteAsync<Results<ObjectVersion>>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

			// Resource must be correct.
			Assert.AreEqual("/REST/objects?vl123=ehello", resourceAddress);
		}

		#endregion

	}
}
