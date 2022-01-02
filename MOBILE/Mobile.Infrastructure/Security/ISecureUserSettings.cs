namespace Mobile.Infrastructure.Security
{
    /// <summary>
    /// Store and retrieve properties that are stored securely
    /// </summary>
    public interface ISecureUserSettings
    {
        /// <summary>
        /// Gets or sets the username, 
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the user role
        /// </summary>
        string Role { get; set; }

        /// <summary>
        /// Gets or sets the access token
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token
        /// </summary>
        string RefreshToken { get; set; }

        /// <summary>
        /// Clears all the stored settings values
        /// </summary>
        void ClearAll();
    }
}
