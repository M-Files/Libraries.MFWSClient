using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Extensions;

namespace MFaaP.MFWSClient
{
	/// <summary>
	/// Value list item operations.
	/// </summary>
	public class MFWSVaultValueListItemOperations
		: MFWSVaultOperationsBase
	{
		/// <inheritdoc />
		public MFWSVaultValueListItemOperations(MFWSClientBase client)
			: base(client)
		{
		}

		/// <summary>
		/// Gets the contents of the value list with Id <see paramref="valueListId"/>.
		/// </summary>
		/// <param name="valueListId">The Id of the value list to return the items from.</param>
		/// <param name="nameFilter">If has a value, is used to filter the items by name.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <param name="limit">If 0, uses Mfiles default at server (default 500 items returned)</param>
		/// <returns>The contents of the value list.</returns>
		/// <remarks>Note that this may be limited.</remarks>
		public async Task<Results<ValueListItem>> GetValueListItemsAsync(int valueListId, string nameFilter = null, CancellationToken token = default(CancellationToken), int limit = 0)
		{
			// Create the request.
			var request = new RestRequest($"/REST/valuelists/{valueListId}/items");

			// Filter by name?
			var querySeperator = "?";
			if (false == string.IsNullOrWhiteSpace(nameFilter))
			{
				request.Resource += "?filter=" + WebUtility.UrlEncode(nameFilter);
				querySeperator = "&";
			}

			if (limit > 0)
			{
				request.Resource += $"{querySeperator}limit={limit.ToString()}";
			}

			// Make the request and get the response.
			var response = await this.MFWSClient.Get<Results<ValueListItem>>(request, token)
				.ConfigureAwait(false);

			// Return the data.
			return response.Data;
		}

		/// <summary>
		/// Gets the contents of the value list with Id <see paramref="valueListId"/>.
		/// </summary>
		/// <param name="valueListId">The Id of the value list to return the items from.</param>
		/// <param name="nameFilter">If has a value, is used to filter the items by name.</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <param name="limit">If 0, uses Mfiles default at server (default 500 items returned)</param>
		/// <returns>The contents of the value list.</returns>
		/// <remarks>Note that this may be limited.</remarks>
		public Results<ValueListItem> GetValueListItems(int valueListId, string nameFilter = null, CancellationToken token = default(CancellationToken), int limit = 0)
		{
			// Execute the async method.
			return this.GetValueListItemsAsync(valueListId, nameFilter, token, limit)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}


		/// <summary>
		/// Creates a new item with the <see cref="newItemName"/> in the value list with id <see cref="valueListId"/>.
		/// </summary>
		/// <param name="valueListId">The Id of the value list to create the new item in.</param>
		/// <param name="newItemName">Name of the new value list item to create</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The newly created value list item</returns>
		public async Task<ValueListItem> AddValueListItemAsync(int valueListId, string newItemName, CancellationToken token = default(CancellationToken))
		{
			var newValueListItem = new ValueListItem
			{
				Name		= newItemName,
				ValueListID = valueListId
			};

			// Create the request.
			var request = new RestRequest($"/REST/valuelists/{valueListId}/items");
			request.AddJsonBody(newValueListItem);

			// Make the request and get the response.
			var response = await this.MFWSClient.Post<ValueListItem>(request, token)
				.ConfigureAwait(false);

			return response.Data;
		}

		/// Creates a new item with the <see cref="newItemName"/> in the value list with id <see cref="valueListId"/>.
		/// </summary>
		/// <param name="valueListId">The Id of the value list to create the new item in.</param>
		/// <param name="newItemName">Name of the new value list item to create</param>
		/// <param name="token">A cancellation token for the request.</param>
		/// <returns>The newly created value list item</returns>
		public ValueListItem AddValueListItem(int valueListId, string newItemName, CancellationToken token = default(CancellationToken))
		{
			// Execute the async method.
			return this.AddValueListItemAsync(valueListId, newItemName, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}
	}
}
