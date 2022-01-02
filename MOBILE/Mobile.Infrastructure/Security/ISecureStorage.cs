namespace Mobile.Infrastructure.Security
{
    /// <summary>
    /// Interface for securely storing data.
    /// </summary>
    public interface ISecureStorage
    {
        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <param name="data">String to store.</param>
        void Store(string key, string data);

        /// <summary>
        /// Retrieves stored data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <returns>String of stored data.</returns>
        string Retrieve(string key);

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">Key for the data to be deleted.</param>
        void Delete(string key);

        /// <summary>
        /// Checks if the storage contains a key.
        /// </summary>
        /// <param name="key">The key to search.</param>
        /// <returns>True if the storage has the key, otherwise false.</returns>
        bool Contains(string key);

        /// <summary>
        /// Clean all the data in the dictionary, mostly used for tests
        /// </summary>
        void Purge();
    }
}
