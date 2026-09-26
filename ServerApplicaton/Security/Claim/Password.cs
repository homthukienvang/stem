namespace Security.Claim
{

    /// <summary>
    /// Class Password
    /// </summary>
    public class Password
    {
        /// <summary>
        /// Gets or sets the hashed password.
        /// </summary>
        /// <value>The hashed password.</value>
        public string HashedPassword { get; set; }
        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public string Salt { get; set; }
    }
}