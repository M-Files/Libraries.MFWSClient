using System.Text.Json.Serialization;

namespace MFaaP.MFWSClient.OAuth2
{
	/// <summary>
	/// Represents a set of tokens returned from an OAuth 2.0 token endpoint.
	/// The <see cref="TokenType"/> should be "Bearer", and the <see cref="AccessToken"/> can be used
	/// to access resources.  Should this expire, a subsequent request can be made using the <see cref="RefreshToken"/>
	/// to retrieve an updated token.
	/// </summary>
	public class OAuth2TokenResponse
	{
		/// <summary>
		/// The token type.  Should be "Bearer".
		/// </summary>
		[JsonPropertyName("token_type")]
		public string TokenType { get; set; }

		/// <summary>
		/// Any scopes defined for the tokens.
		/// </summary>
		[JsonPropertyName("scope")]
		public string Scope { get; set; }

		/// <summary>
		/// How long - since the original request - the access token is valid for.
		/// </summary>
		[JsonPropertyName("expires_in")]
		public long ExpiresIn { get; set; }

		//[JsonPropertyName("ext_expires_in")]
		//public long ExtExpiresIn { get; set; }

		/// <summary>
		/// When the access token expires.
		/// </summary>
		[JsonPropertyName("expires_on")]
		public long ExpiresOn { get; set; }

		/// <summary>
		/// When token becomes valid.
		/// </summary>

		[JsonPropertyName("not_before")]
		public long NotBefore { get; set; }

		/// <summary>
		/// The resource this token is for.
		/// </summary>
		[JsonPropertyName("resource")]
		public string Resource { get; set; }

		/// <summary>
		/// The access token.
		/// </summary>
		[JsonPropertyName("access_token")]
		public string AccessToken { get; set; }

		/// <summary>
		/// The refresh token.
		/// </summary>
		[JsonPropertyName("refresh_token")]
		public string RefreshToken { get; set; }

		/// <summary>
		/// The ID token.
		/// </summary>
		[JsonPropertyName("id_token")]
		public string IdToken { get; set; }

	}
}
