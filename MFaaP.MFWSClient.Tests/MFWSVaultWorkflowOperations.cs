﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MFaaP.MFWSClient.Tests
{
	[TestClass]
	public class MFWSVaultWorkflowOperations
	{
		#region Get workflow states

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStatesAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetWorkflowStatesAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<WorkflowState>>(Method.Get, "/REST/structure/workflows/1234/states.aspx");

			// Execute.
			await runner.MFWSClient.WorkflowOperations.GetWorkflowStatesAsync(1234);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStates"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetWorkflowStates()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<WorkflowState>>(Method.Get, "/REST/structure/workflows/12345/states.aspx");

			// Execute.
			runner.MFWSClient.WorkflowOperations.GetWorkflowStates(12345);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateByName"/>
		/// returns the correct state.
		/// </summary>
		[TestMethod]
		public void GetWorkflowStateByName_IncludesWorkflow()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<WorkflowState>>(Method.Get, "/REST/structure/workflows/12345/states.aspx");

			// Set up the response to include
			runner.ResponseData = new List<WorkflowState>
			{
				new WorkflowState()
				{
					ID = 1,
					Name = "abc"
				},
				new WorkflowState()
				{
					ID = 2,
					Name = "hello"
				}
			};

			// Execute.
			var result = runner.MFWSClient.WorkflowOperations.GetWorkflowStateByName(12345, "hello");

			// Verify.
			runner.Verify();

			// Ensure we got the right result back.
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.ID);
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateByName"/>
		/// returns null if no state with that name is found.
		/// </summary>
		[TestMethod]
		public void GetWorkflowStateByName_DoesNotIncludeWorkflow()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<WorkflowState>>(Method.Get, "/REST/structure/workflows/123/states.aspx");

			// Set up the response to include
			runner.ResponseData = new List<WorkflowState>
			{
				new WorkflowState()
				{
					ID = 1,
					Name = "abc"
				},
				new WorkflowState()
				{
					ID = 2,
					Name = "hello"
				}
			};

			// Execute.
			var result = runner.MFWSClient.WorkflowOperations.GetWorkflowStateByName(123, "hello world");

			// Verify.
			runner.Verify();

			// Ensure we got nothing back.
			Assert.IsNull(result);
		}

		#endregion

		#region Workflow alias to ID resolution

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowIDByAliasAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetWorkflowIDByAliasAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/workflows/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello world" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.WorkflowOperations.GetWorkflowIDByAliasAsync("hello world");

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowIDsByAliasesAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetWorkflowIDsByAliasesAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/workflows/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello", "world", "third option" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.WorkflowOperations.GetWorkflowIDsByAliasesAsync(aliases: new string[] { "hello", "world", "third option" });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowIDsByAliases"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetWorkflowIDsByAliases()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/workflows/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello", "world", "third option" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.WorkflowOperations.GetWorkflowIDsByAliases(aliases: new string[] { "hello", "world", "third option" });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowIDByAlias"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetWorkflowIDByAlias()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/workflows/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello world" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.WorkflowOperations.GetWorkflowIDByAlias("hello world");

			// Verify.
			runner.Verify();
		}

		#endregion

		#region Workflow state alias to ID resolution

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateIDByAliasAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetWorkflowStateIDByAliasAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/workflowstates/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello world" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.WorkflowOperations.GetWorkflowStateIDByAliasAsync("hello world");

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateIDsByAliasesAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetWorkflowStateIDsByAliasesAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/workflowstates/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello", "world", "third option" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.WorkflowOperations.GetWorkflowStateIDsByAliasesAsync(aliases: new string[] { "hello", "world", "third option" });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateIDsByAliases"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetWorkflowStateIDsByAliases()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/workflowstates/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello", "world", "third option" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.WorkflowOperations.GetWorkflowStateIDsByAliases(aliases: new string[] { "hello", "world", "third option" });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateIDByAlias"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetWorkflowStateIDByAlias()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/workflowstates/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello world" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.WorkflowOperations.GetWorkflowStateIDByAlias("hello world");

			// Verify.
			runner.Verify();
		}

		#endregion

		#region Workflow state transition alias to ID resolution

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateTransitionIDByAliasAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetWorkflowStateTransitionIDByAliasAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/statetransitions/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello world" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.WorkflowOperations.GetWorkflowStateTransitionIDByAliasAsync("hello world");

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateTransitionIDsByAliasesAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task GetWorkflowStateTransitionIDsByAliasesAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/statetransitions/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello", "world", "third option" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.WorkflowOperations.GetWorkflowStateTransitionIDsByAliasesAsync(aliases: new string[] { "hello", "world", "third option" });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateTransitionIDsByAliases"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetWorkflowStateTransitionIDsByAliases()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/statetransitions/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello", "world", "third option" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.WorkflowOperations.GetWorkflowStateTransitionIDsByAliases(aliases: new string[] { "hello", "world", "third option" });

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultWorkflowOperations.GetWorkflowStateTransitionIDByAlias"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void GetWorkflowStateTransitionIDByAlias()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<Dictionary<string, int>>(Method.Post, "/REST/structure/statetransitions/itemidbyalias.aspx");

			// Set up the expected body.
			var body = new JArray { "hello world" };
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.WorkflowOperations.GetWorkflowStateTransitionIDByAlias("hello world");

			// Verify.
			runner.Verify();
		}

		#endregion

	}
}