using MFaaP.MFWSClient.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MFaaP.MFWSClient.OAuth2
{
	/// <summary>
	/// Represents the OAuth 2.0 configuration data within an OAuth 2.0 authentication plugin.
	/// </summary>
	public class OAuth2Configuration
	{
		public PluginInfoConfiguration PluginInfo { get; set; }

		/// <summary>
		/// The fallback redirect uri, if no others are specified.
		/// </summary>
		public const string FallbackRedirectUri = "http://localhost";

		/// <summary>
		/// The authorisation endpoint.
		/// </summary>
		/// <remarks>Should not be used directly, but instead through <see cref="GenerateAuthorizationUri"/>.</remarks>
		public string AuthorizationEndpoint { get; set; }

		/// <summary>
		/// The client ID used for OAuth.
		/// </summary>
		public string ClientID { get; set; }

		/// <summary>
		/// The current M-Files server version.
		/// </summary>
		public Version MFServerVersion { get; set; }

		/// <summary>
		/// The protocol this configuration is for.  Should be "OAuth 2.0".
		/// </summary>
		public string Protocol { get; set; } = "OAuth 2.0";

		/// <summary>
		/// The redirect URI if this configuration is used on the web.
		/// </summary>
		/// <remarks>May not be set.</remarks>
		public string RedirectURIForWeb { get; set; }

		/// <summary>
		/// The redirect URI if this configuration is used in native apps.
		/// </summary>
		/// <remarks>May not be set.</remarks>
		public string RedirectURIForNative { get; set; }

		/// <summary>
		/// The redirect URI if this configuration is used in mobile apps.
		/// </summary>
		/// <remarks>May not be set.</remarks>
		public string RedirectURIForMobile { get; set; }

		/// <summary>
		/// The redirect URI if this configuration is used via a WOPI client.
		/// </summary>
		/// <remarks>May not be set.</remarks>
		public string RedirectURIForWOPI { get; set; }

		/// <summary>
		/// The redirect URI.
		/// </summary>
		/// <remarks>May not be set.</remarks>
		public string RedirectURI { get; set; } = OAuth2Configuration.FallbackRedirectUri;

		/// <summary>
		/// The resource this configuration is for.
		/// </summary>
		public string Resource { get; set; }

		/// <summary>
		/// The token endpoint, used for swapping an authorisation code for access (and possibly refresh) tokens.
		/// </summary>
		public string TokenEndpoint { get; set; }

		/// <summary>
		/// The grant type that should be provided.
		/// </summary>
		/// <remarks>May not be set.</remarks>
		public string GrantType { get; set; } = "authorization_code";

		/// <summary>
		/// The scope that should be provided.
		/// </summary>
		/// <remarks>May not be set.</remarks>
		public string Scope { get; set; }
		
		/// <summary>
		/// The client secret that should be provided.
		/// </summary>
		/// <remarks>May not be set.</remarks>
		public string ClientSecret { get; set; }

		/// <summary>
		/// If true then users will be forced to log in, even if they've already logged into this resource.
		/// </summary>
		public bool ForceLogin { get; set; } = false;

		/// <summary>
		/// The prompt type to provide.
		/// </summary>
		public string PromptType => this.ForceLogin ? "login" : null;

		/// <summary>
		/// Whether to use the "Id token" as the access token.
		/// </summary>
		public bool UseIdTokenAsAccessToken { get; set; } = false;

		/// <summary>
		/// Whether to use PKCE.
		/// </summary>
		public bool UsePkceWithAuthorizationCode { get; set; } = false;

		protected internal string CodeVerifier = null;
		protected string CodeChallenge = null;

        /// <summary>
        /// The site realm to provide.
        /// </summary>
        public string SiteRealm { get; set; }

		/// <summary>
		/// The OAuth state to provide and verify.  Defaults to a new GUID.
		/// If set to null or an empty string then will not be provided.
		/// </summary>
		public string State { get; set; } = Guid.NewGuid().ToString("B");

		/// <summary>
		/// The vault guid this relates to.
		/// </summary>
		public string VaultGuid { get; set; }

		/// <summary>
		/// Instantiates the configuration class.
		/// </summary>
		/// <param name="forceLogin">If true, the user will be prompted to log in even if they have done so already (<see cref="ForceLogin"/>).</param>
		public OAuth2Configuration(bool forceLogin = false)
			: base()
		{
			this.ForceLogin = forceLogin;
		}

		/// <summary>
		/// Instantiates the configuration class, retrieving configuration values from the
		/// provided plugin configuration.
		/// </summary>
		/// <param name="pluginConfiguration">The dictionary containing plugin configuration settings.</param>
		/// <param name="vaultGuid">The GUID of the vault.</param>
		/// <param name="forceLogin">If true, the user will be prompted to log in even if they have done so already (<see cref="ForceLogin"/>).</param>
		public OAuth2Configuration(PluginInfoConfiguration plugin, bool forceLogin = false)
			: this(forceLogin)
		{
            // Sanity.
            if (null == plugin)
                throw new ArgumentNullException(nameof(plugin));
			var pluginConfiguration = plugin.Configuration ?? new Dictionary<string, string>();
			var vaultGuid = plugin.VaultGuid;

			// Extract data.
			this.PluginInfo = plugin;
            this.AuthorizationEndpoint = pluginConfiguration.GetValueOrDefault("AuthorizationEndpoint");
			this.ClientID = pluginConfiguration.GetValueOrDefault("ClientID");
			this.Protocol = pluginConfiguration.GetValueOrDefault("Protocol");
			this.RedirectURIForWeb = pluginConfiguration.GetValueOrDefault("RedirectURIForWeb");
			this.RedirectURIForNative = pluginConfiguration.GetValueOrDefault("RedirectURIForNative");
			this.RedirectURIForWOPI = pluginConfiguration.GetValueOrDefault("RedirectURIForWOPI");
			this.RedirectURIForMobile = pluginConfiguration.GetValueOrDefault("RedirectURIForMobile");
			this.RedirectURI = pluginConfiguration.GetValueOrDefault("RedirectURI", OAuth2Configuration.FallbackRedirectUri);
			this.Resource = pluginConfiguration.GetValueOrDefault("Resource");
			this.TokenEndpoint = pluginConfiguration.GetValueOrDefault("TokenEndpoint");
			this.Scope = pluginConfiguration.GetValueOrDefault("Scope");
			this.ClientSecret = pluginConfiguration.GetValueOrDefault("ClientSecret");
			this.SiteRealm = pluginConfiguration.GetValueOrDefault("SiteRealm");
			this.VaultGuid = vaultGuid;
			try { this.MFServerVersion = Version.Parse(pluginConfiguration.GetValueOrDefault("MFServerVersion")); }
			catch { }

            // If the configuration tells us to use the id token instead of the access token then we should.
            this.UseIdTokenAsAccessToken = string.Equals(
                pluginConfiguration.GetValueOrDefault("UseIdTokenAsAccessToken"),
                "true",
                StringComparison.InvariantCultureIgnoreCase);

            // If the configuration tells us to use PKCE then we should.
            this.UsePkceWithAuthorizationCode = string.Equals(
                pluginConfiguration.GetValueOrDefault("UsePkceWithAuthorizationCode"),
                "true",
                StringComparison.InvariantCultureIgnoreCase);

            // If the configuration asks us to force the login then do so.
            if (pluginConfiguration.GetValueOrDefault("PromptLoginParameter") == "login")
			{
				this.ForceLogin = true;
			}
		}

		/// <summary>
		/// Creates an <see cref="OAuth2Configuration"/> by parsing the provided plugin configuration.
		/// </summary>
		/// <param name="pluginConfiguration">The dictionary containing plugin configuration settings.</param>
		/// <param name="vaultGuid">The GUID of the vault.</param>
		/// <returns>A configuration class representing the provided details.</returns>
		public static OAuth2Configuration ParseFrom(PluginInfoConfiguration plugin)
		{
			return new OAuth2Configuration(plugin);
		}

		/// <summary>
		/// Retrieves the appropriate redirect uri from the various ones provided in this configuration.
		/// This method ensures that the logic for which redirect URI is used remains constant.
		/// </summary>
		/// <returns>The redirect uri to use.</returns>
		/// <remarks>The standard implementation will check the following values in this order, using the first one that has a value:
		/// <see cref="RedirectURIForNative"/>, <see cref="RedirectURI"/>, <see cref="FallbackRedirectUri"/>.
		/// </remarks>
		public virtual string GetAppropriateRedirectUri()
		{
			if (false == string.IsNullOrWhiteSpace(this.RedirectURIForNative))
			{
				// Use the native redirect URI.
				return this.RedirectURIForNative;
			}
			else if(false == string.IsNullOrWhiteSpace(this.RedirectURI))
			{
				// Use the redirect URI.
				return this.RedirectURI;
			}
			return OAuth2Configuration.FallbackRedirectUri;
		}

		/// <summary>
		/// Generates a <see cref="Uri"/> that can be displayed in a web browser for authorisation.
		/// Includes both the <see cref="AuthorizationEndpoint"/> and also other required information such as
		/// <see cref="ClientID"/> and <see cref="Resource"/>.
		/// </summary>
		/// <param name="redirectUri">
		/// The redirect Uri to use.
		/// If null or a blank string then this method will attempt to retrieve a valid redirect uri from the current configuration.
		/// If that also cannot be found, will use <see cref="OAuth2Configuration.FallbackRedirectUri"/>.</param>
		/// <returns></returns>
		public Uri GenerateAuthorizationUri(string redirectUri = null)
		{
			// If we do not have a redirect uri then try and make one.
			if (string.IsNullOrWhiteSpace(redirectUri))
			{
				redirectUri = this.GetAppropriateRedirectUri();
			}

			// Build up the URI with mandatory data.
			var uriBuilder = new UriBuilder(this.AuthorizationEndpoint);
			uriBuilder.SetQueryParam("client_id", this.ClientID);
			uriBuilder.SetQueryParam("redirect_uri", redirectUri);
			uriBuilder.SetQueryParam("response_type", "code");

			// Add the optional items, if set.
			uriBuilder.SetQueryParamIfNotNullOrWhitespace("scope", this.Scope);
			uriBuilder.SetQueryParamIfNotNullOrWhitespace("state", this.State);
			uriBuilder.SetQueryParamIfNotNullOrWhitespace("prompt", this.PromptType);
			uriBuilder.SetQueryParamIfNotNullOrWhitespace("resource", this.Resource);

			// If using PKCE then add the required data.
			if(this.UsePkceWithAuthorizationCode)
			{
				// Create the verifier and challenge.
				this.CodeVerifier = this.CreateCodeVerifier();
				this.CodeChallenge = this.CreateCodeChallenge();

                // Add the challenge to the URI.
                uriBuilder.SetQueryParam("code_challenge", this.CodeChallenge);
                uriBuilder.SetQueryParam("code_challenge_method", "S256");
            }

            // Return the generated URI.
            return uriBuilder.Uri;
		}

		protected string CreateCodeVerifier(int size = 32)
		{
			if (size > 1024)
				size = 32;

			using (var cryptoProvider = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[size];
                cryptoProvider.GetBytes(bytes);

                return Convert.ToBase64String(bytes)
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");
            }
        }

		protected string CreateCodeChallenge(string codeVerifier = null)
        {
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier ?? this.CodeVerifier));
                return Convert.ToBase64String(challengeBytes)
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");
            }
        }
	}
}
