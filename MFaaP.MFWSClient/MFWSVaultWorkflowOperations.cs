﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace MFaaP.MFWSClient
{
	/// <summary>
	/// The VaultWorkflowOperations class represents the available workflow operations.
	/// </summary>
	/// <remarks>ref: https://www.m-files.com/api/documentation/latest/index.html#MFilesAPI~VaultWorkflowOperations.html </remarks>
	public class MFWSVaultWorkflowOperations
		: MFWSVaultOperationsBase
	{

		/// <inheritdoc />
		public MFWSVaultWorkflowOperations(MFWSClientBase client)
			: base(client)
		{
		}

		#region Get workflow states

		/// <summary>
		/// Gets all workflow states in the provided workflow.
		/// </summary>
		/// <param name="workflowId">The workflow identifier.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		public async Task<WorkflowState[]> GetWorkflowStatesAsync(int workflowId, CancellationToken token = default)
		{
			// Create the request.
			var request = new RestRequest($"/REST/structure/workflows/{workflowId}/states.aspx");

			// Execute the request and parse the response.
			var response = await this.MFWSClient.Get<List<WorkflowState>>(request, token);

			// Return the typed response.
			return response?.Data.ToArray();
		}

		/// <summary>
		/// Gets all workflow states in the provided workflow.
		/// </summary>
		/// <param name="workflowId">The workflow identifier.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		public WorkflowState[] GetWorkflowStates(int workflowId, CancellationToken token = default)
		{
			// Execute the async method.
			return this.GetWorkflowStatesAsync(workflowId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Gets the <see cref="WorkflowState"/> in the provided workflow with the name <paramref name="stateName"/>.
		/// </summary>
		/// <param name="workflowId">The workflow identifier.</param>
		/// <param name="stateName">Name of the state.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.  Task will return null if no state with that name is found.</returns>
		/// <remarks>Note that this will retrieve all workflow states from the server and then filter them by the name, so will be inefficient to call multiple times.</remarks>
		public WorkflowState GetWorkflowStateByName(int workflowId, string stateName, CancellationToken token = default)
		{
			// Get all the states.
			var workflowStates = this.GetWorkflowStates(workflowId, token);

			// Filter to just the first one.
			return workflowStates.FirstOrDefault(state => (state.Name ?? string.Empty).Equals(stateName, StringComparison.InvariantCultureIgnoreCase));
		}

		#endregion

		#region Workflow alias to ID resolution

		/// <summary>
		/// Retrieves the ID for a workflow with a given alias in the vault.
		/// </summary>
		/// <param name="alias">The alias for the workflow.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflows have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public async Task<int> GetWorkflowIDByAliasAsync(string alias, CancellationToken token = default)
		{
			// Use the other overload.
			var output = await this.GetWorkflowIDsByAliasesAsync(token, aliases: new string[] { alias });
			return output?.Count == 1
				? output[0]
				: -1;
		}

		/// <summary>
		/// Retrieves the IDs for workflows with given aliases in the vault.
		/// </summary>
		/// <param name="aliases">The aliases for the workflow.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflows have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public async Task<List<int>> GetWorkflowIDsByAliasesAsync(CancellationToken token = default, params string[] aliases)
		{
			// Sanity.
			if (null == aliases)
				throw new ArgumentNullException(nameof(aliases));
			if (0 == aliases.Length)
				return new List<int>();

			// Create the request.
			var request = new RestRequest($"/REST/structure/workflows/itemidbyalias.aspx");

			// Assign the body.
			request.AddJsonBody(aliases);

			// Make the request and get the response.
			var response = await this.MFWSClient.Post<Dictionary<string, int>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return aliases.Select(alias => response.Data.ContainsKey(alias) ? response.Data[alias] : -1).ToList();
		}

		/// <summary>
		/// Retrieves the IDs for workflows with given aliases in the vault.
		/// </summary>
		/// <param name="aliases">The aliases for the workflow.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflows have the alias, or more than one does).</remarks>
		public List<int> GetWorkflowIDsByAliases(CancellationToken token = default, params string[] aliases)
		{
			// Execute the async method.
			return this.GetWorkflowIDsByAliasesAsync(token, aliases)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Retrieves the ID for a workflow with a given alias in the vault.
		/// </summary>
		/// <param name="alias">The alias for the workflow.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflows have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public int GetWorkflowIDByAlias(string alias, CancellationToken token = default)
		{
			// Use the other overload.
			var output = this.GetWorkflowIDsByAliases(token, aliases: new string[] { alias });
			return output?.Count == 1
				? output[0]
				: -1;
		}

		#endregion

		#region Workflow state alias to ID resolution

		/// <summary>
		/// Retrieves the ID for a workflow state with a given alias in the vault.
		/// </summary>
		/// <param name="alias">The alias for the workflow state.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflow states have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public async Task<int> GetWorkflowStateIDByAliasAsync(string alias, CancellationToken token = default)
		{
			// Use the other overload.
			var output = await this.GetWorkflowStateIDsByAliasesAsync(token, aliases: new string[] { alias });
			return output?.Count == 1
				? output[0]
				: -1;
		}

		/// <summary>
		/// Retrieves the IDs for workflow states with given aliases in the vault.
		/// </summary>
		/// <param name="aliases">The aliases for the workflow state.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflow states have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public async Task<List<int>> GetWorkflowStateIDsByAliasesAsync(CancellationToken token = default, params string[] aliases)
		{
			// Sanity.
			if (null == aliases)
				throw new ArgumentNullException(nameof(aliases));
			if (0 == aliases.Length)
				return new List<int>();

			// Create the request.
			var request = new RestRequest($"/REST/structure/workflowstates/itemidbyalias.aspx");

			// Assign the body.
			request.AddJsonBody(aliases);

			// Make the request and get the response.
			var response = await this.MFWSClient.Post<Dictionary<string, int>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return aliases.Select(alias => response.Data.ContainsKey(alias) ? response.Data[alias] : -1).ToList();
		}

		/// <summary>
		/// Retrieves the IDs for workflow states with given aliases in the vault.
		/// </summary>
		/// <param name="aliases">The aliases for the workflow state.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflow states have the alias, or more than one does).</remarks>
		public List<int> GetWorkflowStateIDsByAliases(CancellationToken token = default, params string[] aliases)
		{
			// Execute the async method.
			return this.GetWorkflowStateIDsByAliasesAsync(token, aliases)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Retrieves the ID for a workflow state with a given alias in the vault.
		/// </summary>
		/// <param name="alias">The alias for the workflow state.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflow states have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public int GetWorkflowStateIDByAlias(string alias, CancellationToken token = default)
		{
			// Use the other overload.
			var output = this.GetWorkflowStateIDsByAliases(token, aliases: new string[] { alias });
			return output?.Count == 1
				? output[0]
				: -1;
		}

		#endregion

		#region Workflow state transition alias to ID resolution

		/// <summary>
		/// Retrieves the ID for a workflow state transition with a given alias in the vault.
		/// </summary>
		/// <param name="alias">The alias for the workflow state transition.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflow state transitions have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public async Task<int> GetWorkflowStateTransitionIDByAliasAsync(string alias, CancellationToken token = default)
		{
			// Use the other overload.
			var output = await this.GetWorkflowStateTransitionIDsByAliasesAsync(token, aliases: new string[] { alias });
			return output?.Count == 1
				? output[0]
				: -1;
		}

		/// <summary>
		/// Retrieves the IDs for workflow state transitions with given aliases in the vault.
		/// </summary>
		/// <param name="aliases">The aliases for the workflow state transition.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflow state transitions have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public async Task<List<int>> GetWorkflowStateTransitionIDsByAliasesAsync(CancellationToken token = default, params string[] aliases)
		{
			// Sanity.
			if (null == aliases)
				throw new ArgumentNullException(nameof(aliases));
			if (0 == aliases.Length)
				return new List<int>();

			// Create the request.
			var request = new RestRequest($"/REST/structure/statetransitions/itemidbyalias.aspx");

			// Assign the body.
			request.AddJsonBody(aliases);

			// Make the request and get the response.
			var response = await this.MFWSClient.Post<Dictionary<string, int>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return aliases.Select(alias => response.Data.ContainsKey(alias) ? response.Data[alias] : -1).ToList();
		}

		/// <summary>
		/// Retrieves the IDs for workflow state transitions with given aliases in the vault.
		/// </summary>
		/// <param name="aliases">The aliases for the workflow state transition.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflow state transitions have the alias, or more than one does).</remarks>
		public List<int> GetWorkflowStateTransitionIDsByAliases(CancellationToken token = default, params string[] aliases)
		{
			// Execute the async method.
			return this.GetWorkflowStateTransitionIDsByAliasesAsync(token, aliases)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

		/// <summary>
		/// Retrieves the ID for a workflow state transition with a given alias in the vault.
		/// </summary>
		/// <param name="alias">The alias for the workflow state transition.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>An awaitable task for the request.</returns>
		/// <remarks>Returns -1 if the alias cannot be resolved (e.g. no workflow state transitions have the alias, or more than one does).</remarks>
		/// <remarks>Only available in M-Files 12.0.6768.0 upwards.</remarks>
		public int GetWorkflowStateTransitionIDByAlias(string alias, CancellationToken token = default)
		{
			// Use the other overload.
			var output = this.GetWorkflowStateTransitionIDsByAliases(token, aliases: new string[] { alias });
			return output?.Count == 1
				? output[0]
				: -1;
		}

		#endregion

	}
}
