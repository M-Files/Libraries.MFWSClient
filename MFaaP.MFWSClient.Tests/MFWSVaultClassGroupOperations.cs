using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace MFaaP.MFWSClient.Tests
{
	[TestClass]
	public class MFWSVaultClassGroupOperations
	{

		#region GetClassGroups

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultClassGroupOperations.GetClassGroupsAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetClassGroupsAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<ClassGroup>>(Method.GET, "/REST/structure/classes.aspx?objtype=0&bygroup=true");

			// Execute.
			await runner.MFWSClient.ClassGroupOperations.GetClassGroupsAsync(0);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultClassGroupOperations.GetClassGroups"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetClassGroups()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<ClassGroup>>(Method.GET, "/REST/structure/classes.aspx?objtype=0&bygroup=true");

			// Execute.
			runner.MFWSClient.ClassGroupOperations.GetClassGroups(0);

			// Verify.
			runner.Verify();
		}

		#endregion
	}
}
