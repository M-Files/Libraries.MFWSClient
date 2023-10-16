using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace MFaaP.MFWSClient
{
	/// <summary>
	/// Methods to create and modify objects.
	/// </summary>
	public class MFWSVaultClassGroupOperations
		: MFWSVaultOperationsBase
	{
		/// <summary>
		/// Creates a new <see cref="MFWSVaultClassGroupOperations"/> object.
		/// </summary>
		/// <param name="client">The client to interact with the server.</param>
		internal MFWSVaultClassGroupOperations(MFWSClientBase client)
			: base(client)
		{
		}



		/// <summary>
        /// Gets a list of all class groups with classes in the vault for a given object type.
        /// </summary>
        /// <param name="objectTypeId">The type of the object.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>All classes in the vault for the supplied object type.</returns>
        /// <remarks>This may be filtered by the user's permissions.</remarks>
        public async Task<List<ClassGroup>> GetClassGroupsAsync(int objectTypeId, CancellationToken token = default)
        {
            // Create the request.
            var request = new RestRequest($"/REST/structure/classes.aspx?objtype={objectTypeId}&bygroup=true");

            // Make the request and get the response.
            var response = await this.MFWSClient.Get<List<ClassGroup>>(request, token)
                .ConfigureAwait(false);

            // Return the data.
            return response.Data;
        }

        /// <summary>
        /// Gets a list of all classes in the vault for a given object type.
        /// </summary>
        /// <param name="objectTypeId">The type of the object.</param>
        /// <param name="token">A cancellation token for the request.</param>
        /// <returns>All classes in the vault for the supplied object type.</returns>
        /// <remarks>This may be filtered by the user's permissions.</remarks>
        public List<ClassGroup> GetClassGroups(int objectTypeId, CancellationToken token = default)
		{
			// Execute the async method.
			return this.GetClassGroupsAsync(objectTypeId, token)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

	}

}
