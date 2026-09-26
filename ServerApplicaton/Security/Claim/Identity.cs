using System;

namespace Security.Claim
{
    /// <summary>
    /// Class Identity
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public long UserId { get; set; }
        public string UserName { get; set; }
        public bool Activated { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DisplayName { get; set; }          
    }
}